using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos;
using Microsoft.AspNetCore.Mvc;
using ProductReviewApp.Models;
using System.Threading.Tasks;

namespace ProductReviewApp
{
    public class DeleteReview
    {
        private readonly CosmosClient _cosmosClient;

        public DeleteReview(CosmosClient cosmosClient)
        {
            _cosmosClient = cosmosClient;
        }

        [Function("DeleteReview")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "reviews/{id}")] HttpRequestData req,
            string id,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("DeleteReview");
            logger.LogInformation("C# HTTP trigger function processed a request to delete a review.");

            var container = _cosmosClient.GetContainer("YourDatabase", "Reviews");
            var response = await container.DeleteItemAsync<Review>(id, new PartitionKey(id));

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new NotFoundResult();
            }

            return new OkResult();
        }
    }
}