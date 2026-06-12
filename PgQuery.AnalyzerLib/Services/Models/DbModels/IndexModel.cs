using DataChangeAnalyzer.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQuery.AnalyzerLib.Services.Models.DbModels
{
    public class IndexModel
    {
        public string IndexName { get; set; }
        public bool IsUnique { get; set; }
        public int IndexColsCount { get; set; }
        public int IndexKeyColsCount { get; set; }
        public string IndexExpressions { get; set; }
        public string IndexWhereClause { get; set; }
        public string FullIndexDefinition { get; set; }
        public List<ColumnModel> Columns { get; set; }
        public TableModel Table { get; set; }
    }
}
