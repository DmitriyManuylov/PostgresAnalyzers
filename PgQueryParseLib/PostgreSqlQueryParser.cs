using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Text;
using System.Xml.Linq;
using PgQueryParseLib.LibPgQueryModels;
using PgQueryParseLib.CustomExceptions;
using PgQuery;
using pb = global::Google.Protobuf;
using Google.Protobuf;

namespace PgQueryParseLib
{
    public class PostgreSqlQueryParser
    {
        public byte[] GetQueryProtobufParseTree(string query)
        {
            return GetQueryProtobufParseTreeWithOptions(query, (int)RawParseMode.RawParseDefault);
        }

        public string GetQueryParseTree(string query)
        {
            return GetQueryParseTreeWithOptions(query, (int)RawParseMode.RawParseDefault);
        }

        public string GetQueryParseTreeWithOptions(string query, int parser_mode = (int)RawParseMode.RawParsePlpgsqlAssign1)
        {
            string jsonString = string.Empty;

            var str = GetQueryString(query);

            unsafe
            {
                byte* result = PgQueryNativeLibWrapper.GetQueryParseTreeWithOptions(str, parser_mode);

                if (result == null)
                {
                    throw new NullReferenceException("Нулевой указатель");
                }

                int code = *(int*)result;

                if (code == 1)
                {
                    var errorPtr = result + sizeof(int);
                    ParseError parseError = DeserializeParseError(errorPtr);
                    NativeMemory.Free(result);
                    throw new ParseQueryException(parseError);
                }

                int jsonStringLength = *((int*)result + 1);
                byte[] jsonStringByteArr = new byte[jsonStringLength];

                byte* messagePtr = result + 2 * sizeof(int);
                byte* curchar = messagePtr - 1;

                for (int i = 0; i < jsonStringLength; i++)
                {
                    jsonStringByteArr[i] = *++curchar;
                }

                jsonString = Encoding.ASCII.GetString(jsonStringByteArr);

                NativeMemory.Free(result);
            }

            return jsonString;
        }

        public byte[] GetQueryProtobufParseTreeWithOptions(string query, int parser_mode = (int)RawParseMode.RawParsePlpgsqlAssign1)
        {
            var str = GetQueryString(query);

            byte[] protobufArray;

            unsafe
            {
                byte* result = PgQueryNativeLibWrapper.GetQueryProtobufParseTreeWithOptions(str, parser_mode);

                if (result == null)
                {
                    throw new NullReferenceException("Нулевой указатель");
                }
                //byte* bt = (byte*)result;

                int code = *(int*)result;

                if (code == 1)
                {
                    var errorPtr = result + sizeof(int);
                    ParseError parseError = DeserializeParseError(errorPtr);
                    NativeMemory.Free(result);
                    throw new ParseQueryException(parseError);
                }

                int strlength = *((int*)result + 1);

                byte* messagePtr = result + 2 * sizeof(int);
                byte* curChar = messagePtr - 1;

                protobufArray = new byte[strlength];

                for (int i = 0; i < strlength; i++)
                {
                    protobufArray[i] = *++curChar;
                }

                NativeMemory.Free(result);

            }

            return protobufArray;
        }

        public TParseResult GetQueryProtobufParseTreeWithOptions<TParseResult>(
            string query, 
            int parser_mode = (int)RawParseMode.RawParsePlpgsqlAssign1) 
            where TParseResult: pb::IMessage<TParseResult>, new()
        {
            var res = GetQueryProtobufParseTreeWithOptions(query, parser_mode);

            var parser = new MessageParser<TParseResult>(() => new TParseResult());

            return parser.ParseFrom(res);
        }

        public string GetPlPgQueryJsonParseTree(string query)
        {

            var str = GetQueryString(query);
            string jsonString = string.Empty;

            unsafe
            {
                byte* result = PgQueryNativeLibWrapper.GetPlPgQueryJsonParseTree(str);

                if(result == null)
                {
                    throw new NullReferenceException("Нулевой указатель");
                }

                int code = *(int*)result;
                if(code == 1)
                {
                    var errorPtr = result + sizeof(int);
                    ParseError parseError = DeserializeParseError(errorPtr);
                    NativeMemory.Free(result);
                    throw new ParseQueryException(parseError);
                }

                int jsonStringLength = *((int*)result + 1);
                byte[] jsonStringByteArr = new byte[jsonStringLength];

                byte* messagePtr = result + 2 * sizeof(int);
                byte* curchar = messagePtr - 1;

                for(int i = 0; i < jsonStringLength; i++)
                {
                    jsonStringByteArr[i] = *++curchar;
                }

                jsonString = Encoding.ASCII.GetString(jsonStringByteArr);

                NativeMemory.Free(result);
            }

            return jsonString;
        }

        unsafe ParseError DeserializeParseError(byte* errorPtr)
        {
            ParseError error = new ParseError();
            var curPtr = errorPtr;
            int mesLen = *(int*)curPtr;
            //curPtr += sizeof(int);

            var message = ReadASCIIString(curPtr);
            error.Message = message.str;
            curPtr += message.strBytesCount;

            var funcname = ReadASCIIString(curPtr);
            error.Funcname = funcname.str;
            curPtr += funcname.strBytesCount;

            var filename = ReadASCIIString(curPtr);
            error.Filename = filename.str;
            curPtr += filename.strBytesCount;

            error.Lineno = *(int*)curPtr;
            curPtr += sizeof(int);
            error.Cursorpos = *(int*)curPtr;
            curPtr += sizeof(int);

            var context = ReadASCIIString(curPtr);
            error.Context = context.str;

            return error;
        }

        unsafe (string str, int strBytesCount) ReadASCIIString(byte* stringPtr)
        {
            int strLen = *(int*)stringPtr;
            byte* curPtr = stringPtr + sizeof(int) - 1;
            if (strLen == 0)
            {
                return (string.Empty, 0);
            }
            byte[] bytes = new byte[strLen];

            for(int i = 0; i < strLen; i++)
            {
                bytes[i] = *++curPtr;
            }
            string str = Encoding.ASCII.GetString(bytes);

            return (str, strLen + sizeof(int));
        }

        string GetQueryString(string inputString)
        {
            string patternColon = "(?<!:):(?![:=])";

            string str = string.Empty;
            str = Regex.Replace(inputString, patternColon, "@");

            return str;
        }

    }

    public static class PgQueryNativeLibWrapper
    {
        [DllImport("PgQueryAnalyzerLib.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern unsafe byte* GetQueryParseTree(string query);
        [DllImport("PgQueryAnalyzerLib.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern unsafe byte* GetQueryParseTreeWithOptions(string query, int parser_mode);

        [DllImport("PgQueryAnalyzerLib.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern unsafe byte* GetQueryProtobufParseTree(string query);
        [DllImport("PgQueryAnalyzerLib.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern unsafe byte* GetQueryProtobufParseTreeWithOptions(string query, int parse_mode);

        [DllImport("PgQueryAnalyzerLib.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern unsafe byte* GetPlPgQueryJsonParseTree(string query);
    }
}
