using System;
using System.Collections.Generic;
using System.Text;

namespace Intwenty.DataClient.Model
{
    sealed class DbIndexDefinition : DbBaseDefinition
    {

        public bool IsUnique { get; set; }

        public string TableName { get; set; }

        public string ColumnNames
        {
            get { return IndexColumns; }
            set
            {
                IndexColumns = value;
                CreateStringList(ColumnNamesList, IndexColumns);
            }
        }

        public List<string> ColumnNamesList { get; private set; }

        private string IndexColumns;

        public DbIndexDefinition()
        {
            IndexColumns = string.Empty;
            ColumnNamesList = new List<string>();
        }
    }
}
