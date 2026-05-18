using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQueryAnalyzerLib.CustomExceptions
{
    public class PgNodeConstructException: Exception
    {
        public PgNodeConstructException(string message, Exception innerException = null) : base(message, innerException)
        {

        }
    }
}
