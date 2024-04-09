# GraphQl in Asp.NET Core Web API

GraphQL is a query language. It executes queries by using type systems which we define for our data. 
GraphQL isn’t tied to any specific language or a database, just the opposite, it is adaptable to our code and our data as well.

# Create a Asp.NET Core Web API Project

```bash
dotnet new webapi --use-controllers -n GraphQLWebApi
```

Create the "Contract" folder for interfaces. The Contracts folder contains interfaces required for our repository logic:
```csharp
namespace GraphQLDotNetCore.Contracts
{
    public interface IOwnerRepository
    {
    }
}
namespace GraphQLDotNetCore.Contracts
{
    public interface IAccountRepository
    {
    }
}
```

In the Entities folder, we keep model classes with a context class and the seed configuration classes:
```csharp
public class Owner
{
    [Key]
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }
    public string Address { get; set; }
    public ICollection<Account> Accounts { get; set; }
}
public class Account
{
    [Key]
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Type is required")]
    public TypeOfAccount Type { get; set; }
    public string Description { get; set; }
    [ForeignKey("OwnerId")]
    public Guid OwnerId { get; set; }
    public Owner Owner { get; set; }
}
```
The TypeOfAccount enum is used to define the type of the account. The Owner class has a collection of accounts, and the Account class has a reference to the Owner class.
```csharp
public enum TypeOfAccount
{
    Cash,
    Savings,
    Expense,
    Income
}
```
The ApplicationContext class is used to define the database context and to configure the relationship between the Owner and Account classes:
```csharp
public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions options)
        :base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var ids = new Guid[] { Guid.NewGuid(), Guid.NewGuid() };
        modelBuilder.ApplyConfiguration(new OwnerContextConfiguration(ids));
        modelBuilder.ApplyConfiguration(new AccountContextConfiguration(ids));
    }
    public DbSet<Owner> Owners { get; set; }
    public DbSet<Account> Accounts { get; set; }
}
```
And in a Repository folder, we have classes related to the data fetching logic:
```csharp
public class OwnerRepository : IOwnerRepository
{
    private readonly ApplicationContext _context;
    public OwnerRepository(ApplicationContext context)
    {
        _context = context;
    }
}
public class AccountRepository : IAccountRepository
{
    private readonly ApplicationContext _context;
    public AccountRepository(ApplicationContext context)
    {
        _context = context;
    }
}
```

This repository logic has the basic setup without any additional layers. 
The context class and repository classes are registered inside the Program.cs class:
```csharp

    builder.Services.AddDbContext<ApplicationContext>(opt =>
        opt.UseSqlServer(Configuration.GetConnectionString("ApplicationContext")));
    builder.Services.AddScoped<IOwnerRepository, OwnerRepository>();
    builder.Services.AddScoped<IAccountRepository, AccountRepository>();

    builder.Services.AddControllers();
}
```
So, at this point, all you have to do is to modify the connection string (if you have to) in the appsettings.json file and to navigate to the Package Manager Console, and run the 
'''bash
update-database
'''
command. Once you do that, we are ready to move on.

As we can see, we are using the Code-First approach in the begining of this project.

## Add GraphQL to a Asp.Net Core Web API Project

https://www.nuget.org/packages/GraphQL/

```bash
 dotnet add package GraphQL --version 7.8.0


 dotnet add package GraphQL.Server.Transports.AspNetCore


 dotnet add package GraphQL.Server.Transports.AspNetCore.SystemTextJson


 dotnet add package GraphQL.Server.Ui.Playground



 ```

 dotnet add package Microsoft.EntityFrameworkCore --version 8.0.3

 dotnet add package Microsoft.EntityFrameworkCore.Relational --version 8.0.3

 dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.3

 Add the connection string.

 ```json

 {
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "ApplicationContext": "server=.; database=GraphQLWDatabase; Integrated Security=true; TrustServerCertificate=True"
  }
}

 ```

 ## Dependency injection for SQL Server

 ```csharp  
 // Add services to the container.

builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationContext"));
});
```

Run in Package Management Console
```bash
 Add-Migration Initial
 ```

 This command will create two files :
 - 20240408131248_Initial.cs
 and
 - ApplicationContextModelSnapshot.cs

 Now that we have migrations, we can run the update-database command to create the database and the tables:

 ```bash    
 update-database
 ```
 and therefore in our GraphQLWDatabase database we have the tables created.

 ## Dependency injection for GraphQl
 In the Program.cs file, we need to register the GraphQL services:

 ```csharp
  services.AddScoped<AppSchema>();
    services.AddGraphQL()
        .AddSystemTextJson()
        .AddGraphTypes(typeof(AppSchema), ServiceLifetime.Scoped);
  ```
