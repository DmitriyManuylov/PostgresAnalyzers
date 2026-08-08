using PgQueryAnalyzerLib.AnalyzeContext;
using PgQueryAnalyzerLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQuery.AnalyzerLib.GenericWalkers.WalkerBase
{
    public abstract partial class GenericPgTreeWalkerBase
    {
        public StmtsProcessingContext Context { get; set; }

        public GenericPgTreeWalkerBase()
        {

        }
        public GenericPgTreeWalkerBase(StmtsProcessingContext context)
        {
            Context = context;
        }

        //public abstract TAnalyzeResult GetResult();

        #region Прямой проход
        protected abstract void ProcessDirectTraversal(PgGenericNode node);


        protected virtual void ProcessSelectStmt_DirectTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessUpdateStmt_DirectTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessInsertStmt_DirectTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessDeleteStmt_DirectTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessJoinExpr_DirectTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessResTarget_DirectTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessCommonTableExpr_DirectTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessFuncCall_DirectTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessColumnRef_DirectTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessAssignStmt_DirectTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessBlockStmt_DirectTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessExecSqlStmt_DirectTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessForiStmt_DirectTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessForsStmt_DirectTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessIfStmt_DirectTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessPerformStmt_DirectTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessRaiseStmt_DirectTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessReturnNextStmt_DirectTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessReturnStmt_DirectTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessWithClause_DirectTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessAExpr_DirectTraversal(PgGenericNode node)
        {

        }
        protected virtual void ProcessTypeCast_DirectTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessString_DirectTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessBoolExpr_DirectTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessSubLink_DirectTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessCaseExpr_DirectTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessCaseWhen_DirectTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessRangeVar_DirectTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessRangeSubselect_DirectTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessRangeFunction_DirectTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessCaseStmt_DirectTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessAlterTableStmt_DirectTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessAlterTableCmd_DirectTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessDropStmt_DirectTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessRenameStmt_DirectTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessMultiAssignRef_DirectTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessMultiAssignRef_ReverseTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessList_DirectTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessParamRef_DirectTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessCoalesceExpr_DirectTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessNullTest_DirectTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessNullIfExpr_DirectTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessIndexStmt_DirectTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessIndexElem_DirectTraversal(PgGenericNode node)
        {

        }

        #endregion

        #region Обратный проход
        protected abstract void ProcessReverseTraversal(PgGenericNode node);

        protected virtual void ProcessSelectStmt_ReverseTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessUpdateStmt_ReverseTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessInsertStmt_ReverseTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessDeleteStmt_ReverseTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessJoinExpr_ReverseTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessResTarget_ReverseTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessCommonTableExpr_ReverseTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessFuncCall_ReverseTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessColumnRef_ReverseTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessAssignStmt_ReverseTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessBlockStmt_ReverseTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessExecSqlStmt_ReverseTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessForiStmt_ReverseTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessForsStmt_ReverseTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessIfStmt_ReverseTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessPerformStmt_ReverseTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessRaiseStmt_ReverseTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessReturnNextStmt_ReverseTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessReturnStmt_ReverseTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessWithClause_ReverseTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessAExpr_ReverseTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessTypeCast_ReverseTraversal(PgGenericNode node)
        {

        }
        protected virtual void ProcessString_ReverseTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessBoolExpr_ReverseTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessSubLink_ReverseTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessCaseExpr_ReverseTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessCaseWhen_ReverseTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessRangeVar_ReverseTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessRangeSubselect_ReverseTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessRangeFunction_ReverseTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessCaseStmt_ReverseTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessAlterTableStmt_ReverseTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessAlterTableCmd_ReverseTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessDropStmt_ReverseTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessRenameStmt_ReverseTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessRowExpr_DirectTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessRowExpr_ReverseTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessList_ReverseTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessParamRef_ReverseTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessCoalesceExpr_ReverseTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessNullTest_ReverseTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessNullIfExpr_ReverseTraversal(PgGenericNode node)
        {

        }

        protected virtual void ProcessIndexStmt_ReverseTraversal(PgGenericNode node)
        {
            
        }

        protected virtual void ProcessIndexElem_ReverseTraversal(PgGenericNode node)
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
