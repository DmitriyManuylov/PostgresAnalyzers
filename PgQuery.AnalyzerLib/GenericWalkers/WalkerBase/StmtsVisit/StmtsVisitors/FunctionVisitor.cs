using PgQuery;
using PgQueryAnalyzerLib.AnalyzeContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQuery.AnalyzerLib.GenericWalkers.WalkerBase
{
    public abstract partial class GenericPgTreeWalkerBase
    {
        private void VisitPLpgSQLFunction(PLpgSQL_function pLpgSQL_Function, StmtsProcessingContext context)
        {
            ArgumentNullException.ThrowIfNull(pLpgSQL_Function);

            foreach(var stmt in pLpgSQL_Function.Action.PLpgSQLStmtBlock.Body)
            {
                VisitStmt(stmt, context);
            }
        }
    }
}
