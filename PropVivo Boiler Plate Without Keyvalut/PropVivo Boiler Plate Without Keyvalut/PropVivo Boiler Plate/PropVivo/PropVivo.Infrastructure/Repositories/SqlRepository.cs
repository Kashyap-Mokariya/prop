using Dapper;
using Newtonsoft.Json;
using PropVivo.Application.Repositories;
using PropVivo.Infrastructure.Contexts;
using System.Data;
using System.Data.SqlClient;

namespace PropVivo.Infrastructure.Repositories
{
    public class SqlRepository : ISqlRepository
    {
        private readonly DapperContext _applicationContext;

        public SqlRepository(DapperContext applicationContext)
        {
            _applicationContext = applicationContext ?? throw new ArgumentNullException(nameof(applicationContext));
        }

        public int Delete(string queryName, dynamic parameters)
        {
            using (var con = _applicationContext.CreateConnection())
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var dynamicParameteres = new DynamicParameters();
                dynamicParameteres.AddDynamicParams(parameters);

                var result = con.Execute(queryName, dynamicParameteres, sqltrans, 0, System.Data.CommandType.StoredProcedure);

                if (result > 0)
                {
                    sqltrans.Commit();
                }
                else
                {
                    sqltrans.Rollback();
                }
                return result;
            }
        }

