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
            if(funcCall is null)
            {
                throw new ArgumentNullException();
            }

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessFuncCall_DirectTraversal(node);

            foreach (var arg in funcCall.Args)
            {
                VisitExpr(arg, context);
            }

            var funcDef = context.GetDBFunctionPlainModel(funcCall.Funcname[0].String.Sval, funcCall.Funcname[1].String.Sval).ParsedStmt;

            var stmt = new PLpgSQL_stmt
            {
                PLpgSQLStmtBlock = funcDef!.Action.PLpgSQLStmtBlock
            };

            StmtVisitor.VisitStmt(stmt, context);

            context.PgTreeWalker.ProcessFuncCall_ReverseTraversal(node);
        }
    }
}
