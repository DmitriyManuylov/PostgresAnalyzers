using Analyzers.Models.ParametersTypeCastAnalyzer;
using PgQuery;
using PgQueryAnalyzerLib.AnalyzeContext;
using PgQueryAnalyzerLib.GenericWalkers;
using PgQueryAnalyzerLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzers
{
    public class ParametersTypeCastAnalyzer : GenericPgTreeWalkerBase
    {
        private List<ParameterTypeCastAnalyzeModel> analyzeModels = new();

        public ParametersTypeCastAnalyzer()
        {
        }

        public ParametersTypeCastAnalyzer(StmtsProcessingContext context) : base(context)
        {
        }

        public override void ProcessDirectTraversal(PgGenericNode node)
        {

        }

        public override void ProcessReverseTraversal(PgGenericNode node)
        {

        }

        public override void ProcessParamRef_DirectTraversal(PgGenericNode node)
        {
            base.ProcessParamRef_DirectTraversal(node);

            var paramRef = node.PgSqlNode.ParamRef;

            var stackList = Context.PgGenericNodes.ToList();

            TypeCast? typeCastStmt = null;
            string parameterName = Context.QueryParameters[paramRef.Number - 1];

            for (int i = stackList.Count - 1; i >= 0; i--)
            {
                if (stackList[i].PgSqlNode?.NodeCase == PgQuery.Node.NodeOneofCase.TypeCast)
                {
                    typeCastStmt = stackList[i].PgSqlNode.TypeCast;
                }
            }

            var whereClause = Context.GetNearestNodeByType(Node.NodeOneofCase.SelectStmt, node => new List<string>
            {
                nameof(SelectStmt.WhereClause), 
                nameof(SelectStmt.FromClause)
            }
            .Contains(node.SubOperation));

            if (whereClause is null)
            {
                return;
            }

            if (typeCastStmt != null)
            {
                analyzeModels.Add(new ParameterTypeCastAnalyzeModel
                {
                    ParameterName = parameterName,
                    HasCast = true,
                    TypeCastName = typeCastStmt.TypeName.Names.LastOrDefault()?.String.Sval,
                    TypeCastMod = typeCastStmt.TypeName.Typmods.FirstOrDefault()?.AConst?.Ival.Ival.ToString(),
                    IsArray = typeCastStmt.TypeName.ArrayBounds.Any()
                });
            }
            else
            {
                analyzeModels.Add(new ParameterTypeCastAnalyzeModel
                {
                    ParameterName = parameterName,
                    HasCast = false,
                });
            }
        }

        //public override void ProcessAExpr_DirectTraversal(PgGenericNode node)
        //{
        //    base.ProcessAExpr_DirectTraversal(node);

        //    var aExprStmt = node.PgSqlNode.AExpr;

        //    if (aExprStmt.Kind != PgQuery.A_Expr_Kind.AexprOp)
        //    {
        //        return;
        //    }

        //    string aexprName = string.Join(".", aExprStmt.Name.Select(item => item.String.Sval));

        //    if (aexprName != "@")
        //    {
        //        return;
        //    }

        //    var stackList = Context.PgGenericNodes.ToList();

        //    TypeCast? typeCastStmt = null;
        //    string parameterName = string.Empty;

        //    for (int i = stackList.Count - 1; i >= 0; i--)
        //    {
        //        if (stackList[i].PgSqlNode?.NodeCase == PgQuery.Node.NodeOneofCase.TypeCast)
        //        {
        //            typeCastStmt = stackList[i].PgSqlNode.TypeCast;
        //            parameterName = aExprStmt.Rexpr.ColumnRef.Fields.FirstOrDefault()?.String.Sval;
        //        }
        //    }

        //    if (typeCastStmt is null)
        //    {
        //        if (aExprStmt.Rexpr.NodeCase == Node.NodeOneofCase.TypeCast)
        //        {
        //            typeCastStmt = aExprStmt.Rexpr.TypeCast;
        //            parameterName = typeCastStmt.Arg.ColumnRef?.Fields.LastOrDefault()?.String.Sval;
        //        }
        //    }

        //    if (typeCastStmt != null)
        //    {
        //        analyzeModels.Add(new ParameterTypeCastAnalyzeModel
        //        {
        //            ParameterName = parameterName,
        //            HasCast = true,
        //            TypeCastName = typeCastStmt.TypeName.Names.LastOrDefault()?.String.Sval,
        //            TypeCastMod = typeCastStmt.TypeName.Typmods.FirstOrDefault()?.AConst?.Ival.Ival.ToString(),
        //            IsArray = typeCastStmt.TypeName.ArrayBounds.Any()
        //        });
        //    }
        //    else
        //    {
        //        parameterName = aExprStmt.Rexpr?.ColumnRef?.Fields?.FirstOrDefault()?.String?.Sval;

        //        if (string.IsNullOrWhiteSpace(parameterName))
        //        {
        //            return;
        //        }

        //        analyzeModels.Add(new ParameterTypeCastAnalyzeModel
        //        {
        //            ParameterName = aExprStmt.Rexpr.ColumnRef.Fields.FirstOrDefault()?.String.Sval,
        //            HasCast = false,
        //        });
        //    }

        //}

        //public override void ProcessAExpr_ReverseTraversal(PgGenericNode node)
        //{
        //    base.ProcessAExpr_ReverseTraversal(node);
        //}

        public List<ParameterTypeCastAnalyzeModel> GetResult()
        {
            return analyzeModels;
        }
    }
}
