using Intwenty.DataClient.Model;
using System.Collections.Generic;


namespace Intwenty.DataClient.Databases
{
    abstract class BaseSqlBuilder
    {

        public BaseSqlBuilder() 
        {
          
        }

        public abstract string GetCreateTableSql(DbTableDefinition model);

        public abstract string GetAlterTableAddColumnSql(DbTableDefinition tablemodel, DbColumnDefinition columnmodel);

        public abstract string GetCreateIndexSql(DbIndexDefinition model);

        public abstract string GetInsertSql<T>(DbTableDefinition model, T instance, List<IntwentySqlParameter> parameters);

        public abstract string GetUpdateSql<T>(DbTableDefinition model, T instance, List<IntwentySqlParameter> parameters, List<IntwentySqlParameter> keyparameters);

        public abstract string GetDeleteSql<T>(DbTableDefinition model, T instance, List<IntwentySqlParameter> parameters);

        public abstract string GetModifiedSelectStatement(string sqlstatement);

        protected abstract string GetColumnDefinition(DbColumnDefinition model);

      

    }
}
