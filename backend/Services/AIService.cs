using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pgvector;

namespace backend.Services
{
    public class AIService
    {
        private readonly HttpClient _httpClient;
        private const string AI_ENDPOINT = "http://localhost:8000/clip/embed/";

        public AIService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<Vector>> EmbedImagesAsync(List<string> imageUris)
        {
            try
            {
                // Prepare the request data
                var requestBody = new
                {
                    data = new List<object>()
                };

                foreach (var uri in imageUris)
                {
                    requestBody.data.Add(new
                    {
                        uri = uri
                    });
                }

                // Serialize the request body to JSON
                var jsonRequest = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                // Send the POST request to the Python microservice
                var response = await _httpClient.PostAsync(AI_ENDPOINT, content);

                Console.WriteLine(response.Content);

                // Check for success
                if (response.IsSuccessStatusCode)
                {
                    // Parse the JSON response from the AI service
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var responseJson = JObject.Parse(responseContent);

                    // Assuming the embeddings are in the "embeddings" field and are lists of floats
                    var embeddings = responseJson["embeddings"]
                                                .Select(token => token.ToObject<List<float>>())
                                                .Select(embedding => new Vector(new ReadOnlyMemory<float>(embedding.ToArray())))  // Convert to ReadOnlyMemory<float>
                                                .ToList();

                    Console.WriteLine("Images embedded successfully.");
                    return embeddings;  // Return the list of Pgvector.Vectors (embeddings)
                }
                else
                {
                    Console.WriteLine($"Error: {response.ReasonPhrase}");
                    return null;  // Return null if embedding fails
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return null;  // Return null if an exception occurs
            }
        }




    }
}
