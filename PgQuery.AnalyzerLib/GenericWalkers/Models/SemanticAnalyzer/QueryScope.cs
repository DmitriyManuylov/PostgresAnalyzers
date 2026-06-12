using DataChangeAnalyzer.Models.DBModels;
using PgQueryAnalyzerLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQuery.AnalyzerLib.GenericWalkers.Models.SemanticAnalyzer
{
    public class QueryScope : Scope
    {
        public List<(string Alias, TableModel Table)> ScopeTablesList;

        public List<SubqueryModel> SubqueryTables;

        public QueryScope(PgGenericNode node): base(node)
        {
            ScopeTablesList = new List<(string Alias, TableModel Table)>();
            SubqueryTables = new List<SubqueryModel>();
        }
    }
}
