using System.Net;
using System.Text;
using Azure.Storage.Files.Shares;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Company.Function.Handlers;

public class HttpHandler
{
    private readonly ILogger _logger;
    private readonly string? SasConnectionString = Environment.GetEnvironmentVariable("SasConnectionString");
    private readonly string? FileShareName = Environment.GetEnvironmentVariable("FileShareName");
    private readonly string? DirectoryName = Environment.GetEnvironmentVariable("DirectoryName");

    public HttpHandler(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<HttpHandler>();
    }

    [Function("SaveFile")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "save")] HttpRequestData req)
    {
        var fileName = $"{DateTime.Now.ToString("yyyyMMddHHmmss")}-test.txt";
        _logger.LogInformation($"Saving file {fileName}");

        await SaveFileToFileshare(fileName);

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        response.WriteString($"Saved file {fileName}");
        return response;
    }

    private async Task SaveFileToFileshare(string fileName)
    {
        ShareClient shareClient = new ShareClient(SasConnectionString, FileShareName);
        ShareDirectoryClient directoryClient = shareClient.GetDirectoryClient(DirectoryName);
        await directoryClient.CreateIfNotExistsAsync();

        ShareFileClient fileClient = directoryClient.GetFileClient(fileName);
        var file = new MemoryStream(Encoding.UTF8.GetBytes("This is a test file."));
        await fileClient.CreateAsync(file.Length);
        await fileClient.UploadAsync(file);
    }
}
