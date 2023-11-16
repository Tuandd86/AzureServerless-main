using System;
using System.IO;
using System.Net;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs;
using ProductReviewApp.Models;

namespace ProductReviewApp
{
    public class AddProduct
    {
        [Function("AddProduct")]
        public static async Task<IActionResult> Run(
            [Microsoft.Azure.Functions.Worker.HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequestData req,
            FunctionContext context)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var product = JsonConvert.DeserializeObject<Product>(requestBody);

            string connectionString = Environment.GetEnvironmentVariable("SqlConnectionString");
            
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var query = "INSERT INTO Products (Name, Description) VALUES (@Name, @Description)";
                
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", product?.Name);
                    cmd.Parameters.AddWithValue("@Description", product?.Description);
                    await cmd.ExecuteNonQueryAsync();
                }
            }

            return new OkObjectResult(product);
        }
    }
}
