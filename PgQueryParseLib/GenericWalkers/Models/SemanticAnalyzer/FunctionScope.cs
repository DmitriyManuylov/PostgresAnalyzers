using PgQueryAnalyzerLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQuery.AnalyzerLib.GenericWalkers.Models.SemanticAnalyzer
{
    internal class FunctionScope : Scope
    {
        public FunctionScope(PgGenericNode node) : base(node)
        {

        }
    }
}
