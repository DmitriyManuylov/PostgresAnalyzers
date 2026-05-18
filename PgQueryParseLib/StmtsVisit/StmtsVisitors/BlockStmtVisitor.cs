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
        internal static void VisitBlockStatement(PLpgSQL_stmt_block blockStmt, StmtsProcessingContext context)
        {
            if (blockStmt is null)
            {
                throw new ArgumentNullException();
            }

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessBlockStmt_DirectTraversal(node);

            foreach (var stmt in blockStmt.Body)
            {
                VisitStmt(stmt, context);
            }

            context.PgTreeWalker.ProcessBlockStmt_ReverseTraversal(node);

        }
    }
}
