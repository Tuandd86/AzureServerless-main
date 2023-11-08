# Azure Function Image Resizing Guide

This guide outlines the process of creating an Azure Function that automatically resizes images when they are uploaded to Azure Blob Storage.

## Step 1: Set up Azure Blob Storage

### Log in to the Azure Portal

1. Open your web browser and visit [Azure Portal](https://portal.azure.com).
2. Sign in with your Azure account credentials.

### Create a Storage Account

1. Click on the “**Create a resource**” button (plus icon in the top left corner).
2. Select “**Storage**” then “**Storage account**”.
3. Choose your subscription and Resource Group.
4. Enter a unique name for your storage account and select the closest region.
5. Click "**Review + create**", review your settings, then click "**Create**".

### Create Blob Containers

1. In your storage account, under “**Blob service**”, click on “**Containers**”.
2. Click "**+ Container**" and name the first container `images`. Set the access level to "Private".
3. Click "**Create**".
4. Repeat the steps to create a second container named `thumbnails`.

## Step 2: Create an Azure Function App

Ensure you have **Azure Functions Core Tools**, **Visual Studio Code**, and the **Azure Functions extension** for Visual Studio Code installed.

### Open VS Code

1. Launch Visual Studio Code.
2. Install the Azure Functions extension if not already installed.

### Create a new Azure Function Project

1. Press `Ctrl+Shift+P` to open the command palette.
2. Type “**Azure Functions: Create new project…**” and press Enter.
3. Choose a local directory, select a language (C#), and then the "**Blob trigger**" template.
4. Name your function (e.g., “**ImageResizerFunction**”).
5. For the blob path, enter `images/{name}`.
6. Select “**Add to workspace**”.

### Install Necessary NuGet Packages

In the terminal within VS Code:

```sh
dotnet add package SkiaSharp
dotnet add package Microsoft.Azure.WebJobs.Extensions.Storage --version 4.0.4

## Step 3: Implement Image Resizing Logic

Modify the function code within your Azure Function project to resize images using SkiaSharp and save the result to the `thumbnails` container.

### Modify the Function Code

Here's a C# example using SkiaSharp to resize an image when a new blob is added to the `images` container:

```csharp
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SkiaSharp;
using System;
using System.IO;
using Azure.Storage.Blobs;

public static class ImageResizerFunction
{
    [FunctionName("ImageResizer")]
    public static void Run(
        [BlobTrigger("images/{name}", Connection = "AzureWebJobsStorage")] Stream myBlob,
        string name,
        ILogger log)
    {
        log.LogInformation($"C# Blob trigger function processed: {name}");

        // Resize the image using SkiaSharp.
        var resizedImage = ResizeImage(myBlob);
        if (resizedImage != null)
        {
            // Create a reference to the 'thumbnails' container.
            var connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            var serviceClient = new BlobServiceClient(connectionString);
            var containerClient = serviceClient.GetBlobContainerClient("thumbnails");
            var blobClient = containerClient.GetBlobClient(name);

            // Upload the resized image.
            blobClient.Upload(resizedImage, true);
            resizedImage.Dispose();
        }
    }

    private static Stream ResizeImage(Stream originalImageStream)
    {
        // Define the size of the thumbnail.
        const int size = 100;
        const int quality = 75;

        // Decode the image with SkiaSharp.
        using var inputStream = new SKManagedStream(originalImageStream);
        using var original = SKBitmap.Decode(inputStream);

        // Calculate the new height.
        var height = (int)((original.Height / (float)original.Width) * size);
        using var resized = original.Resize(new SKImageInfo(size, height), SKFilterQuality.High);

        // Encode the new image.
        var outputStream = new MemoryStream();
        using (var image = SKImage.FromBitmap(resized))
        using (var data = image.Encode(SKEncodedImageFormat.Jpeg, quality))
        {
            data.SaveTo(outputStream);
        }

        outputStream.Seek(0, SeekOrigin.Begin);
        return outputStream;
    }
}

## Step 4: Test the Function

Once your function is ready, it's time to test it to ensure that it works as expected.

### Run the Azure Function Locally

You can run the Azure Function locally to test the trigger and the resizing logic without deploying it to Azure.

1. **Start the Function App:**
   - Open the terminal in VS Code.
   - Make sure you are in the function app's directory.
   - Run the function app by pressing `F5` or using the command:
     ```shell
     func start
     ```
   - This will compile your function and start it on your local development machine.
   - You should see output in the terminal that indicates the function is running.

### Upload an Image to the `images` Container

To test the function, you will need to upload an image to your Azure Blob Storage `images` container.

1. **Use Azure Storage Explorer:**
   - Open Azure Storage Explorer.
   - Navigate to your blob storage account.
   - Find the `images` container and upload an image.

   Or:

2. **Use the Azure Portal:**
   - Navigate to your storage account in the Azure Portal.
   - Go to the `Blob service` section and select the `images` container.
   - Upload an image using the "Upload" button.

### Check the `thumbnails` Container

After the image is uploaded, the function should trigger automatically and process the image.

1. **Verify the Function Execution:**
   - In VS Code terminal, you should see logs indicating that the function was triggered.
   - Check for any errors or logs that indicate the function executed successfully.

2. **Check the `thumbnails` Container:**
   - After the function has run, go to the `thumbnails` container in Azure Storage Explorer or the Azure Portal.
   - You should see the resized image has been added to this container.
   - Download and view the thumbnail to ensure it's been resized correctly.

If everything went as planned, your function is working correctly. If not, review the logs and the code to troubleshoot any issues.
