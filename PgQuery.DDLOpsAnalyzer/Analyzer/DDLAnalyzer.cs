using DDLOpsAnalyzer.Models;
using PgQueryAnalyzerLib.AnalyzeContext;
using PgQueryAnalyzerLib.GenericWalkers;

namespace DDLOpsAnalyzer.Analyzer
{
    public class DDLAnalyzer : RisingCumulativeAnalyzerBase<DDLAnalyzeModel>
    {
        public DDLAnalyzer(StmtsProcessingContext context) : base(context)
        {
        }
    }
}
