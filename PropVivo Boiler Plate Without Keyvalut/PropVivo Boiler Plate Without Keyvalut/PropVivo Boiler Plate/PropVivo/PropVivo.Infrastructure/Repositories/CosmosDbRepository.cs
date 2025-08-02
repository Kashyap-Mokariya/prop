using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Repositories;
using PropVivo.Domain.Common;
using PropVivo.Infrastructure.Helper;
using PropVivo.Infrastructure.Interfaces;
using System.Linq.Expressions;

namespace PropVivo.Infrastructure.Repositories
{
    public abstract class CosmosDbRepository<T> : ICosmosRepository<T>, IContainerContext<T> where T : BaseEntity
    {
        /// <summary>
        /// Cosmos DB container
        /// </summary>
        private readonly Container _container;

        /// <summary>
        /// Cosmos DB factory
        /// </summary>
        private readonly ICosmosDbContainerFactory _cosmosDbContainerFactory;

        public CosmosDbRepository(ICosmosDbContainerFactory cosmosDbContainerFactory)
        {
            this._cosmosDbContainerFactory = cosmosDbContainerFactory ?? throw new ArgumentNullException(nameof(ICosmosDbContainerFactory));
            this._container = this._cosmosDbContainerFactory.GetContainer(ContainerName)._container;
        }

        /// <summary>
        /// Name of the CosmosDB container
        /// </summary>
        public abstract string ContainerName { get; }

        // <AddItemAsync>
        /// <summary>
        /// Add items to the container
        /// </summary>
        public async Task<T> AddItemAsync(T item, string partitionname = "")
        {
            var partitionKey = string.IsNullOrEmpty(partitionname) ? ResolvePartitionKey(typeof(T).Name) : new PartitionKey(partitionname);
            return await _container.CreateItemAsync<T>(item, partitionKey);
        }

        ///</AddItemAsync>

        // <AddItemsAsync>
        /// <summary>
        /// Add items to the container
        /// </summary>
        public async Task<IEnumerable<T>> AddItemsAsync(IEnumerable<T> items, string partitionname = "")
        {
            var insertedItems = new List<T>();
            var partitionKey = string.IsNullOrEmpty(partitionname) ? ResolvePartitionKey(typeof(T).Name) : new PartitionKey(partitionname);
            foreach (var item in items)
            {
                var insertedItem = await _container.CreateItemAsync<T>(item, partitionKey);
                insertedItems.Add(insertedItem.Resource);
            }
            return insertedItems;
        }

        // <AddItemsAsync>
        /// <summary>
        /// Add items to the container with specified partition keys
        /// </summary>
        public async Task<IEnumerable<T>> AddItemsAsync(Dictionary<T, string> itemsWithPartitionKeys)
        {
            var insertedItems = new List<T>();

            foreach (var kvp in itemsWithPartitionKeys)
            {
                var partitionKey = new PartitionKey(kvp.Value);
                var insertedItem = await _container.CreateItemAsync<T>(kvp.Key, partitionKey);
                insertedItems.Add(insertedItem.Resource);
            }

            return insertedItems;
        }

        ///</AddItemsAsync>

        // <DeleteItemAsync>
        /// <summary>
        /// Delete an item in the container
        /// </summary>
        public async Task<T> DeleteItemAsync(string id, string partitionname = "")
        {
            var partitionKey = string.IsNullOrEmpty(partitionname) ? ResolvePartitionKey(typeof(T).Name) : new PartitionKey(partitionname);
            return await this._container.DeleteItemAsync<T>(id, partitionKey);
        }

        ///</DeleteItemAsync>

        /// <summary>
        /// Generate id
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public abstract string GenerateId(T entity);