        public async Task<int> DeleteAsync(string queryName, dynamic parameters)
        {
            using (var con = _applicationContext.CreateConnection())
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var dynamicParameteres = new DynamicParameters();
                dynamicParameteres.AddDynamicParams(parameters);

                var result = await con.ExecuteAsync(queryName, dynamicParameteres, sqltrans, 0, System.Data.CommandType.StoredProcedure);

                if (result > 0)
                {
                    sqltrans.Commit();
                }
                else
                {
                    sqltrans.Rollback();
                }
                return result;
            }
        }

        public async Task<int> DeleteWithJsonAsync(string queryName, dynamic parameters)
        {
            string jsonInput = JsonConvert.SerializeObject(parameters);
            using (var con = _applicationContext.CreateConnection())
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var dynamicParameteres = new DynamicParameters();
                dynamicParameteres.AddDynamicParams(new { jsonInput });

                var result = await con.ExecuteAsync(queryName, dynamicParameteres, sqltrans, 0, System.Data.CommandType.StoredProcedure);

                if (result > 0)
                {
                    sqltrans.Commit();
                }
                else
                {
                    sqltrans.Rollback();
                }
                return result;
            }
        }

        public IEnumerable<T> GetAll<T>(string queryName, dynamic parameters) where T : class
        {
            using (var con = _applicationContext.CreateConnection())
            {
                var dynamicParameteres = new DynamicParameters();
                dynamicParameteres.AddDynamicParams(parameters);
                return con.Query<T>(queryName, dynamicParameteres, null, true, 0, System.Data.CommandType.StoredProcedure).ToList();
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(string queryName, dynamic parameters) where T : class
        {
            using (var con = _applicationContext.CreateConnection())
            {
                var dynamicParameteres = new DynamicParameters();
                dynamicParameteres.AddDynamicParams(parameters);
                return await con.QueryAsync<T>(queryName, dynamicParameteres, null, 0, System.Data.CommandType.StoredProcedure);
            }
        }

        public T GetSingleItem<T>(string queryName, dynamic parameters) where T : class
        {
            using (var con = _applicationContext.CreateConnection())
            {
                var dynamicParameteres = new DynamicParameters();
                dynamicParameteres.AddDynamicParams(parameters);
                return con.QueryFirstOrDefault<T>(queryName, dynamicParameteres, null, 0, System.Data.CommandType.StoredProcedure);
            }
        }

        public async Task<T> GetSingleItemAsync<T>(string queryName, dynamic parameters) where T : class
        {
            using (var con = _applicationContext.CreateConnection())
            {
                var dynamicParameteres = new DynamicParameters();
                dynamicParameteres.AddDynamicParams(parameters);
                return await con.QueryFirstOrDefaultAsync<T>(queryName, dynamicParameteres, null, 0, System.Data.CommandType.StoredProcedure);
            }
        }

        public async Task<T> GetSingleItemWithJsonAsync<T>(string queryName, dynamic parameters) where T : class
        {
            string jsonInput = JsonConvert.SerializeObject(parameters);
            using (var con = _applicationContext.CreateConnection())
            {
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.AddDynamicParams(new { jsonInput });

                var result = await con.QueryFirstOrDefaultAsync<string>(queryName, dynamicParameters, null, 0, System.Data.CommandType.StoredProcedure);

                return JsonConvert.DeserializeObject<T>(result);
            }
        }

        public async Task<IEnumerable<dynamic>> GetTables(string spName, object? paramteres = null, CommandType commandType = CommandType.StoredProcedure)
        {
            using (var con = _applicationContext.CreateConnection())
            {
                var multi = await con.QueryMultipleAsync(spName, paramteres, commandType: commandType);
                List<dynamic> list = new List<dynamic>();
                dynamic table = null;
                while (!multi.IsConsumed)
                {
                    table = await multi.ReadAsync();
                    list.Add(table);
                }
                return list;
            }
        }

        public int Insert(string queryName, dynamic parameters)
        {
            using (var con = _applicationContext.CreateConnection())
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var param = new DynamicParameters();
                param.Add(parameters);
                var result = con.Execute(queryName, param, sqltrans, 0, System.Data.CommandType.StoredProcedure);

                if (result > 0)
                {
                    sqltrans.Commit();
                }
                else
                {
                    sqltrans.Rollback();
                }
                return result;
            }
        }

        public async Task<int> InsertAsync(string queryName, dynamic parameters)
        {
            using (var con = _applicationContext.CreateConnection())
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var dynamicParameteres = new DynamicParameters();
                dynamicParameteres.AddDynamicParams(parameters);
                var result = await con.ExecuteAsync(queryName, dynamicParameteres, sqltrans, 0, System.Data.CommandType.StoredProcedure);

                if (result > 0)
                {
                    sqltrans.Commit();
                }
                else
                {
                    sqltrans.Rollback();
                }
                return result;
            }
        }

        public async Task<int> InsertWithJsonAsync(string queryName, dynamic parameters)
        {
            string jsonInput = JsonConvert.SerializeObject(parameters);
            using (var con = _applicationContext.CreateConnection())
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var dynamicParameteres = new DynamicParameters();
                dynamicParameteres.AddDynamicParams(new { jsonInput });
                var result = await con.ExecuteAsync(queryName, dynamicParameteres, sqltrans, 0, System.Data.CommandType.StoredProcedure);

                if (result > 0)
                {
                    sqltrans.Commit();
                }
                else
                {
                    sqltrans.Rollback();
                }
                return result;
            }
        }

        public int Update(string queryName, dynamic parameters)
        {
            using (var con = _applicationContext.CreateConnection())
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var dynamicParameteres = new DynamicParameters();
                dynamicParameteres.AddDynamicParams(parameters);
                var result = con.Execute(queryName, dynamicParameteres, sqltrans, 0, System.Data.CommandType.StoredProcedure);

                if (result > 0)
                {
                    sqltrans.Commit();
                }
                else
                {
                    sqltrans.Rollback();
                }
                return result;
            }
        }

        public async Task<int> UpdateAsync(string queryName, dynamic parameters)
        {
            using (var con = _applicationContext.CreateConnection())
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var dynamicParameteres = new DynamicParameters();
                dynamicParameteres.AddDynamicParams(parameters);
                var result = await con.ExecuteAsync(queryName, dynamicParameteres, sqltrans, 0, System.Data.CommandType.StoredProcedure);

                if (result > 0)
                {
                    sqltrans.Commit();
                }
                else
                {
                    sqltrans.Rollback();
                }
                return result;
            }
        }

        public async Task<int> UpdateWithJsonAsync(string queryName, dynamic parameters)
        {
            string jsonInput = JsonConvert.SerializeObject(parameters);
            using (var con = _applicationContext.CreateConnection())
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var dynamicParameteres = new DynamicParameters();
                dynamicParameteres.AddDynamicParams(new { jsonInput });
                var result = await con.ExecuteAsync(queryName, dynamicParameteres, sqltrans, 0, System.Data.CommandType.StoredProcedure);

                if (result > 0)
                {
                    sqltrans.Commit();
                }
                else
                {
                    sqltrans.Rollback();
                }
                return result;
            }
        }
    }
}