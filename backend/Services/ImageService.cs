using AutoMapper;
using backend.Models.Dto;
using backend.Models.Entity;

namespace backend.Services
{
    public class ImageService
    {
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _dbContext;

        public ImageService(IWebHostEnvironment env, IMapper mapper, ApplicationDbContext dbContext)
        {
            _env = env;
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public string GetImagePath(Guid imageId)
        {
            var folderPath = Path.Combine(_env.WebRootPath, "Images");
            var imageFileName = $"{imageId}.jpg"; // Adjust the extension based on your image format
            var imagePath = Path.Combine(folderPath, imageFileName);

            if (File.Exists(imagePath))
            {
                return imagePath;
            }
            else
            {
                throw new InvalidOperationException($"Image file with Id {imageId} not found in the specified folder.");
            }
        }

        public ImageDto SaveImage(IFormFile imageFile)
        {
            // Create a new ImageDto with default values
            var imageDto = new ImageDto
            {
                Id = Guid.NewGuid(),
                generatedCaption = null,
                // Add other properties as needed
            };

            // Process and save the image file to the wwwroot/Images folder
            var imageFolderPath = Path.Combine(_env.WebRootPath, "Images");
            var imageFileName = $"{imageDto.Id}.jpg"; // Adjust the extension based on your image format
            var imageFilePath = Path.Combine(imageFolderPath, imageFileName);

            using (var stream = new FileStream(imageFilePath, FileMode.Create))
            {
                imageFile.CopyTo(stream);
            }

            // Map ImageDto to Image entity
            var newImage = _mapper.Map<Image>(imageDto);

            // Set other properties or perform additional processing as needed

            // Save the Image entity to the database
            _dbContext.Images.Add(newImage);
            _dbContext.SaveChanges();

            // Return the updated ImageDto
            return _mapper.Map<ImageDto>(newImage);
        }

        public void InitDirectories()
        {
            //Create the image directory if it does not exist
            var imageFolderPath = Path.Combine(_env.WebRootPath, "Images");
            if (!Directory.Exists(imageFolderPath))
            {
                Directory.CreateDirectory(imageFolderPath);
            }
        }

    }
}
