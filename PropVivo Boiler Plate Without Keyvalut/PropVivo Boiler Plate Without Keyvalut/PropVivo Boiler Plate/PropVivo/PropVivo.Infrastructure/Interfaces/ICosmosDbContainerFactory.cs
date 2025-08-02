namespace PropVivo.Infrastructure.Interfaces
{
    public interface ICosmosDbContainerFactory
    {
        /// <summary>
        /// Ensure the database is created
        /// </summary>
        /// <returns></returns>
        Task EnsureDbSetupAsync();

        /// <summary>
        /// Returns a CosmosDbContainer wrapper
        /// </summary>
        /// <param name="containerName"></param>
        /// <returns></returns>
        ICosmosDbContainer GetContainer(string containerName);
    }
}