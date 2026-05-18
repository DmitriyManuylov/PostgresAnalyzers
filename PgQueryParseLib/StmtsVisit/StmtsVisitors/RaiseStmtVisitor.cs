using PgQuery;
using PgQueryAnalyzerLib.AnalyzeContext;
using PgQueryAnalyzerLib.StmtsVisit.ExprsVisitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQueryAnalyzerLib.StmtsVisit.StmtsVisitors
{
    public static partial class StmtVisitor
    {
        private static void VisitRaiseStatement(PLpgSQL_stmt_raise raiseStmt, StmtsProcessingContext context)
        {
            if (raiseStmt is null)
            {
                throw new ArgumentNullException();
            }

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessRaiseStmt_DirectTraversal(node);

            foreach(var item in raiseStmt.Params)
            {
                SetStmtParseTree(item.PLpgSQLExpr);

                ExprVisitor.VisitExpr(item.PLpgSQLExpr.ParsedStmt.Stmt, context);
            }

            context.PgTreeWalker.ProcessRaiseStmt_ReverseTraversal(node);
        }
    }
}
