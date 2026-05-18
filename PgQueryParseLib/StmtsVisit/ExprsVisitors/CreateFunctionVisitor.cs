using PgQuery;
using PgQueryAnalyzerLib.AnalyzeContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQueryAnalyzerLib.StmtsVisit.ExprsVisitors
{
    public static partial class ExprVisitor
    {
        private static void VisitCreateFunction(CreateFunctionStmt createFunctionStmt, StmtsProcessingContext context)
        {
            if (createFunctionStmt == null)
            {
                throw new ArgumentNullException();
            }

            var node = context.PgGenericNodes.Peek();

            string funcName = string.Join(".", createFunctionStmt.Funcname.Select(item => item.String.Sval));

            string returnType = string.Join(".", createFunctionStmt.ReturnType.Names.Select(item => item.String.Sval));

            foreach (var option in createFunctionStmt.Options)
            {
                ExprVisitor.VisitExpr(option, context);
            }
        }
    }
}
