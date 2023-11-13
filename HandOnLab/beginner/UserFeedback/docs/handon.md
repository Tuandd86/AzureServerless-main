# Serverless User Feedback Processing System on Azure

## Use Case Description
Create a serverless application on Microsoft Azure to handle user feedback submissions via a web form. The system will store feedback in Azure Cosmos DB and send an automated 'Thank You' email to the user using SendGrid.

## Expected Outcomes
- Feedback stored in Azure Cosmos DB.
- Automated email sent to the user's email address.

## Prerequisites
- Azure account with an active subscription.
- Visual Studio 2019 or later with Azure development workload.
- SendGrid account.

## Step-by-Step Guide

### Step 1: Create Azure Cosmos DB Instance
#### 1.1 Create Cosmos DB Account
- Log in to the [Azure Portal](https://portal.azure.com/).
- Click “Create a resource” > “Databases” > “Azure Cosmos DB.”
- Fill in the details and create the account.

#### 1.2 Create Database and Container
- Go to the created Cosmos DB account.
- Open “Data Explorer” > “New Container.”
- Enter details for the database and container.

#### 1.3 Get Connection String
- Go to “Keys” in your Cosmos DB account.
- Copy the “PRIMARY CONNECTION STRING.”

### Step 2: Set Up SendGrid for Email Notifications
#### 2.1 Create SendGrid API Key
- Log into SendGrid.
- Navigate to “Settings” > “API Keys” > “Create API Key.”
- Copy and save your API key.

#### 2.2 Verify Sender Identity
- Under “Settings,” go to “Sender Authentication.”
- Complete the domain verification process.

### Step 3: Create Azure Function App
#### 3.1 Create Function App
- In Azure Portal, create a new Function App.
- Fill in the necessary details and create the app.

### Step 4: Develop Azure Function in Visual Studio
#### 4.1 Create a New Azure Functions Project
- Open Visual Studio.
- Create a new Azure Functions project.
- Select “HTTP trigger” and set the Authorization level to “Anonymous.”

#### 4.2 Install Required NuGet Packages
- Install `Microsoft.Azure.Cosmos` and `SendGrid` packages via NuGet Package Manager.

#### 4.3 Replace Function Code
```csharp
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SendGrid.Helpers.Mail;
using Microsoft.Azure.Cosmos;
using System;

namespace UserFeedbackFunction
{
    public static class ProcessFeedbackFunction
    {
        [FunctionName("ProcessFeedback")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Processing feedback.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            var email = data?.email;
            var feedback = data?.feedback;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(feedback))
            {
                return new BadRequestObjectResult("Please provide both email and feedback.");
            }

            // Save to Cosmos DB
            string connectionString = Environment.GetEnvironmentVariable("CosmosDBConnection");
            CosmosClient client = new CosmosClient(connectionString);
            Container container = client.GetContainer("FeedbackDb", "Items");

            dynamic item = new { id = Guid.NewGuid().ToString(), email, feedback, timestamp = DateTime.UtcNow };
            await container.CreateItemAsync(item, new PartitionKey(item.id));

            // Send email
            var apiKey = Environment.GetEnvironmentVariable("SendGridApiKey");
            var client = new SendGridClient(apiKey);
            var msg = MailHelper.CreateSingleEmail(
                new EmailAddress("youremail@example.com"),
                new EmailAddress(email),
                "Thank you for your feedback!",
                $"Hello, thank you for your feedback! Here is what we got from you: {feedback}",
                $"Hello, thank you for your feedback! Here is what we got from you: {feedback}");
            await client.SendEmailAsync(msg);

            return new OkObjectResult("Feedback processed.");
        }
    }
}
```
#### 4.4 Update Local Settings
```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "<storage_account_connection_string>",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "CosmosDBConnection": "<your_cosmosdb_connection_string>",
    "SendGridApiKey": "<your_sendgrid_api_key>"
  }
}
```

### Step 5: Test and Deploy
#### 5.1 Test Locally
Run the project in Visual Studio.
Test the function with a tool like Postman.
#### 5.2 Deploy to Azure
Right-click on the project > “Publish.”
Publish to your Azure Function App.
### Step 6: Update Azure Function App Settings
#### 6.1 Update Application Settings in Azure
In Azure Portal, update your Function App settings with Cosmos DB and SendGrid details.
### Step 7: Verify Deployment
#### 7.1 Test the Deployed Function
Send a test request to your Azure Function's URL.
Verify the feedback in Cosmos DB and check for the email.