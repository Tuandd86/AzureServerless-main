using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos;
using InventoryAppFunctions.Models;
using System.Collections.Generic;

namespace InventoryAppFunctions.Function
{
    public static class GetInventoryItems
    {
        private static readonly string _endpointUri = Environment.GetEnvironmentVariable("COSMOS_DB_ENDPOINT");
        private static readonly string _primaryKey = Environment.GetEnvironmentVariable("COSMOS_DB_PRIMARY_KEY");
        private static readonly CosmosClient _client = new CosmosClient(_endpointUri, _primaryKey);
        private static readonly Container _container = _client.GetContainer("InventoryDB", "Items");

        [FunctionName("GetInventoryItems")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            var sqlQueryText = "SELECT * FROM c";
            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<Item> queryResultSetIterator = _container.GetItemQueryIterator<Item>(queryDefinition);

            var items = new List<Item>();
            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<Item> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (Item item in currentResultSet)
                {
                    items.Add(item);
                }
            }

            return new OkObjectResult(items);
        }
    }
}
