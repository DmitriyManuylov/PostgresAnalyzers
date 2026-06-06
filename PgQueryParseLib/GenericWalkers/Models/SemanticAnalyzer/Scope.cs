using DataChangeAnalyzer.Models.DBModels;
using PgQueryAnalyzerLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQuery.AnalyzerLib.GenericWalkers.Models.SemanticAnalyzer
{
    public abstract class Scope
    {
        public PgGenericNode Node;

        public Scope(PgGenericNode node)
        {
            Node = node;
        }
    }
}
