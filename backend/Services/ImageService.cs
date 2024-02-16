using AutoMapper;
using backend.Models.Dto;
using backend.Models.Entity;
using backend.Services.Common;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;
using System.Web;

namespace backend.Services
{
    public class ImageService : BaseFileContentService<Image, ImageDto>
    {

        public ImageService(IWebHostEnvironment env, ApplicationDbContext dbContext, IMapper mapper) : base(env, dbContext, mapper)
        {
            FolderName = "Images";
        }

        public async override Task<List<ImageDto>> UploadMultiple(List<IFormFile> files)
        {
            // ...omitting initial checks and try-catch for brevity

            List<string> imagePaths = new List<string>();
            List<ImageDto> imageDtos = new List<ImageDto>();

            foreach (var imageFile in files)
            {
                if (imageFile == null || imageFile.Length == 0) continue;

                var imageId = new ObjectId();

                var imageDto = new ImageDto
                {
                    Id = imageId,
                    FileExtension = imageFile.ContentType.Split('/')[1],
                    generatedCaption = null,
                    CaptionEmbedding = null,
                };
                imageDtos.Add(imageDto);

                var imagePath = await SaveFile(imageFile, imageId, imageDto.FileExtension);
                imagePaths.Add(imagePath);
            }

            // Ensure all files are saved before proceeding
            for (int i = 0; i < imagePaths.Count; i++)
            {
                var imageDto = imageDtos[i];
                var imagePath = imagePaths[i];

                // Additional processing or actions after successful image uploads

                // Save the Image entity to the database
                var newImage = _mapper.Map<Image>(imageDto);
                _dbContext.Images.Add(newImage);
            }
            _dbContext.SaveChanges();

            // Additional processing or actions after saving images to the database
            return imageDtos;
        }

    }
}
