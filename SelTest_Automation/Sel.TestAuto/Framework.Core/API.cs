using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Sel.TestAuto
{
    public static class API
    {
        private static string content = string.Empty;
        private static int statusCode;
        private static string resultText = string.Empty;
        private static string contentType = string.Empty;

        private static async Task<String> API_AsyncGET(string baseURL, string inputString, string apiType, string defaultHeaderKeyValue = null)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(apiType));
                    if (defaultHeaderKeyValue != null)
                    {
                        string[] headerKeyValuePairs = defaultHeaderKeyValue.Split(';');
                        foreach (string headerKeyValue in headerKeyValuePairs)
                        {
                            httpClient.DefaultRequestHeaders.Add(headerKeyValue.Split(':')[0], headerKeyValue.Split(':')[1]);
                        }
                    }

                    HttpResponseMessage response = await httpClient.GetAsync(baseURL);
                    content = await response.Content.ReadAsStringAsync();
                    statusCode = (int)response.StatusCode;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " " + ex.StackTrace);

            }
            return content;
        }

        private static async Task<String> API_AsyncDELETE(string baseURL, string inputString, string apiType, string defaultHeaderKeyValue = null)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(apiType));
                    if (defaultHeaderKeyValue != null)
                    {
                        string[] headerKeyValuePairs = defaultHeaderKeyValue.Split(';');
                        foreach (string headerKeyValue in headerKeyValuePairs)
                        {
                            httpClient.DefaultRequestHeaders.Add(headerKeyValue.Split(':')[0], headerKeyValue.Split(':')[1]);
                        }
                    }

                    HttpResponseMessage response = await httpClient.DeleteAsync(baseURL);
                    content = await response.Content.ReadAsStringAsync();
                    statusCode = (int)response.StatusCode;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " " + ex.StackTrace);
            }
            return content;
        }

        private static async Task<String> API_AsyncPOST(string baseURL, string inputString, string apiType, string defaultHeaderKeyValue = null)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var httpContent = new StringContent(inputString, Encoding.UTF8, apiType);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(apiType));
                    if (defaultHeaderKeyValue != null)
                    {
                        string[] headerKeyValuePairs = defaultHeaderKeyValue.Split(';');
                        foreach (string headerKeyValue in headerKeyValuePairs)
                        {
                            httpClient.DefaultRequestHeaders.Add(headerKeyValue.Split(':')[0], headerKeyValue.Split(':')[1]);
                        }
                    }

                    HttpResponseMessage response = await httpClient.PostAsync(baseURL, httpContent);
                    content = await response.Content.ReadAsStringAsync();
                    statusCode = (int)response.StatusCode;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " " + ex.StackTrace);
            }
            return content;
        }

        private static async Task<String> API_AsyncPUT(string baseURL, string inputString, string apiType, string defaultHeaderKeyValue = null)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var httpContent = new StringContent(inputString, Encoding.UTF8, apiType);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(apiType));
                    if (defaultHeaderKeyValue != null)
                    {
                        string[] headerKeyValuePairs = defaultHeaderKeyValue.Split(';');
                        foreach (string headerKeyValue in headerKeyValuePairs)
                        {
                            httpClient.DefaultRequestHeaders.Add(headerKeyValue.Split(':')[0], headerKeyValue.Split(':')[1]);
                        }
                    }

                    HttpResponseMessage response = await httpClient.PutAsync(baseURL, httpContent);
                    content = await response.Content.ReadAsStringAsync();
                    statusCode = (int)response.StatusCode;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " " + ex.StackTrace);
            }
            return content;
        }

        public static string API_SendReceive(string method, string baseUrl, string input, string apiType, string defaultHeaderKeyValue = null)
        {
            try
            {
                if (apiType.ToLower().Equals("xml") || apiType.ToLower().Equals("soap"))
                {
                    contentType = "application/xml";
                }
                else if (apiType.ToLower().Equals("json"))
                {
                    contentType = "application/json";
                }

                switch (method.ToLower())
                {
                    case "get":
                        resultText = API_AsyncGET(baseUrl, input, contentType, defaultHeaderKeyValue).Result;
                        break;

                    case "post":
                        resultText = API_AsyncPOST(baseUrl, input, contentType, defaultHeaderKeyValue).Result;
                        break;

                    case "put":
                        resultText = API_AsyncPUT(baseUrl, input, contentType, defaultHeaderKeyValue).Result;
                        break;

                    case "delete":
                        resultText = API_AsyncDELETE(baseUrl, input, contentType, defaultHeaderKeyValue).Result;
                        break;

                    default:
                        resultText = "Not a valid request, please check input parameter";
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " " + ex.StackTrace);
            }
            return resultText;
        }

        public static int StatusCode()
        {
            return statusCode;
        }






    }
}
