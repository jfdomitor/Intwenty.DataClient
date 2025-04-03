using Intwenty.DataClient.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Intwenty.DataClient.Databases.SqlServer
{
    sealed class SqlServerClient : BaseDb, IDataClient
    {

        private SqlConnection connection;
        private SqlTransaction transaction;

        public SqlServerClient(string connectionstring, DataClientOptions options) : base(connectionstring, options)
        {

        }

        public override DBMS Database { get { return DBMS.MSSqlServer; } }

       
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

        private SqlConnection GetConnection()
        {

            if (connection != null && connection.State == ConnectionState.Open)
                return connection;

            connection = new SqlConnection();
            connection.ConnectionString = this.ConnectionString;
            connection.Open();

            if (IsInTransaction && transaction == null)
                transaction = connection.BeginTransaction();

            return connection;
        }

        private async Task<SqlConnection> GetConnectionAsync()
        {

            if (connection != null && connection.State == ConnectionState.Open)
                return connection;

            connection = new SqlConnection();
            connection.ConnectionString = this.ConnectionString;
            await connection.OpenAsync();

            if (IsInTransaction && transaction == null)
                transaction = (SqlTransaction)await connection.BeginTransactionAsync();

            return connection;
        }

        protected override IDbCommand GetCommand()
        {
            var command = new SqlCommand();
            command.Connection = GetConnection();
            if (IsInTransaction && transaction != null)
                command.Transaction = transaction;

            return command;
        }

        protected override async Task<DbCommand> GetCommandAsync()
        {

            var command = new SqlCommand();
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
                var param = new SqlParameter() { ParameterName = p.Name, Value = p.Value, Direction = p.Direction };
                if (param.Direction == ParameterDirection.Output)
                    param.DbType = p.DataType;

                command.Parameters.Add(param);

            }
        }


        protected override BaseSqlBuilder GetSqlBuilder()
        {
            return new SqlServerBuilder();
        }

        protected override void SetPropertyValues<T>(IDataReader reader, KeyValuePair<int,DbColumnDefinition> column, T instance)
        {
            if (column.Value.IsInt32)
                column.Value.Property.SetValue(instance, reader.GetInt32(column.Key), null);
            else if (column.Value.IsBoolean)
                column.Value.Property.SetValue(instance, Convert.ToBoolean(reader.GetInt32(column.Key)), null);
            else if (column.Value.IsDecimal)
                column.Value.Property.SetValue(instance, Convert.ToDecimal(reader.GetValue(column.Key)), null);
            else if (column.Value.IsSingle)
                column.Value.Property.SetValue(instance, Convert.ToSingle(reader.GetValue(column.Key)), null);
            else if (column.Value.IsDouble)
                column.Value.Property.SetValue(instance, Convert.ToDouble(reader.GetValue(column.Key)), null);
            else
                base.SetPropertyValues(reader, column, instance);

          
        }

        protected override void InferAutoIncrementalValue<T>(DbTableDefinition model, List<IntwentySqlParameter> parameters, T entity, IDbCommand command)
        {
            if (model == null)
                return;

            if (command == null)
                return;

            if (command.Parameters == null || parameters == null)
                return;

            var output = parameters.Find(p => p.Direction == ParameterDirection.Output);
            if (output == null)
                return;

            foreach (SqlParameter p in command.Parameters)
            {
                if (p.Direction == ParameterDirection.Output && p.ParameterName.ToLower() == output.Name.ToLower())
                {
                    

                    if (!model.HasAutoIncrementalColumn)
                        return;

                    var autoinccol = model.Columns.Find(p => p.IsAutoIncremental);
                    if (autoinccol == null)
                        return;

                    autoinccol.Property.SetValue(entity, p.Value);

                    break;

                }
                   
            }



          

        }


    }
}
