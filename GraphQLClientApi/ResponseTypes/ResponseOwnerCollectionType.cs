using GraphQLClientApi.OwnerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLClientApi.ResponseTypes
{
    public class ResponseOwnerCollectionType
    {
        public List<Owner> Owners { get; set; }
    }
}
