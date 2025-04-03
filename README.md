![alt text](https://github.com/Domitor/Intwenty/blob/master/IntwentyDemo/wwwroot/images/intwenty_loggo_small.png)

# Intwenty.DataClient
A .net core database client library that includes ORM functions, JSON support and more. Perfect when you need to create or retrieve data and you don't want to work with strongly type objects. 

Example:  
* bool CreateTable(IIBasicDbTable model)
* int InsertEntity(IIBasicDbTable model, JsonElement data)
* JsonElement GetJsonArray(string tablename)


## Description
Intwenty.DataClient is a laser fast database client library with a set of functions for object relational mapping and generating JSON directly from SQL query results. 

## Implementation
Instead of extending the IDbConnection Intwenty.DataClient works as a generic abstraction layer and wraps around other libraries that implements IDbConnection and IDbCommand. All methods on the IDataClient interface that not explicitly takes an sql statment as parameter (GetEntity<T>, InsertEntity<T> etc) generates sql for all supported databases.

## Performance
This is a very fast library but it relies on the DbCommand and the DataReader of the underlying libraries.

## Included libraries
* MySqlConnector
* NpgSql
* System.Data.SqlClient
* System.Data.SQLite.Core

## Supported Databases
Intwenty.DataClient is built as a wrapper around popular client libraries for MS SQLServer, MariaDb, Sqlite and Postgres. This means that all ORM functions and other functions that generates sql is guranteed to work in all databases.


## Example

    [DbTablePrimaryKey("Id")]
    public class Person {
        [AutoIncrement]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
    
    var client = new Connection(DBMS.MariaDb, "MyConnectionString");
    client.Open();
    client.CreateTable<Person>();
    
    //Insert some rows
    for (int i = 0; i < 5000; i++)
         client.InsertEntity(new Person() { FirstName = "Donald", LastName = "Duck"  });
         
     //Get a list of person objects
     var persons = client.GetEntities<Person>();
     
     //Get a filtered list of person objects
     var persons = client.GetEntities<Person>("select * from person where id>@P1", new IIntwentySqlParameter[] {new IntwentySqlParameter("@P1", 1000) });
     
    //Get persons as a json string
     var persons = client.GetJSONArry("select * from person");

    //Get a json array
    JsonElement res = dbclient.GetJsonArray("person");
  
    client.Close();
    
    

## The IDataClient interface

    
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
     
        
## Annotations
Intwenty.DataClient uses it own set of annotations to support ORM functions

       [DbTableIndex("IDX_1", false, "Col2")]
       [DbTablePrimaryKey("Col1")]
       [DbTableName("MyDbTable")]
       public class Example 
       { 
          public int Col1 { get; set; }
          public int Col2 { get; set; }
        
          [DbColumnName("MyDbColumn")]
          public string Col3 { get; set; }
        
          [NotNull]
          public int Col4 { get; set; }
        
          [Ignore]
          public int Col5 { get; set; }
       }