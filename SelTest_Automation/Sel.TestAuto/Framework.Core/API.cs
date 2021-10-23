using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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

        /// <summary>
        /// Function to Send a API call and get response based on API type and method
        /// </summary>
        /// <param name="method"></param>
        /// <param name="baseUrl"></param>
        /// <param name="input"></param>
        /// <param name="apiType"></param>
        /// <param name="defaultHeaderKeyValue">Optional Parameter: To be entered in Key Value pair set. Ex: Key1:Val1;Key2:val2;..</param>
        /// <returns></returns>
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

        public static bool ResponseFileValidation(string filePath, string expectedVals, bool checkExist = true)
        {
            bool flag = false;
            int cntrp = 0;
            int cntrnp = 0;
            string[] strVal = expectedVals.Split(';');
            StreamReader read = new StreamReader(filePath, Encoding.UTF8);
            string replyMsg = read.ReadToEnd().Replace(Environment.NewLine, " ").Replace("\t", "");
            try
            {
                foreach (string exp in strVal)
                {
                    if (replyMsg.Contains(exp))
                    {
                        
                        cntrp++;
                    }
                    else
                    {
                        
                        cntrnp++;
                    }
                }

                if (!checkExist)
                {
                    if (cntrnp == strVal.Length)
                    {
                        flag = true;
                        
                    }
                    else
                    {
                        
                    }
                }
                else if (cntrp == strVal.Length)
                {
                    flag = true;
                    
                }
                else
                {
                    
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " " + ex.StackTrace);
            }
            return flag;
        }

        public static bool ResponseValidation(string responseString, string expectedVals, bool checkExist = true)
        {
            bool flag = false;
            int cntrp = 0;
            int cntrnp = 0;
            string[] strVal = expectedVals.Split(';');
            string replyMsg = responseString;
            try
            {
                foreach (string exp in strVal)
                {
                    if (replyMsg.Contains(exp))
                    {
                        
                        cntrp++;
                    }
                    else
                    {
                        
                        cntrnp++;
                    }
                }

                if (!checkExist)
                {
                    if (cntrnp == strVal.Length)
                    {
                        flag = true;
                        
                    }
                    else
                    {
                        
                    }
                }
                else if (cntrp == strVal.Length)
                {
                    flag = true;
                    
                }
                else
                {
                    
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " " + ex.StackTrace);
            }
            return flag;
        }

        public static string GetXMLTagValue(string xmlfilePath, string tagName)
        {
            string tagVal = string.Empty;
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlfilePath);

                XmlNodeList nodeList = xmlDoc.GetElementsByTagName(tagName);
                if (nodeList.Count != 0)
                {
                    tagVal = nodeList[0].InnerText;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " " + ex.StackTrace);
            }
            return tagVal;
        }


    }
}
