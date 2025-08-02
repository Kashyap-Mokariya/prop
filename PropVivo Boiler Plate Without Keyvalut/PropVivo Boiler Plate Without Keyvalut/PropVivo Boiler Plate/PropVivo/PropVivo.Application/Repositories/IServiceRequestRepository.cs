using PropVivo.Domain.Entities.ServiceRequest;

namespace PropVivo.Application.Repositories
{
    public interface IServiceRequestRepository : ICosmosRepository<ServiceRequests>
    {
    }
}