using Raet.UM.HAS.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Raet.UM.HAS.Core.Reporting.Interface
{
    public interface IRestSharp
    {
        List<T> GetList<T>(RestSharpParams settings);
        List<T> PostList<T>(RestSharpParams settings);
        T Get<T>(RestSharpParams settings);
        string Post(RestSharpParams settings);
        void Delete(RestSharpParams settings);
        string Get(RestSharpParams settings);
    }
}
