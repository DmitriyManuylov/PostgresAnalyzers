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

            int funNameSegmentsCount = funcCall.Funcname.Count;

            string nspName = funNameSegmentsCount == 2 ? funcCall.Funcname[0].String.Sval : null;
            string funcName = funNameSegmentsCount == 2 ? funcCall.Funcname[1].String.Sval :
                funNameSegmentsCount == 1 ? funcCall.Funcname[0].String.Sval : null;

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
                PLpgSQL_stmt funcDef = default;

                bool isExistsFuncCallCycle = funcCall.Funcname.Count == 2 ? context.CheckExistsFuncCallCycle(nspName, funcName) : false;

                if (funcCall.Funcname.Count == 2 && !isExistsFuncCallCycle && funcCall.Funcname[0].String.Sval != "pg_catalog")
                {
                    funcDef = context.GetDBFunctionPlainModel(funcCall.Funcname[0].String.Sval, funcCall.Funcname[1].String.Sval).ParsedStmt;
                }

                if (funcDef is not null && !context.IgnoreFuncCalls)
                {
                    var stmt = new PLpgSQL_stmt
                    {
                        PLpgSQLStmtBlock = funcDef.PLpgSQLFunction.Action.PLpgSQLStmtBlock
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
