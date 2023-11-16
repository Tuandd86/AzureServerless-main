using System.Collections.Generic;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Extensions.Sql;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProductReviewApp.Models;

namespace ProductReviewApp
{
    public static class GetProductBinding
    {
        [Function("GetProductBinding")]
        public static IActionResult Run(
            [Microsoft.Azure.Functions.Worker.HttpTrigger(Microsoft.Azure.Functions.Worker.AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequestData req,
            [SqlInput(commandText: "SELECT * FROM Products", connectionStringSetting : "SqlConnectionString")] IEnumerable<Product> products,
            FunctionContext context)
        {
            var log = context.GetLogger("GetProductBinding");
            log.LogInformation("C# HTTP trigger function processed a request.");

            return new OkObjectResult(products);
        }
    }
}
