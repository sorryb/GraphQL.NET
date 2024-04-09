using GraphQLWebApi.Entities;

namespace GraphQLWebApi.Contracts
{
    public interface IAccountRepository
    {
        IEnumerable<Account> GetAllAccountsPerOwner(Guid ownerId);
        
        /// <summary>
        /// It use DataLoaders to load accounts for each owner.
        /// </summary>
        /// <param name="ownerIds"></param>
        /// <returns></returns>
        Task<ILookup<Guid, Account>> GetAccountsByOwnerIds(IEnumerable<Guid> ownerIds);
    }
}
