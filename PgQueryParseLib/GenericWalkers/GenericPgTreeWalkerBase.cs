using PgQueryAnalyzerLib.AnalyzeContext;
using PgQueryAnalyzerLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQueryAnalyzerLib.GenericWalkers
{
    public abstract class GenericPgTreeWalkerBase
    {
        internal StmtsProcessingContext Context { get; set; }
        public GenericPgTreeWalkerBase(StmtsProcessingContext context)
        {
            Context = context;
        }

        //public abstract TAnalyzeResult GetResult();

        #region Прямой проход
        internal abstract void ProcessDirectTraversal(PgGenericNode node);


        internal virtual void ProcessSelectStmt_DirectTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessUpdateStmt_DirectTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessInsertStmt_DirectTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessDeleteStmt_DirectTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessJoinExpr_DirectTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessResTarget_DirectTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessCommonTableExpr_DirectTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessFuncCall_DirectTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessColumnRef_DirectTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessAssignStmt_DirectTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessBlockStmt_DirectTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessExecSqlStmt_DirectTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessForiStmt_DirectTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessForsStmt_DirectTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessIfStmt_DirectTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessPerformStmt_DirectTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessRaiseStmt_DirectTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessReturnNextStmt_DirectTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessReturnStmt_DirectTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessWithClause_DirectTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessAExpr_DirectTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessString_DirectTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessBoolExpr_DirectTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessSubLink_DirectTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessCaseExpr_DirectTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessCaseWhen_DirectTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessRangeVar_DirectTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessRangeSubselect_DirectTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessRangeFunction_DirectTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessCaseStmt_DirectTraversal(PgGenericNode node)
        {

        }

        #endregion

        #region Обратный проход
        internal abstract void ProcessReverseTraversal(PgGenericNode node);

        internal virtual void ProcessSelectStmt_ReverseTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessUpdateStmt_ReverseTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessInsertStmt_ReverseTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessDeleteStmt_ReverseTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessJoinExpr_ReverseTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessResTarget_ReverseTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessCommonTableExpr_ReverseTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessFuncCall_ReverseTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessColumnRef_ReverseTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessAssignStmt_ReverseTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessBlockStmt_ReverseTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessExecSqlStmt_ReverseTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessForiStmt_ReverseTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessForsStmt_ReverseTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessIfStmt_ReverseTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessPerformStmt_ReverseTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessRaiseStmt_ReverseTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessReturnNextStmt_ReverseTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessReturnStmt_ReverseTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessWithClause_ReverseTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessAExpr_ReverseTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessString_ReverseTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessBoolExpr_ReverseTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessSubLink_ReverseTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessCaseExpr_ReverseTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessCaseWhen_ReverseTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessRangeVar_ReverseTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessRangeSubselect_ReverseTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessRangeFunction_ReverseTraversal(PgGenericNode node)
        {

        }

        internal virtual void ProcessCaseStmt_ReverseTraversal(PgGenericNode node)
        {

        }

        #endregion


        //protected virtual void GeneralPreprocessExpr(PgGenericNode node)
        //{

        //}

        //protected virtual void GeneralPostprocessExpr(PgGenericNode node)
        //{

        //}
    }
}
