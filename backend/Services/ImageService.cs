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

namespace backend.Services
{
    public class ImageService : BaseFileContentService<Image, ImageDto>
    {
        public ImageService(IWebHostEnvironment env, IMapper mapper, ApplicationDbContext dbContext, MongoDBRepository mongoRepo, AnyFileRepository anyFileRepository, UserManager<ApplicationUser> userManager) : base(env, mapper, dbContext, mongoRepo, anyFileRepository, userManager)
        {

        }


        //public async override Task<List<ImageDto>> UploadMultiple(List<IFormFile> files)
        //{
        //    // ...omitting initial checks and try-catch for brevity

        //    List<string> imagePaths = new List<string>();
        //    List<ImageDto> imageDtos = new List<ImageDto>();

        //    foreach (var imageFile in files)
        //    {
        //        if (imageFile == null || imageFile.Length == 0) continue;

        //        string imageId = ObjectId.GenerateNewId().ToString();

        //        var imageDto = new ImageDto
        //        {
        //            Id = imageId,
        //            FileExtension = imageFile.ContentType.Split('/')[1],
        //            generatedCaption = null,
        //            CaptionEmbedding = null,
        //        };
        //        imageDtos.Add(imageDto);

        //        var imagePath = await SaveFile(imageFile, imageId, imageDto.FileExtension);
        //        imagePaths.Add(imagePath);
        //    }

        //    // Ensure all files are saved before proceeding
        //    for (int i = 0; i < imagePaths.Count; i++)
        //    {
        //        var imageDto = imageDtos[i];
        //        var imagePath = imagePaths[i];

        //        // Additional processing or actions after successful image uploads

        //        // Save the Image entity to the database
        //        var newImage = _mapper.Map<Image>(imageDto);
        //        _dbContext.Images.Add(newImage);
        //    }
        //    _dbContext.SaveChanges();

        //    // Additional processing or actions after saving images to the database
        //    return imageDtos;
        //}

        public IQueryable<ImageDto> GetImagesByCaption(string caption)
        {
            // Filter images based on the provided caption (case-insensitive)
            var filteredEntities = _dbContext.Set<Image>()
                .Where(image => image.GeneratedCaption != null && image.GeneratedCaption.ToLower().Contains(caption.ToLower()))
                .AsQueryable();

            var filteredDtoList = _mapper.ProjectTo<ImageDto>(filteredEntities);
            return filteredDtoList;
        }

        public async Task<PaginatedResponseDto<ImageDto>> GetPaginated(int page, int pageSize, string? captionSearch, string? userId)
        {
            IQueryable<Image> query = _dbContext.Set<Image>();

            if (!string.IsNullOrEmpty(captionSearch))
            {
                query = query.Where(image => image.GeneratedCaption != null && image.GeneratedCaption.ToLower().Contains(captionSearch.ToLower()));
            }

            var user = await _userManager.FindByIdAsync(userId);
            bool isAdmin = await _userManager.IsInRoleAsync(user, "admin") || await _userManager.IsInRoleAsync(user, "sektash");

            // Filter images based on ownership or not private
            query = query.Where(image => !image.isPrivate || image.OwnerId == userId || isAdmin);

            var totalCount = query.Count();
            
            var entities = query.Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();

            var dtoList = _mapper.Map<List<ImageDto>>(entities);

            var response = new PaginatedResponseDto<ImageDto>
            {
                Items = dtoList,
                TotalCount = totalCount
            };

            return response;
        }


        public async Task<List<ImageDto>> GetVisuallySimmilar(Guid id, string? userId)
        {
            var entity = await _dbContext.Set<Image>().FindAsync(id); // Ensure asynchronous operation

            if (entity == null)
            {
                return null;
            }

            var imageDto = _mapper.Map<ImageDto>(entity);

            var user = await _userManager.FindByIdAsync(userId);
            bool isAdmin = await _userManager.IsInRoleAsync(user, "admin") || await _userManager.IsInRoleAsync(user, "sektash");

            // Retrieve images that the user can access
            var entities = await _dbContext.Set<Image>()
                .Where(x => x.Id != id && (!x.isPrivate || x.OwnerId == userId || isAdmin))
                .OrderBy(x => x.ClipEmbedding!.L2Distance(imageDto.ClipEmbedding))
                .Take(4)
                .ToListAsync();

            // Map List<Image> to List<ImageDto>
            var dtos = _mapper.Map<List<ImageDto>>(entities);
            return dtos;
        }


        //public async Task<List<ImageDto>> GetVisuallySimmilar(string id, float maxDistance)
        //{
        //    var entity = await _dbContext.Set<Image>().FindAsync(id); // Ensure asynchronous operation

        //    if (entity == null)
        //    {
        //        return null;
        //    }

        //    var imageDto = _mapper.Map<ImageDto>(entity);

        //    var entities = await _dbContext.Set<Image>()
        //        .Where(x => x.Id != id && x.ClipEmbedding != null && x.ClipEmbedding.L2Distance(imageDto.ClipEmbedding) <= maxDistance)
        //        .OrderBy(x => x.ClipEmbedding!.L2Distance(imageDto.ClipEmbedding))
        //        .Take(4)
        //        .ToListAsync();

        //    // Map List<Image> to List<ImageDto>
        //    var dtos = _mapper.Map<List<ImageDto>>(entities);
        //    return dtos;
        //}

        public IQueryable<ImageDto> GetImagesWithoutCaption()
        {
            // Filter images where GeneratedCaption is null
            var filteredEntities = _dbContext.Set<Image>()
                .Where(image => image.GeneratedCaption == null)
                .AsQueryable();

            var filteredDtoList = _mapper.ProjectTo<ImageDto>(filteredEntities);
            return filteredDtoList;
        }

        //public async Task<string> UploadImage(IFormFile image)
        //{
        //    return await _anyFileRepository.SaveImage(image);
        //}

        public async Task DeleteImage(string id)
        {
            await _anyFileRepository.DeleteImage(id);
        }

    }
}
