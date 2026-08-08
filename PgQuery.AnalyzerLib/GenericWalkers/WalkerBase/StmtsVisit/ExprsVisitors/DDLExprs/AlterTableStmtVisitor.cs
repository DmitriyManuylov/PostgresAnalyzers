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
        private void VisitAlterTableStmt(AlterTableStmt alterTableStmt, StmtsProcessingContext context)
        {
            ArgumentNullException.ThrowIfNull(alterTableStmt);

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessAlterTableStmt_DirectTraversal(node);

            foreach(var smd in alterTableStmt.Cmds)
            {
                VisitExpr(smd, context);
            }

            context.PgTreeWalker.ProcessAlterTableStmt_ReverseTraversal(node);
        }
    }
}
