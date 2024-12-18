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
        private readonly string _aiEndpoint;

        public AIService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _aiEndpoint = configuration.GetSection("SektaNetAI")["Endpoint"];
        }

        public class EmbeddingResponse
        {
            public List<List<float>> Embeddings { get; set; }
            public float ExecutionTime { get; set; }
        }


        public async Task<List<Vector>> EmbedImagesAsync(List<string> imageUris)
        {
            try
            {
                // Log the image URIs being sent
                //Console.WriteLine("Requesting embeddings for the following image URIs:");
                foreach (var uri in imageUris)
                {
                    Console.WriteLine(uri);
                }

                var requestBody = new
                {
                    data = imageUris.Select(uri => new { uri }).ToList()
                };

                // Serialize the request body to JSON
                var jsonRequest = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                // Log the request body for debugging
                //Console.WriteLine("Request Body: " + jsonRequest);

                // Send the HTTP request to the AI service
                var response = await _httpClient.PostAsync(_aiEndpoint+"clip/embed/", content);

                // Log the response for debugging purposes
                //Console.WriteLine("AI_RESPONSE Status Code: " + response.StatusCode);
                var responseContent = await response.Content.ReadAsStringAsync();
                //Console.WriteLine("AI_RESPONSE Content: " + responseContent);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Check for empty response content
                    if (string.IsNullOrEmpty(responseContent))
                    {
                        Console.WriteLine("Empty response content.");
                        return null;
                    }

                    // Deserialize the response content into an EmbeddingResponse object
                    var embeddingResponse = JsonConvert.DeserializeObject<EmbeddingResponse>(responseContent);

                    // Check if embeddings are found
                    if (embeddingResponse?.Embeddings == null || embeddingResponse.Embeddings.Count == 0)
                    {
                        Console.WriteLine("No embeddings found in the response.");
                        return null;
                    }

                    // Convert the List<List<float>> into a List<Vector> if needed
                    return embeddingResponse.Embeddings.Select(e => new Vector(e.ToArray())).ToList();
                }
                else
                {
                    // Log the error if the request failed
                    Console.WriteLine($"AI Service Error: {response.ReasonPhrase}");
                    return null;  // Handle the error case
                }
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine($"HTTP Request Exception: {httpEx.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in AiService: {ex.Message}");
                return null;  // Return null in case of any other exception
            }
        }


        public async Task<Vector> EmbedTextAsync(string text)
        {
            try
            {
                // Log the text being sent for embedding
                Console.WriteLine($"Requesting embedding for the following text: {text}");

                if (string.IsNullOrEmpty(text))
                {
                    return null;
                }

                var requestBody = new
                {
                    data = new List<object>
            {
                new { text = text }
            }
                };

                // Serialize the request body to JSON
                var jsonRequest = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                // Send the HTTP request to the AI service
                var response = await _httpClient.PostAsync(_aiEndpoint + "clip/embed/", content);

                // Log the response for debugging
                var responseContent = await response.Content.ReadAsStringAsync();

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Check for empty response content
                    if (string.IsNullOrEmpty(responseContent))
                    {
                        Console.WriteLine("Empty response content.");
                        return null;
                    }

                    // Deserialize the response content into an EmbeddingResponse object
                    var embeddingResponse = JsonConvert.DeserializeObject<EmbeddingResponse>(responseContent);

                    // Check if embeddings are found
                    if (embeddingResponse?.Embeddings == null || embeddingResponse.Embeddings.Count == 0)
                    {
                        Console.WriteLine("No embeddings found in the response.");
                        return null;
                    }

                    // Convert the List<List<float>> into a List<Vector>
                    return new Vector(embeddingResponse.Embeddings[0].ToArray()); // Assuming only one text input
                }
                else
                {
                    // Log the error if the request failed
                    Console.WriteLine($"AI Service Error: {response.ReasonPhrase}");
                    return null;  // Handle the error case
                }
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine($"HTTP Request Exception: {httpEx.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in AiService: {ex.Message}");
                return null;  // Return null in case of any other exception
            }
        }




    }
}
