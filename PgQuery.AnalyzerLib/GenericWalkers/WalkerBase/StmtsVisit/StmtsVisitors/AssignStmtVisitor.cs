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
        private void VisitAssignStatement(PLpgSQL_stmt_assign assignStmt, StmtsProcessingContext context)
        {
            ArgumentNullException.ThrowIfNull(assignStmt);

            SetStmtParseTree(assignStmt.Expr.PLpgSQLExpr);

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessAssignStmt_DirectTraversal(node);

            VisitExpr(assignStmt.Expr.PLpgSQLExpr.ParsedStmt.Stmt, context);

            context.PgTreeWalker.ProcessAssignStmt_ReverseTraversal(node);
        }
    }
}
