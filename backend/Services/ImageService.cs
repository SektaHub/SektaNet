using AutoMapper;
using backend.Models;
using backend.Models.Common;
using backend.Models.Dto;
using backend.Models.Entity;
using backend.Repo;
using backend.Services.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pgvector.EntityFrameworkCore;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using static System.Net.WebRequestMethods;
using Pgvector;
using SharpCompress.Common;

namespace backend.Services
{
    public class ImageService : BaseFileContentService<Image, ImageDto>
    {

        private readonly AIService _aiService;

        public ImageService(IWebHostEnvironment env, IMapper mapper, ApplicationDbContext dbContext, MongoDBRepository mongoRepo, AnyFileRepository anyFileRepository, UserManager<ApplicationUser> userManager, IdentityService identityService, IHttpContextAccessor httpContextAccessor, AIService aiService)
        : base(env, mapper, dbContext, mongoRepo, anyFileRepository, userManager, identityService, httpContextAccessor)
        {
            // Assign the AIService to the field
            _aiService = aiService ?? throw new ArgumentNullException(nameof(aiService));
        }

        public async Task<PaginatedResponseDto<ImageDto>> GetPaginated(int page, int pageSize, string? captionSearch)
        {
            // Use GetAllowed to get the queryable list of allowed entities
            var allowedEntities = GetAllowed();

            // Apply additional filtering based on query parameters
            if (!string.IsNullOrEmpty(captionSearch))
            {
                allowedEntities = allowedEntities.Where(image => image.GeneratedCaption != null && image.GeneratedCaption.ToLower().Contains(captionSearch.ToLower()));
            }

            // Get the total count of filtered entities
            var totalCount = await allowedEntities.CountAsync();

            // Apply pagination
            var paginatedEntities = await allowedEntities.Skip((page - 1) * pageSize)
                                                         .Take(pageSize)
                                                         .ToListAsync();

            // Map entities to DTOs
            var dtoList = _mapper.Map<List<ImageDto>>(paginatedEntities);

            // Create the paginated response
            var response = new PaginatedResponseDto<ImageDto>
            {
                Items = dtoList,
                TotalCount = totalCount
            };

            return response;
        }


        public async Task<PaginatedResponseDto<ImageDto>> GetPaginatedBySemanticAsync(int page, int pageSize, string? captionSearch)
        {

            // Use GetAllowed to get the queryable list of allowed entities
            var allowedEntities = GetAllowed();

            // Apply additional filtering based on query parameters
            if (!string.IsNullOrEmpty(captionSearch))
            {

                // Generate the query vector for the user query (caption or other text)
                var queryVector = await _aiService.EmbedTextAsync(captionSearch);

                if (queryVector == null)
                {
                    return new PaginatedResponseDto<ImageDto>
                    {
                        Items = new List<ImageDto>(),
                        TotalCount = 0
                    };
                }

                allowedEntities = allowedEntities.Where(image => image.ClipEmbedding != null)
                                .OrderBy(image => image.ClipEmbedding!.CosineDistance(queryVector));
            }

            // Get the total count of filtered entities
            var totalCount = await allowedEntities.CountAsync();

            // Apply pagination
            var paginatedEntities = await allowedEntities.Skip((page - 1) * pageSize)
                                                         .Take(pageSize)
                                                         .ToListAsync();

            // Map entities to DTOs
            var dtoList = _mapper.Map<List<ImageDto>>(paginatedEntities);

            // Create the paginated response
            var response = new PaginatedResponseDto<ImageDto>
            {
                Items = dtoList,
                TotalCount = totalCount
            };

            return response;

        }

        public async Task<List<ImageDto>> GetVisuallySimmilar(Guid id)
        {
            // Find the image by id
            var entity = await _dbContext.Set<Image>().FindAsync(id);

            if (entity == null)
            {
                return null;
            }

            // Use GetAllowed to get the queryable list of allowed entities
            var allowedEntities = GetAllowed();

            // Filter out the current image and order by similarity
            var similarEntities = await allowedEntities
                .Where(x => x.Id != id)
                .OrderBy(x => x.ClipEmbedding!.L2Distance(entity.ClipEmbedding))
                .Take(4)
                .ToListAsync();

            // Map List<Image> to List<ImageDto>
            var dtos = _mapper.Map<List<ImageDto>>(similarEntities);
            return dtos;
        }

        public IQueryable<ImageDto> GetImagesWithoutCaption()
        {
            // Filter images where GeneratedCaption is null
            var filteredEntities = _dbContext.Set<Image>()
                .Where(image => image.GeneratedCaption == null)
                .AsQueryable();

            var filteredDtoList = _mapper.ProjectTo<ImageDto>(filteredEntities);
            return filteredDtoList;
        }

        public async Task DeleteImage(string id)
        {
            await _anyFileRepository.DeleteImage(id);
        }

        public async Task<Image> FindImageByOriginalSource(string originalSource)
        {
            return await _dbContext.Set<Image>()
                .FirstOrDefaultAsync(image => image.OriginalSource == originalSource);
        }

        public async Task<List<Vector>> ProcessAndEmbedImages(int count = 100)
        {
            try
            {
                // Select the first 'count' image IDs from the database
                List<string> imageIds = await _dbContext.Set<Image>()
                                             .Where(image => image.ClipEmbedding == null)
                                             .Where(image => new[] { "jpeg", "jpg", "png" }.Contains(image.FileExtension.ToLower())) // Filter by file extension
                                             .Take(count)
                                             .Select(image => image.Id.ToString())
                                             .ToListAsync();

                // Check if the imageIds list is empty or null
                if (imageIds == null || !imageIds.Any())
                {
                    Console.WriteLine("No image IDs found.");
                    return null;  // Return early if no image IDs are found.
                }

                // Build image URLs
                var imageUrls = imageIds.Select(id => $"http://127.0.0.1:8081/api/Image/{id}/Content").ToList();

                // Call the AI service to embed images
                var embeddings = await _aiService.EmbedImagesAsync(imageUrls);

                // Check if the embeddings are null or empty
                if (embeddings == null || !embeddings.Any())
                {
                    Console.WriteLine("No embeddings returned.");
                    return null;  // Handle the case where embeddings are not returned.
                }

                // Retrieve the images from the database by their IDs
                var images = await _dbContext.Set<Image>()
                                              .Where(image => imageIds.Contains(image.Id.ToString()))
                                              .ToListAsync();

                if (images.Count != embeddings.Count)
                {
                    Console.WriteLine("Mismatch between the number of images and embeddings.");
                    return null;  // Ensure that the number of images matches the number of embeddings
                }

                // Update each image's ClipEmbedding with the corresponding embedding
                for (int i = 0; i < images.Count; i++)
                {
                    images[i].ClipEmbedding = embeddings[i];
                }

                // Save the changes to the database
                await _dbContext.SaveChangesAsync();

                // Return the embeddings (or perform additional logic as needed)
                return embeddings;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in ImageService: {ex.Message}");
                return null;
            }
        }



    }
}
