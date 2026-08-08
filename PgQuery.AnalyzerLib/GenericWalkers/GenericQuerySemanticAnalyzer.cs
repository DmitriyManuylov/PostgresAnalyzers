using DataChangeAnalyzer.Models.DBModels;
using PgQuery.AnalyzerLib.GenericWalkers.Models.SemanticAnalyzer;
using PgQuery.AnalyzerLib.GenericWalkers.WalkerBase;
using PgQueryAnalyzerLib.AnalyzeContext;
using PgQueryAnalyzerLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQuery.AnalyzerLib.GenericWalkers
{
    public abstract class GenericQuerySemanticAnalyzer : GenericPgTreeWalkerBase
    {
        public Stack<Scope> ScopeStack = new Stack<Scope>();

        public Scope CurrentScope
        {
            get
            {
                return ScopeStack.Peek();
            }
        }

        public GenericQuerySemanticAnalyzer() : base() 
        {
            
        }

        public GenericQuerySemanticAnalyzer(StmtsProcessingContext context) : base(context)
        {

        }

        protected override void ProcessSelectStmt_DirectTraversal(PgGenericNode node)
        {
            base.ProcessSelectStmt_DirectTraversal(node);

            var scope = new QueryScope(node);
            ScopeStack.Push(scope);

            var selectStmt = node.PgSqlNode.SelectStmt;

            if (selectStmt.WithClause is not null)
            {
                foreach (var cte in selectStmt.WithClause.Ctes)
                {
                    ProcessCTE(cte.CommonTableExpr);
                }
            }

            if (selectStmt.FromClause is not null)
            {
                foreach (var from in selectStmt.FromClause)
                {
                    switch (from.NodeCase)
                    {
                        case Node.NodeOneofCase.RangeVar:
                            ProcessRangeVar(from.RangeVar);
                            break;

                        case Node.NodeOneofCase.JoinExpr:
                            ProcessJoinExpr(from.JoinExpr);
                            break;
                        default:
                            continue;
                    }
                }

            }

        }

        protected override void ProcessSelectStmt_ReverseTraversal(PgGenericNode node)
        {
            var scope = ScopeStack.Pop() as QueryScope;

            base.ProcessSelectStmt_ReverseTraversal(node);
        }

        protected override void ProcessUpdateStmt_DirectTraversal(PgGenericNode node)
        {
            base.ProcessUpdateStmt_DirectTraversal(node);

            var scope = new QueryScope(node);

            ScopeStack.Push(scope);

            var updateStmt = node.PgSqlNode.UpdateStmt;

            ProcessRangeVar(updateStmt.Relation);
        }

        protected override void ProcessUpdateStmt_ReverseTraversal(PgGenericNode node)
        {
            var scope = ScopeStack.Pop() as QueryScope;

            base.ProcessUpdateStmt_ReverseTraversal(node);
        }

        protected override void ProcessDeleteStmt_DirectTraversal(PgGenericNode node)
        {
            base.ProcessDeleteStmt_DirectTraversal(node);

            var scope = new QueryScope(node);
            ScopeStack.Push(scope);

            var deleteStmt = node.PgSqlNode.DeleteStmt;

            ProcessRangeVar(deleteStmt.Relation);
        }

        protected override void ProcessDeleteStmt_ReverseTraversal(PgGenericNode node)
        {
            var scope = ScopeStack.Pop() as QueryScope;

            base.ProcessDeleteStmt_ReverseTraversal(node);
        }

        protected override void ProcessFuncCall_DirectTraversal(PgGenericNode node)
        {
            base.ProcessFuncCall_DirectTraversal(node);

            //var scope = new QueryScope(node);
            //ScopeStack.Push(scope);
        }

        protected override void ProcessFuncCall_ReverseTraversal(PgGenericNode node)
        {
            //var scope = ScopeStack.Pop();

            base.ProcessFuncCall_ReverseTraversal(node);
        }

        protected override void ProcessRangeSubselect_ReverseTraversal(PgGenericNode node)
        {
            var parentScope = this.ScopeStack.ToList()[this.ScopeStack.Count - 2];

            var expr = node.PgSqlNode.RangeSubselect;

            base.ProcessRangeSubselect_ReverseTraversal(node);
        }

        protected TableModel SearchTableInScopeStack(string alias, string columnName)
        {
            TableModel result = default;
            foreach(var scope in ScopeStack)
            {
                var queryScope = scope as QueryScope;

                if (queryScope is null)
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(alias))
                {
                    result = queryScope.ScopeTablesList.FirstOrDefault(item => item.Table.Columns.Any(col => col.ColumnName == columnName)).Table;
                }
                else
                {
                    result = queryScope?.ScopeTablesList.FirstOrDefault(item => item.Alias == alias && item.Table.Columns.Any(col => col.ColumnName == columnName)).Table;
                }

                if(result is not null)
                {
                    break;
                }
            }

            return result;
        }

        private void ProcessCTE(CommonTableExpr cte)
        {
            var currentScope = this.CurrentScope as QueryScope;


        }

        private void ProcessJoinExpr(JoinExpr joinExpr)
        {
            var larg = joinExpr.Larg;
            var rarg = joinExpr.Rarg;

            switch (larg.NodeCase)
            {
                case Node.NodeOneofCase.JoinExpr:
                    ProcessJoinExpr(larg.JoinExpr);
                    break;
                case Node.NodeOneofCase.RangeVar:
                    ProcessRangeVar(larg.RangeVar);
                    break;
                case Node.NodeOneofCase.RangeSubselect:
                    ProcessRangeSubselect(larg.RangeSubselect);
                    break;
                case Node.NodeOneofCase.RangeFunction:
                    ProcessRangeFunction(larg.RangeFunction);
                    break;
            }

            switch (rarg.NodeCase)
            {
                case Node.NodeOneofCase.JoinExpr:
                    ProcessJoinExpr(rarg.JoinExpr);
                    break;
                case Node.NodeOneofCase.RangeVar:
                    ProcessRangeVar(rarg.RangeVar);
                    break;
                case Node.NodeOneofCase.RangeSubselect:
                    ProcessRangeSubselect(rarg.RangeSubselect);
                    break;
                case Node.NodeOneofCase.RangeFunction:
                    ProcessRangeFunction(rarg.RangeFunction);
                    break;
            }
        }

        private void ProcessRangeVar(RangeVar rangeVar)
        {
            var currentScope = this.CurrentScope as QueryScope;

            TableModel tableModel;

            try
            {
                tableModel = Context.GetDBTableModel(rangeVar.Schemaname, rangeVar.Relname);

                var item = (rangeVar.Alias?.Aliasname, tableModel);

                currentScope.ScopeTablesList.Add(item);
            }
            catch
            {

            }

            
        }

        private void ProcessRangeSubselect(RangeSubselect rangeSubselect)
        {
            var currentScope = this.CurrentScope as QueryScope;

            var currentNode = this.Context.PgGenericNodes.Peek();

            try
            {
                SubqueryModel subqueryModel = new SubqueryModel
                {
                    Alias = rangeSubselect.Alias.Aliasname,
                    SubqueryNode = currentNode.PgSqlNode
                };

                currentScope.SubqueryTables.Add(subqueryModel);
            }
            catch
            {

            }
        }

        private void ProcessRangeFunction(RangeFunction rangeFunction)
        {
            var currentScope = this.CurrentScope as QueryScope;

            var currentNode = this.Context.PgGenericNodes.Peek();

            TableModel tableModel;

            try
            {
                var item = (rangeFunction.Alias.Aliasname, null as TableModel);

                SubqueryModel subqueryModel = new SubqueryModel
                {
                    Alias = rangeFunction.Alias.Aliasname,
                    SubqueryNode = currentNode.PgSqlNode
                };

                currentScope.SubqueryTables.Add(subqueryModel);
            }
            catch
            {

            }
        }
    }
}
