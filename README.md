# MOT.NET
## Overview
**MOT.NET** is a .NET Core library which provides access to the [DVSA's MOT history API](https://dvsa.github.io/mot-history-api-documentation/).

As well as MOT tests, this API provides access to other vehicle data. These include **registration date**, **cylinder capacity**, **fuel type** and **make and model**.

## Apply for API access
This API requires an API key. You may obtain an API key for your organisation by completing [this application](https://www.smartsurvey.co.uk/s/MOT_History_TradeAPI_Access_and_Support?).

## Installation
The library is available as "MOT.NET" on [NuGet](https://www.nuget.org/packages/MOT.NET/).

Install the package by running:

`dotnet add package MOT.NET`

## Setup
`MotTestClient` requires an API key, specify it in the constructor as follows:
```csharp
// Example API key, replace with yours. Keep it secret!
var apikey = "foobar";

// Initialise the MotTestClient.
IMotTestClient client = new MotTestClient(apikey);
```

If you prefer, you can provide the API key as a [`SecureString`](https://docs.microsoft.com/en-us/dotnet/api/system.security.securestring).

## Queries
Once initialised, the library may query the API in three different ways:
### By vehicle:
Vehicles are queried by registration. Use the `Registration` method to specify a registration, and `FetchAsync` to fetch records:
```csharp
// IAsyncEnumerable of Vehicles, allowing asynchronous streaming of results.
IAsyncEnumerable<Vehicle> vehicles = client
    .Registration("F1") // Set the registration to query.
    .FetchAsync(); // Fetch the vehicle from the API service.

// Asynchronously iterate reulsts.
await foreach(Vehicle vehicle in vehicles) {
    // Output vehicle make.
    Console.WriteLine(vehicle.Make);
}
```

### By page:
Records are grouped in pages of 400-500 each. Use the `Page` method to specify a page, and `FetchAsync` to fetch records:
```csharp
// Used after a previous query has been run, clears parameters.
client.Clear();

IAsyncEnumerable<Vehicle> vehicles = client
    .Page(0) // Set the page to query.
    .FetchAsync();

await foreach(Vehicle vehicle in vehicles) {
    // Output the vehicle models.
    Console.WriteLine(vehicle.Model);
}
```

This method is useful for downloading all records. Continue to download these pages from zero until exhaustion after ~50,000 pages (server will return status 404).

**Note**: The API is rate limited to 10 requests per second, make sure not to exceed this.

### By date and page:
Records are queryable by date, with 1440 pages per day (1 page per minute).

Use the `Date` method to set the date parameter, the `Page` method to set the page parameter, and the `FetchAsync` method to fetch records:
```csharp
client.Clear();

IAsyncEnumerable<Vehicle> vehicles = client
    .Date(DateTime.Today) // Set the page to query (to today).
    .Page(720) // Set the page to query (720 = noon).
    .FetchAsync();

await foreach(Vehicle vehicle in vehicles) {
    // Output the vehicle engine size.
    Console.WriteLine(vehicle.EngineSize;
}
```

## Invalid Queries
The `Page` parameter may be used with the `Date` parameter. However, the `Registration` parameter may not be used with any other parameter.

Conversely, the `Date` parameter may not be used on its own and must be used with the `Page` parameter.

The library will throw an `InvalidParametersException` when parameters are combined in any way  which the API will not accept.

## Exceptions
Some API error responses are represented by exceptions to improve usability. These are thrown by `FetchAsync`.

These are `NoRecordsFoundException`; thrown when no records are found for the given parameters and `InvalidApiKeyException`; thrown when the specified API key is not accepted by the API service.

`FetchAsync` throws [`HttpResponseException`](https://docs.microsoft.com/en-us/dotnet/api/system.web.http.httpresponseexception) for all other response codes (except 200).

# Thanks!
Your usage and support is greatly appreciated.