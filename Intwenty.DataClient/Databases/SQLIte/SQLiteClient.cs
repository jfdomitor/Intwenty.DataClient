using System.Data.SQLite;
using System.Data;
using Intwenty.DataClient.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.Common;

namespace Intwenty.DataClient.Databases.SQLite
{
    sealed class SQLiteClient : BaseDb, IDataClient
    {

        private SQLiteConnection connection;
        private SQLiteTransaction transaction;

        public SQLiteClient(string connectionstring, DataClientOptions options) : base(connectionstring, options)
        {

        }

        public override DBMS Database { get { return DBMS.SQLite; } }


        
        public override void Close()
        {
            if (connection != null && connection.State != ConnectionState.Closed)
            {
                connection.Close();
            }

            connection = null;
            transaction = null;
            IsInTransaction = false;

        }

        public override async Task CloseAsync()
        {
            if (connection != null && connection.State != ConnectionState.Closed)
            {
                await connection.CloseAsync();
            }

            connection = null;
            transaction = null;
            IsInTransaction = false;
        }

        private SQLiteConnection GetConnection()
        {

            if (connection != null && connection.State == ConnectionState.Open)
               return connection;

            connection = new SQLiteConnection();
            connection.ConnectionString = this.ConnectionString;
            connection.Open();

            if (IsInTransaction && transaction == null)
                transaction = connection.BeginTransaction();

            return connection;
        }

        private async Task<SQLiteConnection> GetConnectionAsync()
        {

            if (connection != null && connection.State == ConnectionState.Open)
                return connection;

            connection = new SQLiteConnection();
            connection.ConnectionString = this.ConnectionString;
            await connection.OpenAsync();

            if (IsInTransaction && transaction == null)
                transaction = (SQLiteTransaction)await connection.BeginTransactionAsync();

            return connection;
        }

        protected override IDbCommand GetCommand()
        {

            var command = new SQLiteCommand();
            command.Connection = GetConnection();
            if (IsInTransaction && transaction != null)
                command.Transaction = transaction;

            return command;
        }

        protected override async Task<DbCommand> GetCommandAsync()
        {

            var command = new SQLiteCommand();
            command.Connection = await GetConnectionAsync();
            if (IsInTransaction && transaction != null)
                command.Transaction = transaction;

            return command;
        }

        protected override DbTransaction GetTransaction()
        {
            return transaction;
        }

     

    

        protected override void AddCommandParameters(IIntwentySqlParameter[] parameters, IDbCommand command)
        {
            if (parameters == null)
                return;

            foreach (var p in parameters)
            {
                var param = new SQLiteParameter() { ParameterName = p.Name, Value = p.Value, Direction = p.Direction };
                if (param.Direction == ParameterDirection.Output)
                    param.DbType = p.DataType;

                command.Parameters.Add(param);

            }
        }

        protected override void InferAutoIncrementalValue<T>(DbTableDefinition model, List<IntwentySqlParameter> parameters, T entity, IDbCommand command)
        {
            var autoinccol = model.Columns.Find(p => p.IsAutoIncremental);
            if (autoinccol == null)
                return;


            command.CommandText = "SELECT Last_Insert_Rowid()";
            command.CommandType = CommandType.Text;
            autoinccol.Property.SetValue(entity, Convert.ToInt32(command.ExecuteScalar()), null);
            
        }

        protected override BaseSqlBuilder GetSqlBuilder()
        {
            return new SqlLiteBuilder();
        }

      

       


    }
}