and at the end after "app.UseAuthorization();":

```csharp
    app.UseAuthorization();

    app.UseGraphQL<AppSchema>();
    app.UseGraphQLPlayground(options: new GraphQLPlaygroundOptions());
```

Therefore, we register the DependencyResolver (for our queries) and the schema class as well. Furthermore, we register GraphQL with the AddGraphQL method and register all 
the GraphQL types (Query and Type classes) with the AddGraphTypes method.
Without this method, we would have to register all the types and queries manually in our API.

## Comparison GraphQL vs REST

GraphQL requires fewer roundtrips to the server and back to fetch all the required data for our view or template page. With REST, we have to visit several endpoints (api/subjects, api/professors, api/students …) to get all the data we need for our page, but that’s not the case with GraphQL. When we use GraphQL, we create only one query which calls several resolvers (functions) on the server-side and returns all the data from different resources in a single request.
With REST, as our application grows, the number of endpoints grows as well, and that requires more and more time to maintain. But, with GraphQL we have only one endpoint api/graphql and that is all.
By using GraphQL, we never face a problem of getting too much or too little data in our response. That’s because we are defining our queries with the fields which state what we need in return. That way, we are always getting what we have requested. So, if we send a query like this one:
query OwnersQuery {
  owners {
    name
    account {
      type
    }
  } 
}
We are 100% sure that we will get this response back:


{
  "data": {
    "owners": [
     {
      "name": "John Doe",
      "accounts": [
        {
          "type": "Cash"
        },
        {
          "type": "Savings"
        }
      ]
     }
    ]
  }
}
With REST this is not the case. Sometimes we get more than we need and sometimes less, it depends on how actions on a certain endpoint are implemented.

## Add GraphQL Specific Objects - Type, Query, Schema

A graphQL searchs base on a schema: AppSchema; a schema has queries and mutations ; a query has fields and a field has a type.

```csharp   
public class AppSchema : Schema
{
    public AppSchema(IServiceProvider provider)
        :base(provider)
    {

    }
}
```
This AppSchema class must inherit from the Schema class which resides in the __GraphQL.Types__ namespace.
Inside the constructor, we inject the IServiceProvider which is going to help us provide our Query, Mutation, or Subscription objects.
What’s important to know is that each of the schema properties (Query, Mutation, or Subscription) implements IObjectGraphType which 
means that the objects we are going to resolve must implement the same type as well. This also means that our GraphQL API can’t return our models directly
as a result but GraphQL types that implement IObjectGraphType instead.

### OwnerType the equivalent for the Owner class --  from model

```csharp
public class OwnerType : ObjectGraphType<Owner>
{
    public OwnerType()
    {
        Field(x => x.Id, type: typeof(IdGraphType)).Description("Id property from the owner object.");
        Field(x => x.Name).Description("Name property from the owner object.");
        Field(x => x.Address).Description("Address property from the owner object.");
    }
}
```
The OwnerType class inherits from the ObjectGraphType class which is a generic class that takes the Owner class as a type parameter.
This class inherits from a generic ObjectGraphType<Owner> class which at some point (in the hierarchy) implements IObjectGraphType interface. 
With the Field method, we specify the fields which represent our properties from the Owner model class.

### AppQuery - where the query resides
'''csharp
public class AppQuery : ObjectGraphType
{
    public AppQuery(IOwnerRepository repository)
    {
        Field<ListGraphType<OwnerType>>(
           "owners",
           resolve: context => repository.GetAll()
       );
    }
}
'''

This class inherits from the ObjectGraphType as well, just the non-generic one. 
Moreover, we inject our repository object inside a constructor and create a field to return the result for the specific query.
In this class, we use the generic version of the Field method which accepts some „strange“ type as a generic parameter.
Well, this is the GraphQL.NET representation for the normal .NET types. 
So, ListGraphType is the representation of the List type, and of course, we have IntGraphType or StringGraphType, etc.


## Chech the GraphQL in browser

To test our GraphQL API, we are going to use the GraphQL.UI.Playground tool. 
Therefore we start the server application and then navigate to the https://localhost:5001/ui/playground address.

A simple query looks like this:
```graphql
{
  owners
  {
    name
    id
    accounts
    {
      type
    }
  }
  
}
```








## Reference
[x] https://app.pluralsight.com/library/courses/building-graphql-apis-aspdotnet-core/table-of-contents
[x] https://github.com/graphql-dotnet/graphql-dotnet?tab=readme-ov-file
[x] https://github.com/graphql-dotnet/examples/tree/master
[x] https://code-maze.com/consume-graphql-api-with-asp-net-core/