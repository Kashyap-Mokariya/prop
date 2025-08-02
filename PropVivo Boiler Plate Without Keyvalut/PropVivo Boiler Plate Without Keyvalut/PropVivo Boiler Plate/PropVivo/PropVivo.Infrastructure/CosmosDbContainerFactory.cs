using Microsoft.Azure.Cosmos;
using PropVivo.Infrastructure.AppSettings;
using PropVivo.Infrastructure.Interfaces;

namespace PropVivo.Infrastructure
{
    public class CosmosDbContainerFactory : ICosmosDbContainerFactory
    {
        /// <summary>
        /// Azure Cosmos DB Client
        /// </summary>
        private readonly CosmosClient _cosmosClient;

        private readonly CosmosDbSettings _cosmosDbConfig;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cosmosClient"></param>
        /// <param name="databaseName"></param>
        /// <param name="containers"></param>
        public CosmosDbContainerFactory(CosmosClient cosmosClient,
                                  CosmosDbSettings cosmosDbConfig
                                   )
        {
            _cosmosClient = cosmosClient ?? throw new ArgumentNullException(nameof(cosmosClient));
            _cosmosDbConfig = cosmosDbConfig ?? throw new ArgumentNullException(nameof(cosmosDbConfig));
        }

        public async Task EnsureDbSetupAsync()
        {
            if (_cosmosDbConfig == null || _cosmosDbConfig.Containers == null || string.IsNullOrEmpty(_cosmosDbConfig.DefaultPartition))
                return;

            DatabaseResponse database = await CreateDatabaseAsync();

            foreach (ContainerInfo container in _cosmosDbConfig.Containers)
            {
                if (container.ThroughPut == null)
                    container.ThroughPut = _cosmosDbConfig.DefaultThroughPut;

                if (container.Partitions == null || !container.Partitions.Any())
                    container.Partitions = new List<string> { _cosmosDbConfig.DefaultPartition };

                ContainerProperties containerProperties = new ContainerProperties();
                containerProperties.Id = container.Name;
                containerProperties.PartitionKeyPaths = container.Partitions;
                await database.Database.CreateContainerIfNotExistsAsync(containerProperties);
            }
        }

        public ICosmosDbContainer GetContainer(string containerName)
        {
            if (_cosmosDbConfig == null || _cosmosDbConfig.Containers == null)
                throw new ArgumentException($"Unable to find");

            if (_cosmosDbConfig.Containers.Where(x => x.Name == containerName) == null)
                throw new ArgumentException($"Unable to find container: {containerName}");

            if (string.IsNullOrEmpty(_cosmosDbConfig.DatabaseName))
                throw new ArgumentException($"Unable to find database");

            return new CosmosDbContainer(_cosmosClient, _cosmosDbConfig.DatabaseName, containerName);
        }

        // <CreateDatabaseAsync>
        /// <summary>
        /// Create the database if it does not exist
        /// </summary>
        private async Task<DatabaseResponse> CreateDatabaseAsync()
        {
            // Create a new database
            return await _cosmosClient.CreateDatabaseIfNotExistsAsync(_cosmosDbConfig.DatabaseName);
        }

        // </CreateDatabaseAsync>
    }
}