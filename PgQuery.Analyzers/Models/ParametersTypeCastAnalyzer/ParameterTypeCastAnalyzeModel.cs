using PgQuery.AnalyzerLib.GenericWalkers.Models.SemanticAnalyzer.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQuery.Analyzers.Models.ParametersTypeCastAnalyzer
{
    public class ParameterTypeCastAnalyzeModel
    {
        public string ParameterName { get; set; }
        public bool HasCast { get; set; }
        public string TypeCastName {  get; set; }
        public string TypeCastMod {  get; set; }
        public bool IsArray { get; set; } = false;
        public ComparableDBColumn ComparableDBColumn { get; set; }
    }
}
