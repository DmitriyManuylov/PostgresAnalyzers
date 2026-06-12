using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndexUsingAnalyzer.Models.InnerModels
{
    public enum TableType
    {
        Table,
        Cte,
        Subquery
    }

    internal class TableInnerModel
    {
        public string TableNsp { get; set; }

        public string TableName { get; set; }

        public string TableAlias { get; set; }

        public List<FieldInnerModel> Fields { get; set; }

        public TableType Type { get; set; }
        
    }
}
