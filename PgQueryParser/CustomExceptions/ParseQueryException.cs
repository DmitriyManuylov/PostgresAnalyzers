using PgQueryParser.LibPgQueryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQueryParseLib.CustomExceptions
{
    public class ParseQueryException: Exception
    {
        public ParseQueryException(ParseError parseError)
        {
            ParseError = parseError;
        }

        public ParseError ParseError { get; }
    }
}
