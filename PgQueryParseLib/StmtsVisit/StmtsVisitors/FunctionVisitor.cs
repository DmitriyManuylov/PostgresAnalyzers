using PgQuery;
using PgQueryAnalyzerLib.AnalyzeContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQueryAnalyzerLib.StmtsVisit.StmtsVisitors
{
    public static partial class StmtVisitor
    {
        private static void VisitPLpgSQLFunction(PLpgSQL_function pLpgSQL_Function, StmtsProcessingContext context)
        {
            foreach(var stmt in pLpgSQL_Function.Action.PLpgSQLStmtBlock.Body)
            {
                VisitStmt(stmt, context);
            }
        }
    }
}
