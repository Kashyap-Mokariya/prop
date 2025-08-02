using PropVivo.Application.Common.Base;
using PropVivo.Domain.Common;
using System.Linq.Expressions;

namespace PropVivo.Application.Repositories
{
    public interface ICosmosRepository<T> where T : BaseEntity
    {
        Task<T> AddItemAsync(T item, string partitionName = "");

        Task<IEnumerable<T>> AddItemsAsync(IEnumerable<T> items, string partitionname = "");

        Task<IEnumerable<T>> AddItemsAsync(Dictionary<T, string> itemsWithPartitionKeys);

        Task<T> DeleteItemAsync(string id, string partitionName = "");

        /// <summary>
        /// Get one item by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> GetItemAsync(string id, string partitionName = "");

        Task<T?> GetItemAsync(Expression<Func<T, bool>> predicate, string? documentType = null);

        /// <summary>
        /// Get items given a string SQL query directly. Likely in production, you may want to use
        /// alternatives like Parameterized Query or LINQ to avoid SQL Injection and avoid having to
        /// work with strings directly. This is kept here for demonstration purpose.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetItemsAsync(string query);

        Task<IEnumerable<T>> GetItemsAsync<TPropType>(Expression<Func<T, bool>> predicate, Request request, Expression<Func<T, TPropType>>? orderBy = null, string? documentType = null);

        Task<int> GetItemsCountAsync(Expression<Func<T, bool>> predicate, Request request, string? documentType = null);

        Task<(IEnumerable<T> data, int count)> GetItemsWithCountAsync<TPropType>(Expression<Func<T, bool>> predicate, Request request, Expression<Func<T, TPropType>>? orderBy = null, string? documentType = null);

        Expression<Func<T, bool>> GetQuery(Request request);

        Task<T> UpdateItemAsync(string id, T item, string partitionName = "");

        Task<IEnumerable<T>> UpdateItemsAsync(IEnumerable<T> items, string partitionname = "");
    }
}