using PropVivo.Domain.Entities.Customer;

namespace PropVivo.Application.Repositories
{
    public interface ICustomerRepository : ICosmosRepository<Customer>
    {
        Task<Customer?> GetCustomerByPhoneNumberAsync(string phoneNumber);
        Task<IEnumerable<Customer>> SearchCustomersAsync(string searchTerm);
    }
}
