using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Intwenty.DataClient.Model
{
    sealed class DbColumnDefinition : DbBaseDefinition
    {

        public bool IsPrimaryKeyColumn { get; set; }

        public bool IsAutoIncremental { get; set; }

        public bool IsNullNotAllowed { get; set; }

        public PropertyInfo Property { get; set; }

        public string GetNetType()
        {
            if (Property == null)
                return string.Empty;

            var typestring = Property.PropertyType.ToString();

            if (typestring.Contains("["))
            {
                var index1 = typestring.IndexOf("[");
                var index2 = typestring.IndexOf("]");
                typestring = typestring.Substring(index1 + 1, (index2) - (index1 + 1));
            }

            return typestring.ToUpper();
        }

        public bool IsSingle{ get; set; }

        public bool IsInt32 { get; set; }

        public bool IsDecimal { get; set; }

        public bool IsString { get; set; }

        public bool IsDateTime { get; set; }

        public bool IsBoolean { get; set; }

        public bool IsDouble{ get; set; }

        public bool IsDateTimeOffset { get; set; }

    }
}
