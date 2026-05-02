using PgQuery;
using PgQueryParseLib.AnalyzeContext;
using PgQueryParseLib.CustomExceptions;
using PgQueryParseLib.StmtsVisit.ExprsVisitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQueryParseLib.StmtsVisit.StmtsVisitors
{
    public static partial class StmtVisitor
    {
        public static void VisitIfStatement(PLpgSQL_stmt_if ifStmt, StmtsProcessingContext context)
        {
            ParseResult conditionParseTree;


            if (ifStmt is null)
            {
                throw new ArgumentNullException();
            }

            PLpgSQL_expr condition = ifStmt.Cond.PLpgSQLExpr;
            var thenBody = ifStmt.ThenBody;

            var elsif_list = ifStmt.ElsifList;

            var else_block = ifStmt.ElseBody;

            if (ifStmt.Cond is null
                || condition is null
                || string.IsNullOrWhiteSpace(condition.Query))
            {
                throw new PlStmtParseException("Не задано условие ветвления");
            }

            if (thenBody is null || thenBody.Count < 1)
            {
                throw new PlStmtParseException("Не задан блок Then");
            }

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessIfStmt_DirectTraversal(node);

            try
            {
                SetStmtParseTree(condition);

                ExprVisitor.VisitExpr(condition.ParsedStmt.Stmt, context);
            }
            catch (ParseQueryException e)
            {
                throw new PlStmtParseException("Ошибка разбора выражения условия ветвления", e);
            }

            try
            {
                foreach (var thenStmt in thenBody)
                {
                    VisitStmt(thenStmt, context);
                }

                if (elsif_list is not null)
                {
                    foreach (var elsif_stmt in elsif_list)
                    {
                        var elsif = elsif_stmt.PLpgSQLIfElsif;

                        foreach(var stmt in elsif.Stmts)
                        {
                            VisitStmt(stmt, context);
                        }
                    }
                }

                if (else_block is not null)
                {
                    foreach(var stmt in else_block)
                    {
                        VisitStmt(stmt, context);
                    }
                }

            }
            catch (Exception e)
            {

            }

            context.PgTreeWalker.ProcessIfStmt_ReverseTraversal(node);
        }

    }
}
