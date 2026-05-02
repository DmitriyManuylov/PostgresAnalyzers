using PgQuery;
using PgQueryParseLib.AnalyzeContext;
using PgQueryParseLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQueryParseLib.StmtsVisit.ExprsVisitors
{
    public static partial class ExprVisitor
    {
        private static PostgreSqlQueryParser _parser = new PostgreSqlQueryParser();

        public static void VisitExpr(Node expr, StmtsProcessingContext context)
        {
            if (expr is null)
            {
                throw new ArgumentNullException();
            }

            var node = new PgGenericNode()
            {
                PgSqlNode = expr
            };

            context.ProcessDirectTraversal(node);

            switch (expr.NodeCase)
            {
                case Node.NodeOneofCase.UpdateStmt:
                    VisitUpdateStmt(expr.UpdateStmt, context);
                    break;
                case Node.NodeOneofCase.InsertStmt:
                    VisitInsertStmt(expr.InsertStmt, context);
                    break;
                case Node.NodeOneofCase.SelectStmt:
                    VisitSelectStmt(expr.SelectStmt, context);
                    break;
                case Node.NodeOneofCase.DeleteStmt:
                    VisitDeleteStmt(expr.DeleteStmt, context);
                    break;
                case Node.NodeOneofCase.ResTarget:
                    VisitResTarget(expr.ResTarget, context);
                    break;
                case Node.NodeOneofCase.CommonTableExpr:
                    VisitCommonTableExpr(expr.CommonTableExpr, context);
                    break;
                case Node.NodeOneofCase.WithClause:
                    VisitWithClause(expr.WithClause, context);
                    break;
                case Node.NodeOneofCase.FuncCall:
                    VisitFuncCall(expr.FuncCall, context);
                    break;
                case Node.NodeOneofCase.ColumnRef:
                    VisitColumnRef(expr.ColumnRef, context);
                    break;
                case Node.NodeOneofCase.AExpr:
                    VisitAExpr(expr.AExpr, context);
                    break;
                case Node.NodeOneofCase.JoinExpr:
                    VisitJoinExpr(expr.JoinExpr, context);
                    break;
                case Node.NodeOneofCase.BoolExpr:
                    VisitBoolExpr(expr.BoolExpr, context);
                    break;
                case Node.NodeOneofCase.SubLink:
                    VisitSubLink(expr.SubLink, context);
                    break;
                case Node.NodeOneofCase.CaseExpr:
                    VisitCaseExpr(expr.CaseExpr, context);
                    break;
                case Node.NodeOneofCase.CaseWhen:
                    VisitCaseWhen(expr.CaseWhen, context);
                    break;
                case Node.NodeOneofCase.RangeVar:
                    VisitRangeVar(expr.RangeVar, context);
                    break;
                case Node.NodeOneofCase.RangeSubselect:
                    VisitRangeSubselect(expr.RangeSubselect, context);
                    break;
                case Node.NodeOneofCase.RangeFunction:
                    VisitRangeFunction(expr.RangeFunction, context);
                    break;
                default:
                    break;
            }

            context.ProcessReverseTraversal(node);
        }
    }
}
