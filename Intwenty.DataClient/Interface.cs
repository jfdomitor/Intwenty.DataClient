using Intwenty.DataClient.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Intwenty.DataClient
{
    public interface IDataClient
    {
        DBMS Database { get; }
        void Open();
        Task OpenAsync();
        void Close();
        Task CloseAsync();
        void BeginTransaction();
        Task BeginTransactionAsync();
        void CommitTransaction();
        Task CommitTransactionAsync();
        void RollbackTransaction();
        Task RollbackTransactionAsync();
        void CreateTable<T>();
        Task CreateTableAsync<T>();
        bool CreateTable(IIBasicDbTable model);
        Task<bool> CreateTableAsync(IIBasicDbTable model);
        void ModifyTable<T>();


        string GetCreateTableSqlStatement<T>();
        string GetInsertSqlStatement<T>(T entity);
        string GetUpdateSqlStatement<T>(T entity);
        string GetCreateTableSqlStatement(IIBasicDbTable model);
        string GetInsertSqlStatement(IIBasicDbTable model, JsonElement data);
        string GetUpdateSqlStatement(IIBasicDbTable model, JsonElement data);


        bool TableExists<T>();
        Task<bool> TableExistsAsync<T>();
        bool TableExists(string tablename);
        Task<bool> TableExistsAsync(string tablename);
        bool ColumnExists(string tablename, string columnname);
        Task<bool> ColumnExistsAsync(string tablename, string columnname);


        void RunCommand(string sql, bool isprocedure=false, IIntwentySqlParameter[] parameters=null);
        Task RunCommandAsync(string sql, bool isprocedure = false, IIntwentySqlParameter[] parameters = null);


        object GetScalarValue(string sql, bool isprocedure = false, IIntwentySqlParameter[] parameters = null);
        Task<object> GetScalarValueAsync(string sql, bool isprocedure = false, IIntwentySqlParameter[] parameters = null);


        T GetEntity<T>(string id) where T : new();
        Task<T> GetEntityAsync<T>(string id) where T : new();
        T GetEntity<T>(int id) where T : new();
        Task<T> GetEntityAsync<T>(int id) where T : new();
        T GetEntity<T>(string sql, bool isprocedure) where T : new();
        Task<T> GetEntityAsync<T>(string sql, bool isprocedure) where T : new();
        T GetEntity<T>(string sql, bool isprocedure, IIntwentySqlParameter[] parameters = null) where T : new();
        Task<T> GetEntityAsync<T>(string sql, bool isprocedure, IIntwentySqlParameter[] parameters = null) where T : new();

        JsonElement GetEntity(IIBasicDbTable model, string id);
        Task<JsonElement> GetEntityAsync(IIBasicDbTable model, string id);
        JsonElement GetEntity(IIBasicDbTable model, int id);
        Task<JsonElement> GetEntityAsync(IIBasicDbTable model, int id);
        JsonElement GetEntity(string sql, bool isprocedure);
        Task<JsonElement> GetEntityAsync(string sql, bool isprocedure);
        JsonElement GetEntity(string sql, bool isprocedure, IIntwentySqlParameter[] parameters = null);
        Task<JsonElement> GetEntityAsync(string sql, bool isprocedure, IIntwentySqlParameter[] parameters = null);

        List<T> GetEntities<T>() where T : new();
        Task<List<T>> GetEntitiesAsync<T>() where T : new();
        List<T> GetEntities<T>(string sql, bool isprocedure=false) where T : new();
        Task<List<T>> GetEntitiesAsync<T>(string sql, bool isprocedure = false) where T : new();
        List<T> GetEntities<T>(string sql, bool isprocedure, IIntwentySqlParameter[] parameters=null) where T : new();
        Task<List<T>> GetEntitiesAsync<T>(string sql, bool isprocedure, IIntwentySqlParameter[] parameters = null) where T : new();
        
        JsonElement GetJsonArray(string sql, bool isprocedure, IIntwentySqlParameter[] parameters = null);
        JsonElement GetJsonArray(string tablename);
        Task<JsonElement> GetJsonArrayAsync(string sql, bool isprocedure, IIntwentySqlParameter[] parameters = null);
        Task<JsonElement> GetJsonArrayAsync(string tablename);






        IResultSet GetResultSet(string sql, bool isprocedure = false, IIntwentySqlParameter[] parameters = null);
        Task<IResultSet> GetResultSetAsync(string sql, bool isprocedure = false, IIntwentySqlParameter[] parameters = null);
        DataTable GetDataTable(string sql, bool isprocedure = false, IIntwentySqlParameter[] parameters = null);
        Task<DataTable> GetDataTableAsync(string sql, bool isprocedure = false, IIntwentySqlParameter[] parameters = null);



        int InsertEntity<T>(T entity);
        Task<int> InsertEntityAsync<T>(T entity);
        int InsertEntity(IIBasicDbTable model, JsonElement data);
        Task<int> InsertEntityAsync(IIBasicDbTable model, JsonElement data);


        int UpdateEntity<T>(T entity);
        Task<int> UpdateEntityAsync<T>(T entity);
        bool UpdateEntity(IIBasicDbTable model, JsonElement data);
        Task<bool> UpdateEntityAsync(IIBasicDbTable model, JsonElement data);

        int DeleteEntity<T>(T entity);
        Task<int> DeleteEntityAsync<T>(T entity);

        bool DeleteEntity(IIBasicDbTable model, string id);
        Task<bool> DeleteEntityAsync(IIBasicDbTable model, string id);
        bool DeleteEntity(IIBasicDbTable model, int id);
        Task<bool> DeleteEntityAsync(IIBasicDbTable model, int id);


        List<TypeMapItem> GetDbTypeMap();
        List<CommandMapItem> GetDbCommandMap();
    }


    public interface IIntwentySqlParameter
    {
        public string Name { get;  }
        public object Value { get; }
        public DbType DataType { get; }
        public ParameterDirection Direction { get; }
    }

    public interface IResultSet
    {
        string Name { get; set; }
        List<IResultSetRow> Rows { get;}
        public bool HasRows { get; }
    }

    public interface IResultSetRow
    {
        List<IResultSetValue> Values { get; }
        int? GetAsInt(string name);
        string GetAsString(string name);
        bool? GetAsBool(string name);
        decimal? GetAsDecimal(string name);
        DateTime? GetAsDateTime(string name);
        void SetValue(string name, object value);
    }

    public interface IResultSetValue
    {
        string Name { get; set; }
        bool HasValue { get; }
        int? GetAsInt();
        string GetAsString();
        bool? GetAsBool();
        decimal? GetAsDecimal();
        DateTime? GetAsDateTime();
        void SetValue(object value);

    }

    public interface IIBasicDbTable
    {
        public string DbTableName { get; }
        public List<IBasicDbColumn> DataColumns { get; }
        public IBasicDbColumn PrimaryKeyColumn { get; }
    }

    public interface IBasicDbColumn
    {
        public string DbColumnName { get; }
        public IntwentyDataType DataType { get; }
        public bool IsAutoIncremental { get; }
        public bool IsPrimaryKey { get; }
    }




}
