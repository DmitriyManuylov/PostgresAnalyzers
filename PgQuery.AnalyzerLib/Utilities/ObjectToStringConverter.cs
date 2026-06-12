using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace PgQueryAnalyzerLib.Utilities
{
    public static class ObjectToStringConvertExtention
    {
        public static string ConvertObjectToString<TParam>(this TParam Param)
        {
            string objJson = JsonSerializer.Serialize(Param);

            Dictionary<string, object> map = JsonSerializer.Deserialize<Dictionary<string, object>>(objJson);
            StringBuilder builder = new StringBuilder();
            foreach(var item in map)
            {
                builder.Append($"{item.Key}: {item.Value} ");
                builder.AppendLine();
            }
            string result = builder.ToString();
            return result;
        }

        public static string ConvertObjectCollectionToString<TParam>(this List<TParam> list) 
        {
            int x;
            int y;
            y = x = 5;
            StringBuilder builder = new StringBuilder();
            foreach (var item in list)
            {
                builder = builder.Append(item.ConvertObjectToString());
                builder.AppendLine();
            }
            string result = builder.ToString();
            return result;
        }
    }
}
