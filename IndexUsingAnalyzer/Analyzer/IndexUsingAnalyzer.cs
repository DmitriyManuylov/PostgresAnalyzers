using IndexUsingAnalyzer.Models;
using IndexUsingAnalyzer.Models.InnerModels;
using PgQuery;
using PgQueryAnalyzerLib.Models;
using PgQueryAnalyzerLib.AnalyzeContext;
using PgQueryAnalyzerLib.GenericWalkers;
using PgQueryAnalyzerLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndexUsingAnalyzer.Analyzer
{
    public class IndexUsingAnalyzer : RisingCumulativeAnalyzerBase<IndexUsingAnalyzeNode>
    {
        private List<Node.NodeOneofCase> range = new List<Node.NodeOneofCase> { Node.NodeOneofCase.RangeVar, Node.NodeOneofCase.RangeSubselect };
        private Stack<PgGenericNode> _joinStack = new Stack<PgGenericNode>();

        private InnerIndexUsingModel _innerIndexUsingModel;
        private HashSet<Node> _visitedNodes = new();

        private List<List<TableInnerModel>> QueryMultiLevelContext = new List<List<TableInnerModel>>();

        public IndexUsingAnalyzer(StmtsProcessingContext context) : base(context)
        {

        }

        internal override void ProcessJoinExpr_DirectTraversal(PgGenericNode node)
        {
            base.ProcessJoinExpr_DirectTraversal(node);

            var stmt = node.PgSqlNode.JoinExpr;

            var analyzeNodeModel = CurrentNode.AnalyzeNodeModel;

            //analyzeNodeModel.AnalyzeModel.Add

            _joinStack.Push(node);

            _innerIndexUsingModel.AddJoinToLastNode(node);

            var larg = stmt.Larg;
            var genericLeftNode = new PgGenericNode
            {
                PgSqlNode = larg,
            };
            var genericRightNode = new
            {
                PgSqlNode = stmt.Rarg,
            };

            switch (stmt.Larg.NodeCase)
            {
                case Node.NodeOneofCase.RangeVar:
                    AddRangeVar(stmt.Larg.RangeVar);
                    break;
                case Node.NodeOneofCase.JoinExpr:
                    ProcessJoinExpr_DirectTraversal(genericLeftNode);
                    break;
                case Node.NodeOneofCase.RangeSubselect:
                    ProcessSelectStmt_DirectTraversal(genericLeftNode);
                    break;
                case Node.NodeOneofCase.RangeFunction:
                    ProcessRangeFunction_DirectTraversal(genericLeftNode);
                    break;
                default:
                    break;
            }

            switch (stmt.Rarg.NodeCase)
            {
                case Node.NodeOneofCase.RangeVar:
                    AddRangeVar(stmt.Larg.RangeVar);
                    break;
                case Node.NodeOneofCase.JoinExpr:
                    ProcessJoinExpr_DirectTraversal(genericLeftNode);
                    break;
                case Node.NodeOneofCase.RangeSubselect:
                    ProcessSelectStmt_DirectTraversal(genericLeftNode);
                    break;
                case Node.NodeOneofCase.RangeFunction:
                    ProcessRangeFunction_DirectTraversal(genericLeftNode);
                    break;
                default:
                    break;
            }

            if (stmt.Larg.NodeCase == Node.NodeOneofCase.RangeVar && stmt.Rarg.NodeCase == Node.NodeOneofCase.RangeVar)
            {
                var leftCol = stmt.Larg.RangeVar;
                var rightCol = stmt.Rarg.RangeVar;



                var leftTable = Context.GetDBTableModel(leftCol.Schemaname, leftCol.Relname);

                var rightTable = Context.GetDBTableModel(rightCol.Schemaname, rightCol.Relname);

                ProcessJoinQuals(stmt.Quals);
                // var lexpr = stmt.Quals.
            }
                

            if(stmt.Quals?.AExpr?.Lexpr is not null && stmt.Quals?.AExpr?.Lexpr is not null)
            {

            }
        }

        private void AddRangeVar(RangeVar rangeVar)
        {
            QueryMultiLevelContext.Last().Add(new TableInnerModel
            {
                TableNsp = rangeVar.Schemaname,
                TableName = rangeVar.Relname,
                TableAlias = rangeVar.Alias.Aliasname,
                Type = TableType.Table
            });

            //_innerIndexUsingModel.JoinsParentStackHead.TablesFrom.Add(new TableInnerModel()
            //{
            //    TableNsp = rangeVar.Schemaname,
            //    TableName = rangeVar.Relname,
            //    TableAlias = rangeVar.Alias.Aliasname,
            //    Type = TableType.Table
            //});
        }

        private void AddRangeSubselect(RangeSubselect rangeSubselect)
        {
            //_innerIndexUsingModel.JoinsParentStackHead.TablesFrom.Add(new TableInnerModel()
            //{
            //    TableNsp = rangeSubselect.,
            //    TableName = rangeSubselect.Relname,
            //    TableAlias = rangeSubselect.Alias.Aliasname,
            //    Type = TableType.Table
            //});
        }

        internal override void ProcessJoinExpr_ReverseTraversal(PgGenericNode node)
        {
            base.ProcessJoinExpr_ReverseTraversal(node);

            var stmt = node.PgSqlNode.JoinExpr;
            //node.PgSqlNode.SubSe

            _joinStack.Pop();
        }


        internal override void ProcessSelectStmt_DirectTraversal(PgGenericNode node)
        {
            base.ProcessSelectStmt_DirectTraversal(node);

            QueryMultiLevelContext.Add(new List<TableInnerModel>());

            var stmt = node.PgSqlNode.SelectStmt;

            //_innerIndexUsingModel.AddNode(node);

        }

        internal override void ProcessSelectStmt_ReverseTraversal(PgGenericNode node)
        {
            base.ProcessSelectStmt_ReverseTraversal(node);

            QueryMultiLevelContext.RemoveAt(QueryMultiLevelContext.Count - 1);

            var stmt = node.PgSqlNode.SelectStmt;

            _joinStack.Pop();
        }

        internal override void ProcessUpdateStmt_DirectTraversal(PgGenericNode node)
        {
            base.ProcessUpdateStmt_DirectTraversal(node);

            QueryMultiLevelContext.Add(new List<TableInnerModel>());


            //_innerIndexUsingModel.AddNode(node);

            //var stmt = node.PgSqlNode.UpdateStmt;

            //_joinStack.Push(node);
        }

        internal override void ProcessUpdateStmt_ReverseTraversal(PgGenericNode node)
        {
            base.ProcessUpdateStmt_ReverseTraversal(node);

            QueryMultiLevelContext.RemoveAt(QueryMultiLevelContext.Count - 1);

            var stmt = node.PgSqlNode.UpdateStmt;
        }

        internal override void ProcessDeleteStmt_DirectTraversal(PgGenericNode node)
        {
            base.ProcessUpdateStmt_DirectTraversal(node);

            var stmt = node.PgSqlNode.UpdateStmt;
        }

        internal override void ProcessDeleteStmt_ReverseTraversal(PgGenericNode node)
        {
            base.ProcessUpdateStmt_ReverseTraversal(node);

            var stmt = node.PgSqlNode.UpdateStmt;
        }

        internal override void ProcessRangeSubselect_DirectTraversal(PgGenericNode node)
        {
            base.ProcessRangeSubselect_DirectTraversal(node);
        }

        internal override void ProcessRangeSubselect_ReverseTraversal(PgGenericNode node)
        {
            base.ProcessRangeSubselect_ReverseTraversal(node);
        }

        private void AnalyzeJoinChain(JoinExpr joinExpr)
        {

        }

        private void ProcessJoinQuals(Node node)
        {
            switch(node.NodeCase)
            {
                case Node.NodeOneofCase.BoolExpr:
                    VisitBoolExpr(node.BoolExpr);
                    break;
                case Node.NodeOneofCase.AExpr:
                    VisitAExpr(node.AExpr);
                    break;
                case Node.NodeOneofCase.ResTarget:
                    VisitResTarget(node.ResTarget);
                    break;
                case Node.NodeOneofCase.TypeCast:
                    VisitTypeCast(node.TypeCast);
                    break;
                case Node.NodeOneofCase.CaseExpr:
                    VisitCaseExpr(node.CaseExpr);
                    break;
                case Node.NodeOneofCase.CaseWhen:
                    VisitCaseWhen(node.CaseWhen);
                    break;
            }
        }

        private void VisitBoolExpr(BoolExpr boolExpr)
        {
            foreach(var arg in boolExpr.Args)
            {
                ProcessJoinQuals(arg);
            }

        }

        private void VisitAExpr(A_Expr a_Expr)
        {
            var left = a_Expr.Lexpr;
            var right = a_Expr.Rexpr;

            ProcessJoinQuals(left);
            ProcessJoinQuals(right);
        }

        private void VisitResTarget(ResTarget resTarget)
        {

        }

        private void VisitTypeCast(TypeCast typeCast)
        {
            ProcessJoinQuals(typeCast.Arg);

            var tn = typeCast.TypeName;
        }

        private void VisitCaseExpr(CaseExpr caseExpr)
        {
            foreach (var arg in caseExpr.Args)
            {
                ProcessJoinQuals(arg);
            }

            if (caseExpr.Defresult is not null)
            {
                ProcessJoinQuals(caseExpr.Defresult);
            }
        }

        private void VisitCaseWhen(CaseWhen caseWhen)
        {
            ProcessJoinQuals(caseWhen.Expr);

            ProcessJoinQuals(caseWhen.Result);
        }
    }
}
