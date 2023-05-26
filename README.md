# Writing to Azure Storage File Share

This is an example of writing files to an Azure Storage File Share using a SAS connection string with .NET 7 Isolated C# Functions.

## local.settings.json

To execute this Function App locally add in a *local.settings.json* file in the root directory with the following values:

```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    "SasConnectionString": "[your storage account SAS connection string]",
    "FileShareName": "[your file share's name]",
    "DirectoryName": "[the directory to save files into]"
  }
}
```

## Generating a SAS Connection String

1. Go to an Azure Storage Account in the [Azure Portal](https://portal.azure.com)
1. In the left navigation, select ***Shared access signature***
1. Select the following to generate a SAS Connection String
    * Allowed services: File
    * Allowed resource types: Service, Container, Object
    * Allowed permissions: Write, Create
    * Start and expiry date/time: Time range for valid SAS
    * Allowed protocols: HTTPS only
1. After generating the SAS, copy the ***Connection String*** into the ***SasConnectionString*** local.settings.json value.
