using PgQuery;
using PgQueryParseLib.AnalyzeContext;
using PgQueryParseLib.StmtsVisit.StmtsVisitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQueryParseLib.StmtsVisit.ExprsVisitors
{
    public static partial class ExprVisitor
    {
        public static void VisitFuncCall(FuncCall funcCall, StmtsProcessingContext context)
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

            StmtVisitor.VisitBlockStatement(funcDef!.Action.PLpgSQLStmtBlock, context);

            context.PgTreeWalker.ProcessFuncCall_ReverseTraversal(node);
        }
    }
}
