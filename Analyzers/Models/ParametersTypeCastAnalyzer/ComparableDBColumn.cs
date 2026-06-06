using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQuery.AnalyzerLib.GenericWalkers.Models.SemanticAnalyzer.Results
{
    public class ComparableDBColumn
    {
        public string SchemaName { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string ColumnType { get; set; }
        public int? TypeMod { get; set; }
        public bool HasIndex { get; set; }
    }
}
