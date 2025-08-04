using Microsoft.Azure.Cosmos;
using PropVivo.Application.Repositories;
using PropVivo.Domain.Entities.Call;
using PropVivo.Domain.Enums;
using PropVivo.Infrastructure.Constants;
using PropVivo.Infrastructure.Interfaces;
using System.Linq.Expressions;

namespace PropVivo.Infrastructure.Repositories
{
    public class CallRepository : CosmosDbRepository<Call>, ICallRepository
    {
        public CallRepository(ICosmosDbContainerFactory factory) : base(factory)
        { }

        public override string ContainerName { get; } = CosmosDbContainerConstants.CONTAINER_NAME_CALL;

        public override string GenerateId(Call entity) => $"{Guid.NewGuid()}";

        public override PartitionKey ResolvePartitionKey(string entityId) => new PartitionKey(entityId);

        public async Task<IEnumerable<Call>> GetCallHistoryByCustomerIdAsync(string customerId)
        {
            Expression<Func<Call, bool>> filter = x => 
                x.CustomerId == customerId && x.Status == Status.Active;
            
            var request = new PropVivo.Application.Common.Base.Request();
            return await GetItemsAsync(filter, request, x => x.StartTime, nameof(Call));
        }

        public async Task<Call?> GetActiveCallAsync(string phoneNumber)
        {
            Expression<Func<Call, bool>> filter = x => 
                x.CallerPhoneNumber == phoneNumber && 
                (x.CallStatus == CallStatus.Incoming || x.CallStatus == CallStatus.Connected);
            
            return await GetItemAsync(filter, nameof(Call));
        }
    }
}
