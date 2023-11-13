using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using SendGrid.Helpers.Mail;
using Microsoft.Azure.Cosmos;

public static class ProcessFeedback
{
    [FunctionName("ProcessFeedback")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function,"post", Route = null)] HttpRequest req,
        ILogger log)
    {
        log.LogInformation("C# HTTP trigger function processed a request.");

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var feedbackRequest = JsonConvert.DeserializeObject<FeedbackRequest>(requestBody);

        if (feedbackRequest == null)
        {
            return new BadRequestObjectResult("Please pass a valid feedback request in the request body.");
        }

        var userEmail = feedbackRequest.Email;
        var feedback = feedbackRequest.Content;

        // Store feedback in Cosmos DB
        string connectionString = Environment.GetEnvironmentVariable("CosmosDBConnection");
        CosmosClient client = new CosmosClient(connectionString);
        Database database = await client.CreateDatabaseIfNotExistsAsync("FeedbackDB");
        Container container = await database.CreateContainerIfNotExistsAsync("Feebacks", "/id");
        
        dynamic feedbackItem = new
        {
            id = Guid.NewGuid().ToString(),
            email = userEmail,
            feedback = feedback,
            timestamp = DateTime.UtcNow
        };

        await container.CreateItemAsync(feedbackItem, new PartitionKey(feedbackItem.id));

        // Send a thank you email using sendGrid
        // var sendGridKey = Environment.GetEnvironmentVariable("SendGridApiKey");
        // var message = new SendGridMessage()
        // {
        //     From = new EmailAddress("no-reply@yourdomain.com", "Your Website"),
        //     Subject = "Thank you for your feedback!",
        //     PlainTextContent = $"Hello, thank you for your feedback! Here is what we got from you: {feedback}",
        //     HtmlContent = $"<strong>Hello, thank you for your feedback!</strong> Here is what we got from you: {feedback}"
        // };
        
        // message.AddTo(new EmailAddress(userEmail));
        // var sendGridclient = new SendGrid.SendGridClient(sendGridKey);
        // await sendGridclient.SendEmailAsync(message);

        return new OkObjectResult("Thank you for your feedback!");
    }

    public class FeedbackRequest
    {
        public string Email { get; set; }
        public string Content { get; set; }
    }
}
