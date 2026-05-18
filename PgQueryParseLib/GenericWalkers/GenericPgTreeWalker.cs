using PgQueryAnalyzerLib.AnalyzeContext;
using PgQueryAnalyzerLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
[assembly: InternalsVisibleTo("IndexUsingAnalyzer")]
[assembly: InternalsVisibleTo("DMLOpsAnalyzer")]

namespace PgQueryAnalyzerLib.GenericWalkers
{

    public class GenericPgTreeWalker : GenericPgTreeWalkerBase
    {
        public GenericPgTreeWalker(StmtsProcessingContext context) : base(context)
        {

        }


        //public void VisitGenericNode(PgGenericNode node, StmtsProcessingContext context)
        //{
        //    switch (node.PgNodeDialectType)
        //    {
        //        case PgNodeDialectType.PgSql:
        //            node.PgSqlNode
        //            break;

        //        case PgNodeDialectType.PlPgSql:
        //            break;
        //    }
        //}


        private List<GenericPgTreeWalkerBase> PgTreeWalkerList { get; set; } = new List<GenericPgTreeWalkerBase>();

        //public override TAnalyzeResult GetResult()
        //{
        //    throw new NotImplementedException();
        //}

        public void AddWalker(GenericPgTreeWalkerBase walker)
        {
            walker.Context = this.Context;
            PgTreeWalkerList.Add(walker);
        }

        public bool IsWalkerListNotEmpty()
        {
            return PgTreeWalkerList.Any();
        }

        //public override void ProcessDirectTraversal(PgGenericNode node)
        //{
        //    foreach (var item in PgTreeWalkerList)
        //    {
        //        item.ProcessDirectTraversal(node);
        //    }
        //}

        //public override void ProcessReverseTraversal(PgGenericNode node)
        //{
        //    foreach (var item in PgTreeWalkerList)
        //    {
        //        item.ProcessReverseTraversal(node);
        //    }
        //}

        public TPgTreeWalker GetTreeWalkerByType<TPgTreeWalker>() where TPgTreeWalker : GenericPgTreeWalkerBase
        {
            if (this.PgTreeWalkerList is null || this.PgTreeWalkerList.Count < 1)
            {
                throw new Exception("Не задано ни одного обработчика дерева вызовов");
            }

            foreach (var walker in this.PgTreeWalkerList)
            {
                if (typeof(TPgTreeWalker).Equals(walker.GetType()))
                {
                    return (TPgTreeWalker)walker;
                }
            }

            throw new Exception("Не найден обработчик указанного типа");
        }

        #region Прямой проход


