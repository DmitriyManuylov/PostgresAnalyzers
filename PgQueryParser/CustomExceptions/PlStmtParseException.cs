using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQueryParseLib.CustomExceptions
{
    public class PlStmtParseException: Exception
    {
        public PlStmtParseException(
            string message,
            Exception? innerException = null): base(message, innerException)
        { 

        }
    }
}
