using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace basicapiwithdotnet.DataAccess;

public class DataContextDapper
{

    private readonly IConfiguration _configuration;
    private readonly IDbConnection _dbConnection;

    public DataContextDapper(IConfiguration configuration)
    { 
        _configuration = configuration;
        _dbConnection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
    }


    public IEnumerable<T> LoadData<T>(string sql) => _dbConnection.Query<T>(sql);

    public T LoadDataSingle<T>(string sql) => _dbConnection.QuerySingle<T>(sql);
    public IEnumerable<T> LoadDataWithParameters<T>(string sql, object parameters) =>  _dbConnection.Query<T>(sql, parameters);

    public bool ExecuteSql(string sql) => _dbConnection.Execute(sql) > 0;

    public int ExecuteSqlWithRowCount(string sql, object parameters) => _dbConnection.Execute(sql, parameters);

    public T ExecuteScalar<T>(string sql, object? parameters = null)
    {
        var result = _dbConnection.ExecuteScalar<T>(sql, parameters) ?? throw new InvalidOperationException("Query returned null.");
        return result;
    }

    
}