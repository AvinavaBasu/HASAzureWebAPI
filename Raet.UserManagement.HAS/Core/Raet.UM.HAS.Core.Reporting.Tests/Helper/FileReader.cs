using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Raet.UM.HAS.Core.Reporting.Tests.Helper
{
    public static class FileReader
    {
        public static IEnumerable<T> GetList<T>(string fileName)
        {
            var data = GetFileData(fileName);
            return JsonConvert.DeserializeObject<IEnumerable<T>>(data);
        }

        public static T Get<T>(string fileName)
        {
            var data = GetFileData(fileName);
            return JsonConvert.DeserializeObject<T>(data);
        }
        public static string GetFileData(string fileName)
        {
            using (StreamReader sr = new StreamReader($"./TestData/{fileName}"))
            {
                var data = sr.ReadToEnd();
                return data;
            }
        }
    }
}
