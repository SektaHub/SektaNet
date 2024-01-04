using AutoMapper;
using backend.Models.Dto;
using backend.Models.Entity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;
using System.Web;

namespace backend.Services
{
    public class ImageService
    {
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _dbContext;
        private readonly HttpClient _httpClient;

        public ImageService(IWebHostEnvironment env, IMapper mapper, ApplicationDbContext dbContext, HttpClient httpClient)
        {
            _env = env;
            _mapper = mapper;
            _dbContext = dbContext;
            _httpClient = httpClient;
            _httpClient = httpClient;
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

            // Set other properties or perform additional processing as needed

            // Map ImageDto to Image entity
            var newImage = _mapper.Map<Image>(imageDto);

            // Save the Image entity to the database
            _dbContext.Images.Add(newImage);
            _dbContext.SaveChanges();

            // Return the updated ImageDto
            return _mapper.Map<ImageDto>(newImage);
        }

        public async Task<ImageDto> SaveImageAsync(IFormFile imageFile)
        {
            // Process and save the image file to the wwwroot/Images folder
            var imageFolderPath = Path.Combine(_env.WebRootPath, "Images");
            Guid imageId = Guid.NewGuid();
            var imageFileName = $"{imageId}.jpg"; // Adjust the extension based on your image format
            var imageFilePath = Path.Combine(imageFolderPath, imageFileName);

            using (var stream = new FileStream(imageFilePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            // Generate caption using FastAPI
            var apiUrl = "http://127.0.0.1:8000/api/generateCaptionFromUpload/";
            var apiUrl2 = "http://localhost:8000/api/embed_sentence";

            var jsonResponse = await GenerateCaptionFromFastAPIAsync(apiUrl, imageFilePath);

            //Console.WriteLine(jsonResponse.ToString());

            var caption = JObject.Parse(jsonResponse)?["caption"]?.ToString() ?? null;

            var jsonResponse2 = await EmbedSentenceUsingFastAPIAsync(apiUrl2, caption);

            //Console.WriteLine(jsonResponse2.ToString());

            var captionEmbedding = JObject.Parse(jsonResponse2)?["embedding"]?.ToObject<List<float>>() ?? null;

            // Create a new ImageDto with default values
            var imageDto = new ImageDto
            {
                Id = imageId,
                generatedCaption = caption,
                CaptionEmbedding = new Pgvector.Vector(captionEmbedding.ToArray())
            };

            // Map ImageDto to Image entity
            var newImage = _mapper.Map<Image>(imageDto);

            // Save the Image entity to the database
            _dbContext.Images.Add(newImage);
            await _dbContext.SaveChangesAsync();

            // Return the updated ImageDto
            return _mapper.Map<ImageDto>(newImage);
        }

        private async Task<string> EmbedSentenceUsingFastAPIAsync(string apiUrl, string sentence)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    // Prepare the content with the sentence
                    var content = new StringContent($"{{\"sentence\": \"{sentence}\"}}", Encoding.UTF8, "application/json");

                    // Send a POST request to the FastAPI endpoint
                    var response = await httpClient.PostAsync(apiUrl, content);
                    response.EnsureSuccessStatusCode();

                    // Read and return the response content
                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions or log errors
                return $"Error communicating with FastAPI: {ex.Message}";
            }
        }

        private async Task<string> GenerateCaptionFromFastAPIAsync(string apiUrl, string imagePath)
        {
            try
            {
                // Use HttpClient to send a POST request to FastAPI
                using (var httpClient = new HttpClient())
                using (var content = new MultipartFormDataContent())
                using (var fileStream = File.OpenRead(imagePath))
                {
                    content.Add(new StreamContent(fileStream), "file", Path.GetFileName(imagePath));

                    var response = await httpClient.PostAsync(apiUrl, content);
                    response.EnsureSuccessStatusCode();

                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions or log errors
                return $"Error communicating with FastAPI: {ex.Message}";
            }
        }

        private async Task<string> GenerateCaptionEmbeddingFromFastAPIAsync(string apiUrl, string caption)
        {
            try
            {
                using (var httpClient = new HttpClient())
                using (var content = new MultipartFormDataContent())
                {
                    // Add the caption to the request
                    content.Add(new StringContent(caption), "caption");

                    var response = await httpClient.PostAsync(apiUrl, content);
                    response.EnsureSuccessStatusCode();

                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions or log errors
                return $"Error communicating with FastAPI: {ex.Message}";
            }
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

        public async Task<string> GenerateCaptionFromUploadAsync(IFormFile imageFile)
        {
            // Save the image locally
            var savedImage = SaveImage(imageFile);

            // Generate caption using FastAPI
            var apiUrl = "http://127.0.0.1:8000/api/generateCaptionFromUpload/";
            var response = await SendImageToFastAPI(apiUrl, savedImage);

            return response;
        }

        public async Task<string> GenerateCaptionFromLinkAsync(string imageLink)
        {
            // Generate caption using FastAPI
            var apiUrl = "http://127.0.0.1:8000/api/generateCaptionFromLink/";
            var response = await SendImageToFastAPI(apiUrl, new { image_link = imageLink });

            return response;
        }

        private async Task<string> SendImageToFastAPI(string apiUrl, object payload)
        {
            try
            {
                // Use HttpClient to send a POST request to FastAPI
                var response = await _httpClient.PostAsJsonAsync(apiUrl, payload);

                // Check if the request was successful
                response.EnsureSuccessStatusCode();

                // Read the response content (captions generated by FastAPI)
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                // Handle exceptions or log errors
                return $"Error communicating with FastAPI: {ex.Message}";
            }
        }

    }
}
