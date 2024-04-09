using GraphQLWebApi.Entities;

namespace GraphQLWebApi.Contracts
{
    public interface IOwnerRepository
    {
        IEnumerable<Owner> GetAll();
        Owner GetById(Guid id);

        Owner CreateOwner(Owner owner);

        Owner UpdateOwner(Owner dbOwner, Owner owner);

        void DeleteOwner(Owner owner);
    }
}
