using PgQueryParser.LibPgQueryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQueryAnalyzerLib.CustomExceptions
{
    public class PlStmtParseException: Exception
    {
        public PlStmtParseException(
            string message,
            Exception? innerException = null): base(message, innerException)
        { 

        }

        public PlStmtParseException(ParseError parseError)
        {
            ParseError = parseError;
        }

        public ParseError ParseError { get; }
    }
}
