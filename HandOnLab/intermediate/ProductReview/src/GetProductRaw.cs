using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProductReviewApp.Models;

namespace ProductReviewApp
{
    public static class GetProductRaw
    {
        [Function("GetAllProducts")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "products")] HttpRequestData req,
            FunctionContext context)
        {
            string connectionString = Environment.GetEnvironmentVariable("SqlConnectionString");
            if (connectionString == null)
            {
                throw new ArgumentNullException("SqlConnectionString", "Connection string is null");
            }
            List<Product> products = new List<Product>();
            
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var query = "SELECT Id, Name, Description FROM Products";
                
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        products.Add(new Product
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Description = reader.GetString(2)
                        });
                    }
                }
            }

            return new OkObjectResult(products);
        }
    }
}
