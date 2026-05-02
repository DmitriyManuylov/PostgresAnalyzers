using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQueryParseLib.Models
{
    /// <summary>
    /// Узлы дерева операций Insert, Update, Delete
    /// </summary>
    public class DMLAnalyzeModel : PgGenericNode
    {
        public SQLDmlType OpType { get; set; }

        public string Schema { get; set; }
        public string Table { get; set; }
        public List<string> Fields { get; set; }

    }
}
