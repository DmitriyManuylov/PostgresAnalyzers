using PgQuery;
using PgQueryAnalyzerLib.AnalyzeContext;
using PgQueryAnalyzerLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQuery.AnalyzerLib.GenericWalkers.WalkerBase
{
    public abstract partial class GenericPgTreeWalkerBase
    {
        private void VisitForsStatement(PLpgSQL_stmt_fors forsStmt, StmtsProcessingContext context)
        {
            ArgumentNullException.ThrowIfNull(forsStmt);

            SetStmtParseTree(forsStmt.Query.PLpgSQLExpr);

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessForsStmt_DirectTraversal(node);

            VisitExpr(forsStmt.Query.PLpgSQLExpr.ParsedStmt.Stmt, context);

            foreach(PLpgSQL_stmt stmt in forsStmt.Body)
            {
                VisitStmt(stmt, context);
            }

            context.PgTreeWalker.ProcessForsStmt_ReverseTraversal(node);
        }
    }
}
