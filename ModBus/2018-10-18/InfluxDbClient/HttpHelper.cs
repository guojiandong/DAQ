using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Ksat.InfluxDbClient
{
    internal static class HttpHelper
    {
        private static string _username = "admin";
        private static string _password = "admin";

        public static void SetAuthorization(string user, string pwd)
        {
            _username = user;
            _password = pwd;
        }

        public static string HttpHelperGet(string uri)
        {
            try
            {
                string result = string.Empty;
                HttpWebRequest wbRequest = (HttpWebRequest)WebRequest.Create(uri);
                if (!string.IsNullOrEmpty(_username) && !string.IsNullOrEmpty(_password))
                {
                    wbRequest.Credentials = GetCredentialCache(uri, _username, _password);
                    wbRequest.Headers.Add(_username, _password);
                }
                wbRequest.Method = "GET";
                HttpWebResponse wbResponse = (HttpWebResponse)wbRequest.GetResponse();
                using (Stream responseStream = wbResponse.GetResponseStream())
                {
                    using (StreamReader sReader = new StreamReader(responseStream))
                    {
                        result = sReader.ReadToEnd();
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="paramStr"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string HttpHelperPost(string uri, string paramStr)
        {
            try
            {
                string result = string.Empty;
                HttpWebRequest wbRequest = (HttpWebRequest)WebRequest.Create(uri);
                wbRequest.Proxy = null;
                wbRequest.Method = "POST";
                wbRequest.ContentType = "application/x-www-form-urlencoded";
                wbRequest.ContentLength = Encoding.UTF8.GetByteCount(paramStr);
                if (!string.IsNullOrEmpty(_username) && !string.IsNullOrEmpty(_password))
                {
                    wbRequest.Credentials = GetCredentialCache(uri, _username, _password);
                    wbRequest.Headers.Add(_username, _password);
                }
                byte[] data = Encoding.ASCII.GetBytes(paramStr);
                using (Stream requestStream = wbRequest.GetRequestStream())
                {
                    using (StreamWriter swrite = new StreamWriter(requestStream))
                    {
                        swrite.Write(paramStr);
                    }
                }
                HttpWebResponse wbResponse = (HttpWebResponse)wbRequest.GetResponse();
                using (Stream responseStream = wbResponse.GetResponseStream())
                {
                    using (StreamReader sread = new StreamReader(responseStream))
                    {
                        result = sread.ReadToEnd();
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private static CredentialCache GetCredentialCache(string uri, string username, string password)
        {
            string authorization = string.Format("{0}:{1}", username, password);
            CredentialCache credCache = new CredentialCache();
            credCache.Add(new Uri(uri), "Basic", new NetworkCredential(username, password));
            return credCache;
        }

    }
}
