using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raet_UserManagement_HAS.HelperClasses
{
    public static class HASTokensAndVariables
    {
        public static string AccessToken;
        public static string AuthenticationToken;
        public static string Application_Name;
        public static string Permission_Name;
        public static string GUID_GenerateReport;
        public static string GUID_EventResponse;
        public static string FileName_API;
        public static string FromDate;
        public static string ReportName;


        public static void clear()
        {
            AccessToken = string.Empty;
            AuthenticationToken = string.Empty;
            Application_Name = string.Empty;
            Permission_Name = string.Empty;
            GUID_GenerateReport = string.Empty;
            GUID_EventResponse = string.Empty;
            FileName_API = string.Empty;
            FromDate = string.Empty;
            ReportName = string.Empty;
        }
    }
}
