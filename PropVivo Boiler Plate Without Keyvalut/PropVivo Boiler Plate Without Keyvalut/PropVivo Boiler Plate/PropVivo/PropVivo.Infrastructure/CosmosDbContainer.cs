using Microsoft.Azure.Cosmos;
using PropVivo.Infrastructure.Interfaces;

namespace PropVivo.Infrastructure
{
    public class CosmosDbContainer : ICosmosDbContainer
    {
        public CosmosDbContainer(CosmosClient cosmosClient,
                                 string databaseName,
                                 string containerName)
        {
            this._container = cosmosClient.GetContainer(databaseName, containerName);
        }

        public Container _container { get; }
    }
}