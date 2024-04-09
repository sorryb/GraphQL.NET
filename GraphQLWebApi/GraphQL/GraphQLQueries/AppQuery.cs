using GraphQL;
using GraphQL.Types;
using GraphQLWebApi.Contracts;
using GraphQLWebApi.GraphQL.GraphQLTypes;

namespace GraphQLWebApi.GraphQL.GraphQLQueries
{
    public class AppQuery : ObjectGraphType
    {
        public AppQuery(IOwnerRepository repository)
        {
            Field<ListGraphType<OwnerType>>(
               "owners",
               resolve: context => repository.GetAll()
            );

            //Field<OwnerType>(
            //    "owner",
            //    arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "ownerId" }),
            //    resolve: context =>
            //    {
            //        var id = context.GetArgument<Guid>("ownerId");
            //        return repository.GetById(id);
            //    }
            //);
            Field<OwnerType>(
                "owner",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "ownerId" }),
                resolve: context =>
                {
                    Guid id;
                    if (!Guid.TryParse(context.GetArgument<string>("ownerId"), out id))
                    {
                        context.Errors.Add(new ExecutionError("Wrong value for guid as ownerId!"));
                        return null;
                    }

                    return repository.GetById(id);
                }
            );
        }
    }
}
