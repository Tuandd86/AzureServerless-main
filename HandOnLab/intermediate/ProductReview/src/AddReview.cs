using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProductReviewApp.Models; // Replace with the actual namespace of your Review model
using Microsoft.Azure.Cosmos;

namespace ProductReviewApp
{
    public class AddReview
    {
        private readonly CosmosClient _cosmosClient;

        public AddReview(CosmosClient cosmosClient)
        {
            _cosmosClient = cosmosClient;
        }

        [Function("AddReview")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("AddReview");
            logger.LogInformation("C# HTTP trigger function processed a request to add a review.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var review = JsonConvert.DeserializeObject<Review>(requestBody);
            review.Id = Guid.NewGuid().ToString();

            var container = _cosmosClient.GetContainer("YourDatabase", "Reviews");
            await container.CreateItemAsync(review, new PartitionKey(review.Id));

            var response = req.CreateResponse(HttpStatusCode.Created);
            await response.WriteStringAsync("Review added successfully.");

            return response;
        }
    }
}