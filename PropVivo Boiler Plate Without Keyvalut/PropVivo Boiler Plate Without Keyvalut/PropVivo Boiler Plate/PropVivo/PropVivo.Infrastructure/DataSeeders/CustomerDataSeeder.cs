using PropVivo.Application.Repositories;
using PropVivo.Domain.Common;
using PropVivo.Domain.Entities.Customer;
using PropVivo.Domain.Enums;

namespace PropVivo.Infrastructure.DataSeeders
{
    public class CustomerDataSeeder
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerDataSeeder(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task SeedDataAsync()
        {
            var customers = new List<Customer>
            {
                new Customer
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@email.com",
                    PhoneNumber = "+1234567890",
                    Company = "Tech Corp",
                    Address = new Address
                    {
                        Address1 = "123 Main St",
                        City = "New York",
                        State = "NY",
                        ZipCode = "10001",
                        Country = "USA"
                    },
                    Status = Status.Active,
                    UserContext = new UserBase
                    {
                        CreatedByUserId = "system",
                        CreatedByUserName = "System",
                        CreatedOn = DateTime.UtcNow
                    }
                },
                new Customer
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane.smith@email.com",
                    PhoneNumber = "+1987654321",
                    Company = "Business Inc",
                    Status = Status.Active,
                    UserContext = new UserBase
                    {
                        CreatedByUserId = "system",
                        CreatedByUserName = "System",
                        CreatedOn = DateTime.UtcNow
                    }
                }
            };

            foreach (var customer in customers)
            {
                var existing = await _customerRepository.GetCustomerByPhoneNumberAsync(customer.PhoneNumber);
                if (existing == null)
                {
                    await _customerRepository.AddItemAsync(customer, nameof(Customer));
                }
            }
        }
    }
}
