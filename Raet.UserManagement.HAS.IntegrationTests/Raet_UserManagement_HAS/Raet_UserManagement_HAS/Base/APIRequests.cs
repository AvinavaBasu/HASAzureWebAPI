using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Raet_UserManagement_HAS.HelperClasses;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Raet_UserManagement_HAS.Base
{
    public class APIRequests
    {
        public string url { get; set; }
        public string headerContentType { get; set; }
        public string headerClientIdValue { get; set; }
        public string headerAuthorization { get; set; }
        public string parameterValue { get; set; }
        public string ErrorMessage { get; set; }
        public static RestClient client;
        public static Method requestType;
        public static RestRequest request;
        public static IRestResponse response;

        public void EnterURI(string url)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            client = new RestClient(url);

        }

        public void EnterRequestType(string InputRequestType)
        {
            if (InputRequestType.ToLower() == "post")
            {
                requestType = Method.POST;
            }
            else if (InputRequestType.ToLower() == "get")
            {
                requestType = Method.GET;
            }
            request = new RestRequest(requestType);
        }

        public void AddHeader_ContentType(string headerContentType)
        {
            request.AddHeader("Content-Type", headerContentType);
        }

        public void AddBodyValue(string parameterValue)
        {
            request.AddParameter("undefined", parameterValue, ParameterType.RequestBody);
        }

        public void PostRequest()
        {
            response = client.Execute(request);
        }

        public string GetResponse()
        {
            string actualResult = response.StatusCode.ToString();
            Console.WriteLine(actualResult);
            return actualResult;
        }

        public void GetResponseBody_GrantedResponse()
        {
            string responseBody = response.Content;
            //fetching value from key
            //deserialize
            var data = (JObject)JsonConvert.DeserializeObject(responseBody);
            //fetch access_token
            string AuthenticationToken = data["access_token"].Value<string>();
            Console.WriteLine(AuthenticationToken);
            HASTokensAndVariables.AccessToken = AuthenticationToken;
        }

        public void AddHeader_CleintIDValue(string headerClientIdValue)
        {
            request.AddHeader("x-raet-tenant-id", headerClientIdValue);
        }

        public void AddHeader_AuthorizationValue1()
        {
            request.AddHeader("Authorization", $"Bearer {HASTokensAndVariables.AccessToken}");
            Console.WriteLine(HASTokensAndVariables.AccessToken);
        }

        public void GetResponseBody_GrantedAPIResponse()
        {
            string responseBody = response.Content;
            //fetching value from key
            //deserialize
            var data = (JObject)JsonConvert.DeserializeObject(responseBody);
            //fetch access_token
            string Event_GUID = data.SelectToken("id").Value<string>();
            string Permission = data.SelectToken("effectiveAuthorization.permission.id").Value<string>();
            string ApplicationName = data.SelectToken("effectiveAuthorization.permission.application").Value<string>();
            string FromDate = data.SelectToken("fromDateTime").Value<string>();
            //string ApplicationName = data["permission.id"].Value<string>();
            Console.WriteLine(Event_GUID);
            Console.WriteLine(Permission);
            Console.WriteLine(ApplicationName);
            HASTokensAndVariables.GUID_EventResponse = Event_GUID;
            HASTokensAndVariables.Application_Name = ApplicationName;
            HASTokensAndVariables.Permission_Name = Permission;
            HASTokensAndVariables.FromDate = FromDate;
        }
        public void AddHeader_AuthorizationValue()
        {
            request.AddHeader("Authorization", $"Bearer {HASTokensAndVariables.AuthenticationToken}");
            Console.WriteLine(HASTokensAndVariables.AuthenticationToken);
        }
        public void AddBodyValue_GenerateReportAPI()
        {
            var ApplicationName = HASTokensAndVariables.Application_Name;
            var PermissionName = HASTokensAndVariables.Permission_Name;
            var StartDate = HASTokensAndVariables.FromDate;
            string d1 = DateTime.Now.ToString().Replace(":", "_");
            Console.WriteLine(ApplicationName + PermissionName + StartDate);
            request.AddParameter("undefined", "{ \"application\": \"" + ApplicationName + "\", \"permissions\": [   \"" + PermissionName + "\" ],  \"startDate\": \"" + StartDate + "\",  \"endDate\": \"2020-03-01T05:49:52+00:00\",  \"fileName\": \"Report_APIScript" + d1 + "\",\"tenantId\": \"188a2e34-410b-41af-a501-8e99482a8e8e\",\n\t\"sourceUser\": {\n    \"context\": \"\",\n    \"id\": \"\"},\n\t\"targetUser\": {\n    \"context\": \"Youforce.Users\",\n    \"id\": \"IC112070\"}\n}", ParameterType.RequestBody);
        }

        public void GetResponseBody_GUID()
        {
            string responseBody = response.Content;
            //fetching value from key
            //deserialize
            var data = (JObject)JsonConvert.DeserializeObject(responseBody);
            //fetch access_token
            string GUID = data["guid"].Value<string>();
            string FileName = data["fileName"].Value<string>();
            Console.WriteLine(GUID);
            Console.WriteLine(FileName);
            HASTokensAndVariables.GUID_GenerateReport = GUID;
            HASTokensAndVariables.FileName_API = FileName;
        }

        public void GetApplications_Response()
        {
            var v = HASTokensAndVariables.Application_Name;
            string responseBody = response.Content;
            Console.WriteLine(responseBody);
            if (responseBody.Contains(v))
            {
                Console.WriteLine("Application Exists : " + v);
            }
            else
            {
                Console.WriteLine("Fail");
            }
        }

        public void EnterURI_GetPermissionDetailsAPI(string url)
        {
            client = new RestClient(url + HASTokensAndVariables.Application_Name + "/188a2e34-410b-41af-a501-8e99482a8e8e");
            Console.WriteLine(client);
        }

        public void GetPermission_Response()
        {
            var v = HASTokensAndVariables.Permission_Name;
            string responseBody = response.Content;
            Console.WriteLine(responseBody);
            if (responseBody.Contains(v))
            {
                Console.WriteLine("Permission Exists : " + v);
            }
            else
            {
                Console.WriteLine("Fail");
            }
        }

        public void GetReport_Response()
        {
            var v = HASTokensAndVariables.FileName_API;
            string responseBody = response.Content;
            Console.WriteLine(responseBody);
            if (responseBody.Contains(v))
            {
                Console.WriteLine("Report Exists : " + v);
            }
            else
            {
                Console.WriteLine("Fail");
            }
        }

        public void EnterURI_DownloadAPI(string url)
        {
            Thread.Sleep(20000);
            client = new RestClient(url + HASTokensAndVariables.GUID_GenerateReport);
            Console.WriteLine(client);
        }

        public void GetResponseBody_ReportDetail()
        {
            string responseBody = response.Content;
            Console.WriteLine(responseBody);
            //fetching value from key
            //deserialize
            // var data = (JObject)JsonConvert.DeserializeObject(responseBody);
            //fetch access_token
            //string GUID = data["guid"].Value<string>();
            //Console.WriteLine(GUID);
            // HASTokensAndVariables.GUID_GenerateReport = GUID;
        }
        public void GetResponseBody_ErrorMSG_GrantedAPIResponse(string ErrorMessage)
        {
            string responseBody = response.Content;
            Console.WriteLine(ErrorMessage);
            //deserialize
            var data = (JObject)JsonConvert.DeserializeObject(responseBody);
            string MsgDetails = data.ToString();
            if (MsgDetails.Contains(ErrorMessage))
            {
                Console.WriteLine(ErrorMessage);
            }
            Console.WriteLine(MsgDetails);
        }
    }
}