        internal override void ProcessSelectStmt_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessSelectStmt_DirectTraversal(node);
            }
        }

        internal override void ProcessUpdateStmt_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessUpdateStmt_DirectTraversal(node);
            }
        }

        internal override void ProcessInsertStmt_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessInsertStmt_DirectTraversal(node);
            }
        }

        internal override void ProcessDeleteStmt_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessDeleteStmt_DirectTraversal(node);
            }
        }

        internal override void ProcessJoinExpr_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessJoinExpr_DirectTraversal(node);
            }
        }

        internal override void ProcessResTarget_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessResTarget_DirectTraversal(node);
            }
        }

        internal override void ProcessCommonTableExpr_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessCommonTableExpr_DirectTraversal(node);
            }
        }

        internal override void ProcessFuncCall_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessFuncCall_DirectTraversal(node);
            }
        }

        internal override void ProcessColumnRef_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessColumnRef_DirectTraversal(node);
            }
        }

        internal override void ProcessAssignStmt_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessAssignStmt_DirectTraversal(node);
            }
        }

        internal override void ProcessBlockStmt_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessBlockStmt_DirectTraversal(node);
            }
        }

        internal override void ProcessExecSqlStmt_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessExecSqlStmt_DirectTraversal(node);
            }
        }

        internal override void ProcessForiStmt_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessForiStmt_DirectTraversal(node);
            }
        }

        internal override void ProcessForsStmt_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessForsStmt_DirectTraversal(node);
            }
        }

        internal override void ProcessIfStmt_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessIfStmt_DirectTraversal(node);
            }
        }

        internal override void ProcessPerformStmt_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessPerformStmt_DirectTraversal(node);
            }
        }

        internal override void ProcessRaiseStmt_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessRaiseStmt_DirectTraversal(node);
            }
        }

        internal override void ProcessReturnNextStmt_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessReturnNextStmt_DirectTraversal(node);
            }
        }

        internal override void ProcessReturnStmt_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessReturnStmt_DirectTraversal(node);
            }
        }

        internal override void ProcessWithClause_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessWithClause_DirectTraversal(node);
            }
        }

        internal override void ProcessAExpr_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessAExpr_DirectTraversal(node);
            }
        }

        internal override void ProcessString_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessString_DirectTraversal(node);
            }
        }

        internal override void ProcessBoolExpr_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessBoolExpr_DirectTraversal(node);
            }
        }

        internal override void ProcessSubLink_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessSubLink_DirectTraversal(node);
            }
        }

        internal override void ProcessCaseExpr_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessCaseExpr_DirectTraversal(node);
            }
        }

        internal override void ProcessCaseWhen_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessCaseWhen_DirectTraversal(node);
            }
        }

        internal override void ProcessRangeVar_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessRangeVar_DirectTraversal(node);
            }
        }

        internal override void ProcessRangeSubselect_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessRangeSubselect_DirectTraversal(node);
            }
        }

        internal override void ProcessRangeFunction_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessRangeFunction_DirectTraversal(node);
            }
        }

        internal override void ProcessCaseStmt_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessCaseStmt_DirectTraversal(node);
            }
        }

        #endregion

        #region Обратный проход

        internal override void ProcessSelectStmt_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessSelectStmt_ReverseTraversal(node);
            }
        }

        internal override void ProcessUpdateStmt_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessUpdateStmt_ReverseTraversal(node);
            }
        }

        internal override void ProcessInsertStmt_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessInsertStmt_ReverseTraversal(node);
            }
        }

        internal override void ProcessDeleteStmt_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessDeleteStmt_ReverseTraversal(node);
            }
        }

        internal override void ProcessJoinExpr_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessJoinExpr_ReverseTraversal(node);
            }
        }

        internal override void ProcessResTarget_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessResTarget_ReverseTraversal(node);
            }
        }

        internal override void ProcessCommonTableExpr_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessCommonTableExpr_ReverseTraversal(node);
            }
        }

        internal override void ProcessFuncCall_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessFuncCall_ReverseTraversal(node);
            }
        }

        internal override void ProcessColumnRef_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessColumnRef_ReverseTraversal(node);
            }
        }

        internal override void ProcessAssignStmt_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessAssignStmt_ReverseTraversal(node);
            }
        }

        internal override void ProcessBlockStmt_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessBlockStmt_ReverseTraversal(node);
            }
        }

        internal override void ProcessExecSqlStmt_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessExecSqlStmt_ReverseTraversal(node);
            }
        }

        internal override void ProcessForiStmt_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessForiStmt_ReverseTraversal(node);
            }
        }

        internal override void ProcessForsStmt_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessForsStmt_ReverseTraversal(node);
            }
        }

        internal override void ProcessIfStmt_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessIfStmt_ReverseTraversal(node);
            }
        }

        internal override void ProcessPerformStmt_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessPerformStmt_ReverseTraversal(node);
            }
        }

        internal override void ProcessRaiseStmt_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessRaiseStmt_ReverseTraversal(node);
            }
        }

        internal override void ProcessReturnNextStmt_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessReturnNextStmt_ReverseTraversal(node);
            }
        }

        internal override void ProcessReturnStmt_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessReturnStmt_ReverseTraversal(node);
            }
        }

        internal override void ProcessWithClause_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessWithClause_ReverseTraversal(node);
            }
        }

        internal override void ProcessAExpr_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessAExpr_DirectTraversal(node);
            }
        }

        internal override void ProcessString_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessString_ReverseTraversal(node);
            }
        }

        internal override void ProcessBoolExpr_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessBoolExpr_ReverseTraversal(node);
            }
        }

        internal override void ProcessSubLink_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessSubLink_ReverseTraversal(node);
            }
        }

        internal override void ProcessCaseExpr_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessCaseExpr_ReverseTraversal(node);
            }
        }

        internal override void ProcessCaseWhen_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessCaseWhen_ReverseTraversal(node);
            }
        }

        internal override void ProcessRangeVar_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessRangeVar_ReverseTraversal(node);
            }
        }

        internal override void ProcessRangeSubselect_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessRangeSubselect_ReverseTraversal(node);
            }
        }

        internal override void ProcessRangeFunction_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessRangeFunction_ReverseTraversal(node);
            }
        }

        internal override void ProcessCaseStmt_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessCaseStmt_ReverseTraversal(node);
            }
        }

        #endregion

        internal override void ProcessDirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessDirectTraversal(node);
            }
        }

        internal override void ProcessReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessReverseTraversal(node);
            }
        }
    }
}
