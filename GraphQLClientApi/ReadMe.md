# Use GraphQl inside an ASP.NET Core application as a Client

For this example, we will use the __GraphQL Client__ Library for .NET

## Create an ASp.NET Core Web Application

But First, let's check the available .NET SDKs on your machine using the following command:
```
dotnet --list-sdks
```

Then install the latest .NET 8 SDK if not exist:
```
winget install --id Microsoft.DotNet.SDK.8
```

Create a new ASP.NET Core Web Application using the following command:
```
dotnet new web -n GraphQLClientApi --use-controllers
```

Modify the launchsettings.json file, by setting the launchBrowser property to false and applicationUrl property to https://localhost:5003;http://localhost:5004

And set the GraphQL server api to '"GraphQLURI": "https://localhost:5001/graphql",':
```json	
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "GraphQLURI": "https://localhost:5001/graphql",
  "AllowedHosts": "*"
}
```

## GraphQL.Client 
The GraphQL Client for .NET Standard over HTTP can be fount here on nuget site: https://www.nuget.org/packages/GraphQL.Client/

Add the package to your project using the following command:
```
dotnet add package GraphQL.Client --version 6.0.3
```
then we need one more library for the GraphQL serialization:
```
dotnet add package GraphQL.Client.Serializer.Newtonsoft
```

## Add client in Configuration
```csharp
builder.Services.AddScoped<IGraphQLClient>(s => new GraphQLHttpClient(builder.Configuration["GraphQLURI"], new NewtonsoftJsonSerializer()));
```

The next step is creating the OwnerConsumer class, which will store all the queries and mutations:

```csharp
public class OwnerConsumer
{
    private readonly IGraphQLClient _client;

    public OwnerConsumer(IGraphQLClient client)
    {
        _client = client;
    }
}
```

and register it:
```csharp
builder.Services.AddScoped<OwnerConsumer>();
```

## Add models

Let’s create the models:
```csharp
public enum TypeOfAccount
{
    Cash,
    Savings,
    Expense,
    Income
}
public class Account
{
    public Guid Id { get; set; }
    public TypeOfAccount Type { get; set; }
    public string Description { get; set; }
}
public class Owner
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public ICollection<Account> Accounts { get; set; }
}
```

and the Input type class for the mutation actions. This class is required as well, so let’s create it:
```csharp
public class OwnerInput
{
    public string Name { get; set; }
    public string Address { get; set; }
}
```

## Queries and Mutations to Consume GraphQL API

We keep the queries and the mutation from the above project, where we have created the GraphQL API. 

In the __OwnerConsumer class--, we will add the all CRUD methods:

```csharp
public async Task<List<Owner>> GetAllOwners()

public async Task<Owner> GetOwnerById(Guid id)

public async Task<Owner> CreateOwner(OwnerInput ownerInput)

public async Task<Owner> UpdateOwner(Guid id, OwnerInput ownerInput)

public async Task<Owner> DeleteOwner(Guid id)
```


We will use the same queries and mutations to consume the GraphQL API,
and to test them we use the http file __GraphQLClientApi.http__:

```http
@GraphQLClientApi_HostAddress = https://localhost:5004

GET {{GraphQLClientApi_HostAddress}}/owner/
Accept: application/json

###

GET {{GraphQLClientApi_HostAddress}}/owner/791716be-7eff-4370-814e-3884d27ae682
Accept: application/json
###

POST {{GraphQLClientApi_HostAddress}}/owner
Content-Type: application/json

  {
    "name": "Clark Doe",
    "address" : "New York, street Central 34"
  }

###

PUT {{GraphQLClientApi_HostAddress}}/owner/791716be-7eff-4370-814e-3884d27ae682
Content-Type: application/json

  {
    "name": "Clark Doe Updated",
    "address" : "New York, street Central 34- updated"
  }
 
###
  
DELETE {{GraphQLClientApi_HostAddress}}/owner/4891a704-656a-4730-9b2b-c34d363d3e2a
Accept: application/json

###
```

// describe more about the queries and mutations
    
## Run the application
    Run the application and test the queries and mutations using the GraphQLClientApi.http file. Make sure that the other api works on https://localhost:5001



## Reference
[x] https://app.pluralsight.com/library/courses/building-graphql-apis-aspdotnet-core/table-of-contents
[x] https://github.com/graphql-dotnet/graphql-dotnet?tab=readme-ov-file
[x] https://github.com/graphql-dotnet/examples/tree/master
[x] https://code-maze.com/consume-graphql-api-with-asp-net-core/
