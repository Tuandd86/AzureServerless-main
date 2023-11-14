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
    public static class CreateInventoryItem
    {
        private static readonly string _endpointUri = Environment.GetEnvironmentVariable("COSMOS_DB_ENDPOINT");
        private static readonly string _primaryKey = Environment.GetEnvironmentVariable("COSMOS_DB_PRIMARY_KEY");
        private static readonly CosmosClient _client = new CosmosClient(_endpointUri, _primaryKey);
        private static readonly Container _container = _client.GetContainer("InventoryDB", "Items");
        [FunctionName("CreateInventoryItem")]
        public static async Task<IActionResult> CreateItem(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var item = JsonConvert.DeserializeObject<Item>(requestBody);
            await _container.CreateItemAsync(item, new PartitionKey(item.Id));
            return new OkObjectResult(item);
        }
    }
}
