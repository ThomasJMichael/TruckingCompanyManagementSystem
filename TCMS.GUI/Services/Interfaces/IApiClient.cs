using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCMS.GUI.Services.Interfaces
{
    // IApiClient interface declaration.
    // This interface defines the contract for an API client that will be used to interact with a web service.
    // By defining this interface, you ensure that the implementation can vary
    // without changing how the rest of your application interacts with web services.
    public interface IApiClient
    {
        // GetAsync method for performing HTTP GET requests.
        // Generic <T> allows for any return type, making this method versatile for various endpoints.
        // 'endpoint' parameter specifies the relative URI to which the GET request is sent.
        Task<T> GetAsync<T>(string endpoint);

        // PostAsync method for performing HTTP POST requests.
        // This method sends data to the specified 'endpoint' and expects a response of type <T>.
        // 'data' parameter is the payload that will be serialized to JSON and sent in the request body.
        Task<T> PostAsync<T>(string endpoint, object data);

        // PutAsync method for performing HTTP PUT requests.
        // Similar to PostAsync, but semantically used for updating resources at the 'endpoint'.
        // 'data' is the new content to be sent to the specified resource.
        Task<T> PutAsync<T>(string endpoint, object data);

        // DeleteAsync method for performing HTTP DELETE requests.
        // 'endpoint' specifies the resource to be deleted. No return type is expected.
        Task DeleteAsync(string endpoint);

        // Placeholder for additional HTTP verb methods, such as PATCH, OPTIONS, etc.
        // These can be added as needed to support further actions as defined by your web service's API.
    }
}

