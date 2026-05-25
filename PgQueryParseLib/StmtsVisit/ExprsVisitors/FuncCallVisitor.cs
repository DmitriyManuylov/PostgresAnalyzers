using PgQuery;
using PgQueryAnalyzerLib.AnalyzeContext;
using PgQueryAnalyzerLib.StmtsVisit.StmtsVisitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQueryAnalyzerLib.StmtsVisit.ExprsVisitors
{
    public static partial class ExprVisitor
    {
        private static void VisitFuncCall(FuncCall funcCall, StmtsProcessingContext context)
        {
            ArgumentNullException.ThrowIfNull(funcCall);

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessFuncCall_DirectTraversal(node);

            if (funcCall.Args is not null)
            {
                foreach (var arg in funcCall.Args)
                {
                    VisitExpr(arg, context);
                }
            }

            try
            {
                var funcDef = context.GetDBFunctionPlainModel(funcCall.Funcname[0].String.Sval, funcCall.Funcname[1].String.Sval).ParsedStmt;

                if (funcDef is not null)
                {
                    var stmt = new PLpgSQL_stmt
                    {
                        PLpgSQLStmtBlock = funcDef!.Action.PLpgSQLStmtBlock
                    };

                    StmtVisitor.VisitStmt(stmt, context);
                }
            }
            catch
            {

            }

            context.PgTreeWalker.ProcessFuncCall_ReverseTraversal(node);
        }
    }
}
