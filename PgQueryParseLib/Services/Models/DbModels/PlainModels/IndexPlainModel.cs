using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQuery.AnalyzerLib.Services.Models.DbModels.PlainModels
{
    public class IndexPlainModel
    {
        public string SchemaName { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public int ColumnOrder { get; set; }
        public string IndexName { get; set; }
        public bool IsUnique { get; set; }
        public int IndexColsCount { get; set; }
        public int IndexKeyColsCount { get; set; }
        public string IndexExpressions { get; set; }
        public string IndexWhereClause { get; set; }
        public string FullIndexDefinition { get; set; }
    }
}
