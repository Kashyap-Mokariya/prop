using Microsoft.Azure.Cosmos;
using PropVivo.Application.Repositories;
using PropVivo.Domain.Entities.Customer;
using PropVivo.Domain.Enums;
using PropVivo.Infrastructure.Constants;
using PropVivo.Infrastructure.Interfaces;
using System.Linq.Expressions;

namespace PropVivo.Infrastructure.Repositories
{
    public class CustomerRepository : CosmosDbRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(ICosmosDbContainerFactory factory) : base(factory)
        { }

        public override string ContainerName { get; } = CosmosDbContainerConstants.CONTAINER_NAME_CUSTOMER;

        public override string GenerateId(Customer entity) => $"{Guid.NewGuid()}";

        public override PartitionKey ResolvePartitionKey(string entityId) => new PartitionKey(entityId);

        public async Task<Customer?> GetCustomerByPhoneNumberAsync(string phoneNumber)
        {
            Expression<Func<Customer, bool>> filter = x => 
                x.PhoneNumber == phoneNumber && x.Status == Status.Active;
            
            return await GetItemAsync(filter, nameof(Customer));
        }

        public async Task<IEnumerable<Customer>> SearchCustomersAsync(string searchTerm)
        {
            Expression<Func<Customer, bool>> filter = x => 
                (x.FirstName != null && x.FirstName.Contains(searchTerm)) ||
                (x.LastName != null && x.LastName.Contains(searchTerm)) ||
                (x.Email != null && x.Email.Contains(searchTerm)) ||
                (x.PhoneNumber != null && x.PhoneNumber.Contains(searchTerm));
            
            var request = new PropVivo.Application.Common.Base.Request();
            return await GetItemsAsync(filter, request, x => x.UserContext.CreatedOn, nameof(Customer));
        }
    }
}
