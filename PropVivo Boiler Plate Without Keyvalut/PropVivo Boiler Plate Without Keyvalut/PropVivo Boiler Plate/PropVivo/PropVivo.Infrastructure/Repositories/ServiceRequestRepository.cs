using Microsoft.Azure.Cosmos;
using PropVivo.Application.Repositories;
using PropVivo.Domain.Entities.ServiceRequest;
using PropVivo.Infrastructure.Constants;
using PropVivo.Infrastructure.Interfaces;

namespace PropVivo.Infrastructure.Repositories
{
    public class ServiceRequestRepository : CosmosDbRepository<ServiceRequests>, IServiceRequestRepository
    {
        public ServiceRequestRepository(ICosmosDbContainerFactory factory) : base(factory)
        { }

        public override string ContainerName { get; } = CosmosDbContainerConstants.CONTAINER_NAME_ServiceRequest;

        public override string GenerateId(ServiceRequests entity) => $"{Guid.NewGuid()}";

        public override PartitionKey ResolvePartitionKey(string entityId) => new PartitionKey(entityId);
    }
}