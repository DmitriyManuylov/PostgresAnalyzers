using PgQuery;
using PgQueryAnalyzerLib.AnalyzeContext;
using PgQueryAnalyzerLib.Models;
using PgQueryParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQueryAnalyzerLib.StmtsVisit.ExprsVisitors
{
    public static partial class ExprVisitor
    {
        private static PostgreSqlQueryParser _parser = new PostgreSqlQueryParser();

        public static void VisitExpr(Node expr, StmtsProcessingContext context)
        {
            ArgumentNullException.ThrowIfNull(expr);

            var node = new PgGenericNode()
            {
                PgSqlNode = expr
            };

            context.ProcessDirectTraversal(node);

            switch (expr.NodeCase)
            {
                case Node.NodeOneofCase.CreateFunctionStmt:
                    VisitCreateFunction(expr.CreateFunctionStmt, context);
                    break;
                case Node.NodeOneofCase.DefElem:
                    VisitDefElem(expr.DefElem, context);
                    break;
                case Node.NodeOneofCase.UpdateStmt:
                    VisitUpdateStmt(expr.UpdateStmt, context);
                    break;
                case Node.NodeOneofCase.InsertStmt:
                    VisitInsertStmt(expr.InsertStmt, context);
                    break;
                case Node.NodeOneofCase.SelectStmt:
                    VisitSelectStmt(expr.SelectStmt, context);
                    break;
                case Node.NodeOneofCase.List:
                    VisitList(expr.List, context);
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
                case Node.NodeOneofCase.AlterTableStmt:
                    VisitAlterTableStmt(expr.AlterTableStmt, context);
                    break;
                case Node.NodeOneofCase.AlterTableCmd:
                    VisitAlterTableCmd(expr.AlterTableCmd, context);
                    break;
                case Node.NodeOneofCase.TypeCast:
                    VisitTypeCast(expr.TypeCast, context);
                    break;
                case Node.NodeOneofCase.DropStmt:
                    VisitDropStmt(expr.DropStmt, context);
                    break;
                case Node.NodeOneofCase.RenameStmt:
                    VisitRenameStmt(expr.RenameStmt, context);
                    break;
                case Node.NodeOneofCase.MultiAssignRef:
                    VisitMultiAssignRef(expr.MultiAssignRef, context);
                    break;
                case Node.NodeOneofCase.RowExpr:
                    VisitRowExpr(expr.RowExpr, context);
                    break;
                default:
                    break;
            }

            context.ProcessReverseTraversal(node);
        }
    }
}
