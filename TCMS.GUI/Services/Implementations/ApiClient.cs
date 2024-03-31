using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TCMS.GUI.Services.Interfaces;

namespace TCMS.GUI.Services.Implementations
{
    // The ApiClient class implements the IApiClient interface.
    // This class provides a simplified way to make HTTP requests to a RESTful API.
    public class ApiClient : IApiClient
    {
        // An instance of HttpClient provided by dependency injection.
        // Used to send HTTP requests and receive HTTP responses from a resource identified by a URI.
        private readonly HttpClient _httpClient;

        // Constructor accepting an HttpClient instance.
        // This allows for configuring the HttpClient instance outside of this class, promoting flexibility and testability.
        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Public method to perform a GET request.
        // Generic <T> indicates this method returns a task of type T, allowing it to be used with various return types.
        public Task<T> GetAsync<T>(string endpoint) => SendAsync<T>(HttpMethod.Get, endpoint);

        // Public method to perform a POST request.
        // The 'data' parameter represents the payload to be sent to the server.
        public Task<T> PostAsync<T>(string endpoint, object data) => SendAsync<T>(HttpMethod.Post, endpoint, data);

        // Public method to perform a PUT request.
        // Similar to PostAsync, but semantically used for updating resources.
        public Task<T> PutAsync<T>(string endpoint, object data) => SendAsync<T>(HttpMethod.Put, endpoint, data);

        // Public method to perform a DELETE request.
        // Typically used to delete a resource identified by the endpoint URI.
        public Task DeleteAsync(string endpoint) => SendAsync(HttpMethod.Delete, endpoint);
        // Additional HTTP methods like PATCH or OPTIONS could be implemented similarly.

        // Private helper method without a return type for operations that do not return data, e.g., DELETE.
        private async Task SendAsync(HttpMethod method, string endpoint, object data = null)
        {
            // Create a new HttpRequestMessage using the specified method and endpoint.
            var request = new HttpRequestMessage(method, endpoint);
            // If 'data' is not null, serialize it to JSON and add it to the request's content.
            if (data != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            }

            // Send the request asynchronously and await the response.
            var response = await _httpClient.SendAsync(request);
            // Throws an exception if the HTTP response status indicates failure.
            response.EnsureSuccessStatusCode();
        }

        // Private helper method with a return type for operations that return data.
        private async Task<T> SendAsync<T>(HttpMethod method, string endpoint, object data = null)
        {
            // Similar setup to the SendAsync method without a return type.
            var request = new HttpRequestMessage(method, endpoint);
            if (data != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            }

            // Send the request and await the response.
            var response = await _httpClient.SendAsync(request);

            // Check if the response indicates success. If not, handle accordingly.
            if (!response.IsSuccessStatusCode)
            {
                // Read the response body as a string for logging or further processing.
                string errorResponse = await response.Content.ReadAsStringAsync();
                // Logging the error for diagnostics. Consider a more sophisticated logging approach for production.
                Debug.WriteLine($"API call failed: {errorResponse}");
                // Throwing an exception to indicate failure. Customize the exception handling as needed.
                throw new HttpRequestException($"API call failed with status code: {response.StatusCode}, Response: {errorResponse}");
            }

            // Deserialize the JSON response to the specified type T and return.
            string jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(jsonResponse);
        }
    }
}

