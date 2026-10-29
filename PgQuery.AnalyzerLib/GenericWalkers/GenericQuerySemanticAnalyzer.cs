using Dapper;
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

        private TableModel LastSubqueryTable { get; set; }

        public GenericQuerySemanticAnalyzer() : base() 
        {
            
        }

        public GenericQuerySemanticAnalyzer(StmtsProcessingContext context) : base(context)
        {

        }

        protected override void ProcessSelectStmt_DirectTraversal(PgGenericNode node)
        {
            var scope = new QueryScope(node);

            ScopeStack.Push(scope);

            base.ProcessSelectStmt_DirectTraversal(node);
        }

        protected override void ProcessSelectStmt_ReverseTraversal(PgGenericNode node)
        {
            var scope = ScopeStack.Pop() as QueryScope;
            var parentScope = ScopeStack.Peek() as QueryScope;

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

            var table = new TableModel
            {
                Columns = new List<Services.Models.DbModels.ColumnModel>()
            };

            var colrefs = selectStmt
                .TargetList
                .Where(item => item.ResTarget?.Val?.ColumnRef is not null);
                
            var resTargets = colrefs;

            foreach (var resTarget in selectStmt.TargetList.Select(item => item.ResTarget))
            {
                switch (resTarget.Val.NodeCase)
                {
                    case Node.NodeOneofCase.ColumnRef:
                        var colref = resTarget.Val.ColumnRef;

                        var firstSegment = colref.Fields.First();
                        if (firstSegment.NodeCase == Node.NodeOneofCase.AStar)
                        {
                            table.Columns.AddRange(scope.ScopeTablesList.SelectMany(tb => tb.Table.Columns));

                            continue;
                        }

                        string tableName = colref.Fields.First().String.Sval;

                        switch (colref.Fields[1].NodeCase)
                        {
                            case Node.NodeOneofCase.AStar:
                                table.Columns.AddRange(scope.ScopeTablesList.FirstOrDefault(tb => (tb.Alias ?? tb.Table.Name) == tableName).Table.Columns);
                                break;

                            case Node.NodeOneofCase.String:
                                table.Columns.Add(new Services.Models.DbModels.ColumnModel()
                                {
                                    ColumnName = resTarget.Name ?? colref.Fields[1].String.Sval
                                });
                                break;
                        }
                        break;

                    default:
                        table.Columns.Add(new Services.Models.DbModels.ColumnModel
                        {
                            ColumnName = resTarget.Name,
                        });

                        break;
                }
            }

            parentScope?.SubqueryTables.Add(new SubqueryModel
            {
                Alias = null,
                SubqueryNode = node.PgSqlNode,
                ParentNode = node.Parent.PgSqlNode,
                Table = table,
            });

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

            var currentNode = this.Context.PgGenericNodes.Peek();

            try
            {
                SubqueryModel subqueryModel = currentScope.SubqueryTables.FirstOrDefault(item => item.ParentNode.CommonTableExpr == cte);
                subqueryModel.Table.Name = cte.Ctename;
            }
            catch
            {

            }
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

                tableModel ??= currentScope.SubqueryTables.FirstOrDefault(item => item.Table.Name == rangeVar.Relname)?.Table;

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

            try
            {
                SubqueryModel subqueryModel = currentScope.SubqueryTables.FirstOrDefault(item => item.ParentNode.RangeSubselect == rangeSubselect);

                subqueryModel.Table.Name = rangeSubselect?.Alias?.Aliasname;

                var item = (rangeSubselect.Alias?.Aliasname, subqueryModel.Table);

                currentScope.ScopeTablesList.Add(item);
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
                var funcCall = rangeFunction.Functions
                .FirstOrDefault()?.List.Items.FirstOrDefault()?.FuncCall;

                var funcNameList = funcCall.Funcname;

                string funcNsp = null;
                string funcName = null;

                switch (funcNameList.Count)
                {
                    case 1:
                        funcName = funcNameList.First().String.Sval;
                        break;
                    case 2:
                        funcNsp = funcNameList.First().String.Sval;
                        funcName = funcNameList.Last().String.Sval;
                        break;
                }

                var function = Context.DBFunctionList.FirstOrDefault(item => item.NspName == funcNsp && item.FuncName == funcName);

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
