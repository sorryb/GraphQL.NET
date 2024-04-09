using GraphQL.Types;
//using GraphQL.Utilities;
using GraphQLWebApi.GraphQL.GraphQLQueries;

namespace GraphQLWebApi.GraphQL.GraphQLSchema
{
    public class AppSchema : Schema
    {
        public AppSchema(IServiceProvider provider)
            :base(provider)
        {
            Query = provider.GetRequiredService<AppQuery>();
            Mutation = provider.GetRequiredService<AppMutation>();
        }
    }
}
