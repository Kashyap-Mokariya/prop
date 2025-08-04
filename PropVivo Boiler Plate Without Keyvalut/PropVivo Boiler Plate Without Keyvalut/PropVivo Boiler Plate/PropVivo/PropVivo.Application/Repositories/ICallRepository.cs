using PropVivo.Domain.Entities.Call;

namespace PropVivo.Application.Repositories
{
    public interface ICallRepository : ICosmosRepository<Call>
    {
        Task<IEnumerable<Call>> GetCallHistoryByCustomerIdAsync(string customerId);
        Task<Call?> GetActiveCallAsync(string phoneNumber);
    }
}
