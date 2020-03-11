using RestSharp;
using System.Collections.Generic;

namespace Raet.UM.HAS.Core.Domain
{
    public class RestSharpParams
    {
        public string BaseUrl { get; set; }
        public string ApiEndPoint { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
        public object Parameter { get; set; }
        public DataFormat DataFormat { get; set; } = DataFormat.Json;
        public Method MethodType { get; set; }
        public bool IsDeserializeCamelCaseFormat { get; set; } = false;
    }
}
