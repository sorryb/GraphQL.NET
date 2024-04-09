using GraphQL.DataLoader;
using GraphQL.Types;
using GraphQLWebApi.Contracts;
using GraphQLWebApi.Entities;

namespace GraphQLWebApi.GraphQL.GraphQLTypes
{
    public class OwnerType : ObjectGraphType<Owner>
    {
        public OwnerType(IAccountRepository repository, IDataLoaderContextAccessor dataLoader)
        {
            Field(x => x.Id, type: typeof(IdGraphType)).Description("Id property from the owner object.");
            Field(x => x.Name, type: typeof(IdGraphType)).Description("Name property from the owner object.");
            Field(x => x.Address, type: typeof(IdGraphType)).Description("Address property from the owner object.");
            //Field<ListGraphType<AccountType>>(
            //    "accounts",
            //    resolve: context => repository.GetAllAccountsPerOwner(context.Source.Id)
            //);

            //use DataLoader for only one trip to database

            Field<ListGraphType<AccountType>>(
                "accounts",
                resolve: context =>
                {
                    var loader = dataLoader.Context.GetOrAddCollectionBatchLoader<Guid, Account>("GetAccountsByOwnerIds", repository.GetAccountsByOwnerIds);
                    return loader.LoadAsync(context.Source.Id);
                });
        }
    }
}
