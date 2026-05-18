using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQueryAnalyzerLib.StmtsVisit
{
    public class ProcessingNode
    {
        public ProcessingNode()
        {

        }

        public ProcessingNode ParentNode { get; set; }

        public List<ProcessingNode> ChildNodes { get; set; }


    }
}
