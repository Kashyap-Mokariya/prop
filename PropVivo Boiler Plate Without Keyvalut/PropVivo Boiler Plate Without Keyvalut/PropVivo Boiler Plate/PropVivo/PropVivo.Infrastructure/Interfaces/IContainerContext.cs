using Microsoft.Azure.Cosmos;
using PropVivo.Domain.Common;

namespace PropVivo.Infrastructure.Interfaces
{
    public interface IContainerContext<T> where T : BaseEntity
    {
        string ContainerName { get; }

        string GenerateId(T entity);

        PartitionKey ResolvePartitionKey(string entityId);
    }
}