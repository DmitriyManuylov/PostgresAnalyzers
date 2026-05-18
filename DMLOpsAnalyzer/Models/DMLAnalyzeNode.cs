using PgQueryAnalyzerLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQueryAnalyzerLib.GenericWalkers.Models
{
    public class DMLAnalyzeNode : AnalyzeNodeBase
    {
        public DMLAnalyzeModel Model { get; set; }

        public DMLAnalyzeNode()
        {

        }
    }
}
