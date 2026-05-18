using PgQueryAnalyzerLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQueryAnalyzerLib.GenericWalkers.Models
{
    public class AnalyzeNodeBase
    {
        public PgGenericNode PgGenericNode { get; set; }

        /// <summary>
        /// Нужно ли распространять результат анализа в вышележащие узлы
        /// </summary>
        public bool IsNeedToPropogate { get; set; } = true;
    
    }
}
