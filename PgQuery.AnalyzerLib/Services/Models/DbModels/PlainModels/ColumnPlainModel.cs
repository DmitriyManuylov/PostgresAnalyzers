using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQuery.AnalyzerLib.Services.Models.DbModels.PlainModels
{
    public class ColumnPlainModel
    {
        public string SchemaName { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string TypeName { get; set; }
        public int ColumnOrder { get; set; }
        public int TypeMode { get; set; }
    
    }
}
