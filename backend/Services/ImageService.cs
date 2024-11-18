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

namespace backend.Services
{
    public class ImageService : BaseFileContentService<Image, ImageDto>
    {

        private readonly AIService _aiService;

        public ImageService(IWebHostEnvironment env, IMapper mapper, ApplicationDbContext dbContext, MongoDBRepository mongoRepo, AnyFileRepository anyFileRepository, UserManager<ApplicationUser> userManager, IdentityService identityService, IHttpContextAccessor httpContextAccessor, AIService aiService)
            : base(env, mapper, dbContext, mongoRepo, anyFileRepository, userManager, identityService, httpContextAccessor)
        {

        }

        public IQueryable<ImageDto> GetImagesByCaption(string caption)
        {
            // Filter images based on the provided caption (case-insensitive)
            var filteredEntities = _dbContext.Set<Image>()
                .Where(image => image.GeneratedCaption != null && image.GeneratedCaption.ToLower().Contains(caption.ToLower()))
                .AsQueryable();

            var filteredDtoList = _mapper.ProjectTo<ImageDto>(filteredEntities);
            return filteredDtoList;
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
                                         .OrderBy(image => image.Id) // Ensure there is a defined order
                                         .Take(count)
                                         .Select(image => image.Id.ToString())
                                         .ToListAsync();


                if (imageIds == null || imageIds.Count == 0)
                {
                    Console.WriteLine("No image IDs found.");
                    return null;  // Return early if no image IDs are found.
                }

                // Build image URLs
                var imageUrls = imageIds.Select(id => $"http://127.0.0.1:8081/api/Image/{id}/Content").ToList();

                // Call the AI service to embed images
                var embeddings = await _aiService.EmbedImagesAsync(imageUrls);

                if (embeddings == null || !embeddings.Any())
                {
                    Console.WriteLine("No embeddings returned.");
                    return null;  // Handle the case where embeddings are not returned.
                }

                // Process further logic here (e.g., save embeddings in the database)
                return embeddings;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return null;
            }
        }


    }
}