        public async Task<T> GetItemAsync(string id, string partitionname = "")
        {
            try
            {
                var partitionKey = string.IsNullOrEmpty(partitionname) ? ResolvePartitionKey(typeof(T).Name) : new PartitionKey(partitionname);
                ItemResponse<T> response = await _container.ReadItemAsync<T>(id, partitionKey);
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return default;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<T?> GetItemAsync(Expression<Func<T, bool>> predicate, string? documentType = null)
        {
            try
            {
                // Build LINQ query
                IQueryable<T> query = _container.GetItemLinqQueryable<T>()
                    .Where(a => string.IsNullOrEmpty(documentType) ? a.DocumentType == typeof(T).Name : a.DocumentType == documentType)
                    .Where(predicate);

                // Execute query and retrieve results
                FeedIterator<T> resultSetIterator = query.ToFeedIterator();
                List<T> results = new List<T>();

                while (resultSetIterator.HasMoreResults)
                {
                    FeedResponse<T> response = await resultSetIterator.ReadNextAsync();
                    results.AddRange(response);
                }

                // Return the first item that matches the predicate, or default if none found
                return results?.FirstOrDefault();
            }
            catch (CosmosException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<T>> GetItemsAsync<TPropType>(Expression<Func<T, bool>> predicate, Request request, Expression<Func<T, TPropType>>? orderBy = null, string? documentType = null)
        {
            var results = new List<T>();
            try
            {
                IQueryable<T> query_ExtractRecords = _container.GetItemLinqQueryable<T>()
                                        .Where(a => a.DocumentType == (string.IsNullOrEmpty(documentType) ? typeof(T).Name : documentType))
                                        .Where(predicate);

                if (orderBy != null)
                {
                    if (request.OrderByCriteria != null)
                        query_ExtractRecords = request.OrderByCriteria.OrderBy == "Ascending" ? query_ExtractRecords.OrderBy(orderBy) : query_ExtractRecords.OrderByDescending(orderBy);
                    else
                        query_ExtractRecords = query_ExtractRecords.OrderByDescending(orderBy);
                }

                if (request?.PageCriteria?.EnablePage ?? false)
                    query_ExtractRecords = query_ExtractRecords
                                            .Skip(request.PageCriteria.Skip)
                                            .Take(request.PageCriteria.PageSize);

                FeedIterator<T> resultSetIterator = query_ExtractRecords.ToFeedIterator();

                while (resultSetIterator.HasMoreResults)
                {
                    FeedResponse<T> response = await resultSetIterator.ReadNextAsync();
                    results.AddRange(response.ToList());
                }

                return results;
            }
            catch (Exception)
            {
                return results.AsEnumerable(); ;
            }
        }

        // <GetItemsAsync>
        /// <summary>
        /// Run a query (using Azure Cosmos DB SQL syntax) against the container
        /// </summary>
        // Search data using SQL query string This shows how to use SQL string to read data from
        // Cosmos DB for demonstration purpose. For production, try to use safer alternatives like
        // Parameterized Query and LINQ if possible. Using string can expose SQL Injection
        // vulnerability, e.g. select * from c where c.id=1 OR 1=1. String can also be hard to work
        // with due to special characters and spaces when advanced querying like search and
        // pagination is required.
        public async Task<IEnumerable<T>> GetItemsAsync(string queryString)
        {
            //QueryRequestOptions options = new();
            //options.MaxItemCount = 5;
            try
            {
                FeedIterator<T> resultSetIterator = _container.GetItemQueryIterator<T>(new QueryDefinition(queryString));
                List<T> results = new List<T>();
                while (resultSetIterator.HasMoreResults)
                {
                    FeedResponse<T> response = await resultSetIterator.ReadNextAsync();
                    results.AddRange(response.ToList());
                }

                return results;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> GetItemsCountAsync(Expression<Func<T, bool>> predicate, Request request, string? documentType = null)
        {
            try
            {
                var count = 0;
                count = _container.GetItemLinqQueryable<T>(true)
                                    .Where(a => a.DocumentType == (string.IsNullOrEmpty(documentType) ? typeof(T).Name : documentType))
                                    .Where(predicate).Count();

                return await Task.FromResult(count);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<(IEnumerable<T> data, int count)> GetItemsWithCountAsync<TPropType>(Expression<Func<T, bool>> predicate, Request request, Expression<Func<T, TPropType>>? orderBy = null, string? documentType = null)
        {
            var results = new List<T>();
            try
            {
                IQueryable<T> query_ExtractRecords = _container.GetItemLinqQueryable<T>()
                                        .Where(a => a.DocumentType == (string.IsNullOrEmpty(documentType) ? typeof(T).Name : documentType))
                                        .Where(predicate);

                if (orderBy != null)
                {
                    if (request.OrderByCriteria != null)
                        query_ExtractRecords = request.OrderByCriteria.OrderBy == "Ascending" ? query_ExtractRecords.OrderBy(orderBy) : query_ExtractRecords.OrderByDescending(orderBy);
                    else
                        query_ExtractRecords = query_ExtractRecords.OrderByDescending(orderBy);
                }

                var count = await query_ExtractRecords.CountAsync();

                if (request?.PageCriteria?.EnablePage ?? false)
                    query_ExtractRecords = query_ExtractRecords
                                            .Skip(request.PageCriteria.Skip)
                                            .Take(request.PageCriteria.PageSize);

                FeedIterator<T> resultSetIterator = query_ExtractRecords.ToFeedIterator();

                while (resultSetIterator.HasMoreResults)
                {
                    FeedResponse<T> response = await resultSetIterator.ReadNextAsync();
                    results.AddRange(response.ToList());
                }

                return (results, count);
            }
            catch (Exception)
            {
                return (results.AsEnumerable(), 0);
            }
        }

        // </GetItemsAsync>
        public Expression<Func<T, bool>> GetQuery(Request request)
        {
            Expression<Func<T, bool>> filterExpression = x => true;

            if (request.FilterByCriteria != null)
            {
                foreach (var filterByCriteria in request.FilterByCriteria)
                {
                    if (!string.IsNullOrEmpty(filterByCriteria.Field) && filterByCriteria.Value != null)
                    {
                        var lambdaExpression = ExpressionHelper.GetCriteriaWhere<T>(filterByCriteria.Field, filterByCriteria.OperationExpression, filterByCriteria.Value);
                        filterExpression = filterExpression.And(lambdaExpression);
                    }
                }
            }

            return filterExpression;
        }

        public Expression<Func<T, object>>? OrderBy(Request request)
        {
            if (request != null && request.OrderByCriteria != null && !string.IsNullOrEmpty(request.OrderByCriteria.Order))
                return Sort(request.OrderByCriteria.Order);

            return Sort(string.Empty);
        }

        /// <summary>
        /// Resolve the partition key for the audit record. All entities will share the same audit
        /// container, so we can define this method here with virtual default implementation. Audit
        /// records for different entities will use different partition key values, so we are not
        /// limited to the 20G per logical partition storage limit.
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public virtual PartitionKey ResolveAuditPartitionKey(string entityId) => new PartitionKey($"{entityId}");

        /// <summary>
        /// Resolve the partition key
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public abstract PartitionKey ResolvePartitionKey(string entityId);

        public async Task<T> UpdateItemAsync(string id, T item, string partitionname = "")
        {
            // Update
            var partitionKey = string.IsNullOrEmpty(partitionname) ? ResolvePartitionKey(typeof(T).Name) : new PartitionKey(partitionname);
            return await this._container.UpsertItemAsync<T>(item, partitionKey);
        }

        public async Task<IEnumerable<T>> UpdateItemsAsync(IEnumerable<T> items, string partitionname = "")
        {
            var updatedtItems = new List<T>();
            var partitionKey = string.IsNullOrEmpty(partitionname) ? ResolvePartitionKey(typeof(T).Name) : new PartitionKey(partitionname);
            foreach (var item in items)
            {
                var updatedtItem = await this._container.UpsertItemAsync<T>(item, partitionKey);
                updatedtItems.Add(updatedtItem.Resource);
            }
            return updatedtItems;
        }

        // </ReplaceItemAsync>
        /// Replace an item in the container </summary>
        private async Task<T> ReplaceItemAsync(string id, T document, string partitionname = "")
        {
            // replace the item with the updated content
            var partitionKey = string.IsNullOrEmpty(partitionname) ? ResolvePartitionKey(typeof(T).Name) : new PartitionKey(partitionname);
            var response = await _container.ReplaceItemAsync<T>(document, id, partitionKey);
            return response;
            //Console.WriteLine("Updated Family [{0},{1}].\n \tBody is now: {2}\n", itemBody.LastName, itemBody.Id, wakefieldFamilyResponse.Resource);
        }

        // </ReplaceItemAsync>

        private Expression<Func<T, object>>? Sort(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                return null;

            var parameter = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameter, propertyName);
            var propAsObject = Expression.Convert(property, typeof(object));

            var sortExpression = Expression.Lambda<Func<T, object>>(propAsObject, parameter);

            return sortExpression;
        }
    }
}