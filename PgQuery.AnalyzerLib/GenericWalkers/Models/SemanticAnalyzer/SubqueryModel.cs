using DataChangeAnalyzer.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQuery.AnalyzerLib.GenericWalkers.Models.SemanticAnalyzer
{
    public class SubqueryModel
    {
        public string Alias { get; set; }
        public Node SubqueryNode { get; set; }
        public TableModel Table { get; set; }
    }
}
