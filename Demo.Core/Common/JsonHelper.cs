using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Demo.Core.Common
{
    public class JsonHelper
    {
        public static dynamic ToDic(string value)
        {
            var obj = JsonConvert.DeserializeObject(value) as JToken;

            return ToDic(obj);
        }
        private static dynamic ToDic(JToken token)
        {
            var list = new List<dynamic>();
            var item = new Dictionary<string, object>();

            switch (token.Type)
            {
                case JTokenType.Array:
                    for (var i = 0; i < token.Count(); i++)
                    {
                        list.Add(ToDic(token[i]));
                    }
                    return list;
                case JTokenType.Object:
                    var to = token as JObject;
                    var node = to.First;
                    do
                    {
                        var tp = node as JProperty;
                        item.Add(tp.Name, ToDic(tp.Value));
                        node = node.Next;
                    }
                    while (node != null);
                    return item;
                default:
                    return (token as dynamic).Value;
            }
        }

        public static T ToClass<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
        public static T ToClass<T>(string value, T type)
        {
            return JsonConvert.DeserializeAnonymousType(value, type);
        }
        public static string ToJson(object value)
        {
            return JsonConvert.SerializeObject(value, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                DateFormatString = "yyyy'-'MM'-'dd' 'HH':'mm':'ss"
            });
        }
    }
}
