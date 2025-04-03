using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Intwenty.DataClient.Model
{
    public enum IntwentyDataType
    {
        Bool = 0
      , String = 1
      , Text = 2
      , Int = 3
      , DateTime = 4
      , OneDecimal = 5
      , TwoDecimal = 6
      , ThreeDecimal = 7
      , Blob = 8
    }

    public enum StringLength { Standard, Long, Short };

    public class TypeMapItem
    {
        public StringLength Length { get; set; }

        public string NetType { get; set; }

        public IntwentyDataType IntwentyType { get; set; }

        public DbType DataDbType { get; set; }

        public DBMS DbEngine { get; set; }

        public string DBMSDataType { get; set; }

        public TypeMapItem()
        {
            Length = StringLength.Standard;
        }
    }
}
