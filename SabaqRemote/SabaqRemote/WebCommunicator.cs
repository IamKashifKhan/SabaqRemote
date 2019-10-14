using ModernHttpClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SabaqRemote
{
    public class WebCommunicator
    {
          

        //BROWSE METHOD FOR WEB BROWSING
        public async Task<String> FormGetWebBroserJSON(string uri)
        {
            string result = String.Empty;
            HttpClientHandler handler = new HttpClientHandler()
            {
                Proxy = null,
                UseProxy = false,
            };
            // using (var client = new HttpClient(handler))
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                try
                {
                    using (var WebResponse = await client.GetAsync(uri))
                    {
                        using (var WebResponseContent = WebResponse.Content)
                        {
                            result = await WebResponseContent.ReadAsStringAsync();
                            return await Task.Run(() => result);
                        }
                    }
                }
                catch { return await Task.Run(() => "[{\"ERROR\":\"INTERNET MISSING\"}]"); }
            }
        }



        //POST METHOD FOR WEBSERVICE USING MultipartFormDataContent
        public async Task<JArray> FormGetWebServiceJSON(string uri, MultipartFormDataContent Parameters)
        {
            string result = String.Empty;
            HttpClientHandler handler = new HttpClientHandler()
            {
                Proxy = null,
                UseProxy = false,
            };
            // using (var client = new HttpClient(handler))
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                var content = Parameters;
                try
                {
                    using (var WebResponse = await client.PostAsync(uri, content))
                    {
                        using (var WebResponseContent = WebResponse.Content)
                        {
                            result = await WebResponseContent.ReadAsStringAsync();
                            result = result.Substring(result.IndexOf("["), result.LastIndexOf("]") - result.IndexOf("[") + 1);
                            return await Task.Run(() => JArray.Parse(result));

                        }
                    }
                }
                catch { return await Task.Run(() => JArray.Parse("[{\"ERROR\":\"INTERNET MISSING\"}]")); }
            }
        }

        public async Task<String> FormGetWebServiceJSONString(string uri, MultipartFormDataContent Parameters)
        {
            string result = String.Empty;
            HttpClientHandler handler = new HttpClientHandler()
            {
                Proxy = null,
                UseProxy = false,
            };
            //using (var client = new HttpClient(handler)
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                var content = Parameters;
                try
                {
                    using (var WebResponse = await client.PostAsync(uri, content))
                    {
                        using (var WebResponseContent = WebResponse.Content)
                        {
                            result = await WebResponseContent.ReadAsStringAsync();
                            result = result.Substring(result.IndexOf("["), result.LastIndexOf("]") - result.IndexOf("[") + 1);
                            return await Task.Run(() => result);
                        }
                    }
                }
                catch { return await Task.Run(() => "[{\"ERROR\":\"INTERNET MISSING\"}]"); }
            }
        }

    }
}
