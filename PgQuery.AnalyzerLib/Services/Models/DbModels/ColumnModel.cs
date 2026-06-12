using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQuery.AnalyzerLib.Services.Models.DbModels
{
    public class ColumnModel
    {
        public string ColumnName { get; set; }
        public string TypeName { get; set; }
        public int? TypeMode { get; set; }
        public List<IndexModel> ColumnIndices {  get; set; }
    }
}
