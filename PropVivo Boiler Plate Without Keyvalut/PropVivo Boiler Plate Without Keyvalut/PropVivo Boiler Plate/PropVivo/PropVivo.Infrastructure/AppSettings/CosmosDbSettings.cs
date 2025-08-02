namespace PropVivo.Infrastructure.AppSettings
{
    public class ContainerInfo
    {
        /// <summary>
        /// Container Name
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Container partition Key
        /// </summary>
        public List<string>? Partitions { get; set; }

        public int? ThroughPut { get; set; }
    }

    public class CosmosDbSettings
    {
        /// <summary>
        /// List of containers in the database
        /// </summary>
        public List<ContainerInfo>? Containers { get; set; }

        /// <summary>
        /// Database name
        /// </summary>
        public string? DatabaseName { get; set; }

        public string? DefaultPartition { get; set; }

        public int DefaultThroughPut { get; set; }

        /// <summary>
        /// CosmosDb Account - The Azure Cosmos DB endpoint
        /// </summary>
        public string? EndpointUrl { get; set; }

        /// <summary>
        /// Key - The primary key for the Azure DocumentDB account.
        /// </summary>
        public string? PrimaryKey { get; set; }
    }
}