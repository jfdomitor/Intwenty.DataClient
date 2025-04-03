using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Intwenty.DataClient.Model
{

  
    sealed class DbTableDefinition : DbBaseDefinition
    {
       

        public string PrimaryKeyColumnNames 
        {
            get { return pkcolnames; }
            set 
            {
                pkcolnames = value;
                CreateStringList(PrimaryKeyColumnNamesList, pkcolnames);
            }
        }

        public List<string> PrimaryKeyColumnNamesList { get; private set; }

        public List<DbIndexDefinition> Indexes { get; set; }

        public List<DbColumnDefinition> Columns { get; set; }

        public bool HasPrimaryKeyColumn { get { return PrimaryKeyColumnNamesList.Count > 0; } }

        public bool HasAutoIncrementalColumn { get { return Columns.Exists(p=> p.IsAutoIncremental); } }

        private string pkcolnames { get; set; }


        public DbTableDefinition()
        {

            Columns = new List<DbColumnDefinition>();
            Indexes = new List<DbIndexDefinition>();
            pkcolnames = string.Empty;
            PrimaryKeyColumnNamesList = new List<string>();
        }


      
       

    }
}
