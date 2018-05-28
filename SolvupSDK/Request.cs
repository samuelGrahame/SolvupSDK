using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace SolvupSDK
{
    public class Request
    {
        private static string apiURL = @"https://repairs-api.solvup.com/";
        private static string stagingApiURL = @"https://staging-apps.solvup.com/";        

        public static string GetUrl()
        {
            return Config.InStaging ? stagingApiURL : apiURL;
        }

        public static string Get(string command = "")
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(GetUrl() + command);
                request.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                request.Method = "GET";
                SetupHeaders(request);

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (WebException e)
            {
                try
                {
                    using (WebResponse response = e.Response)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        using (Stream data = response.GetResponseStream())
                        using (var reader = new StreamReader(data))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private static void SetupHeaders(HttpWebRequest request)
        {
            string encoded = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(Config.Username + ":" + Config.Password));
            request.Headers.Add("Authorization", "Basic " + encoded);
        }

        public static string Post(string body, string command = "")
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(GetUrl() + command);
            SetupHeaders(request);

            request.Method = "POST";
            request.ContentType = "text/xml";
            if (!string.IsNullOrWhiteSpace(body))
            {
                request.ContentLength = body.Length;

                using (StreamWriter sw = new StreamWriter(request.GetRequestStream()))
                {
                    sw.Write(body);
                }
            }

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (WebException e)
            {
                using (WebResponse response = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        try
                        {
                            return reader.ReadToEnd();
                        }
                        catch (Exception)
                        {
                            return string.Empty;
                        }

                    }
                }
            }
        }

        public static string Put(string body, string command = "")
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(GetUrl() + command);
            SetupHeaders(request);

            request.Method = "PUT";
            request.ContentType = "text/xml";
            if (!string.IsNullOrWhiteSpace(body))
            {
                request.ContentLength = body.Length;

                using (StreamWriter sw = new StreamWriter(request.GetRequestStream()))
                {
                    sw.Write(body);
                }
            }

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (WebException e)
            {
                using (WebResponse response = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        try
                        {
                            return reader.ReadToEnd();
                        }
                        catch (Exception)
                        {
                            return string.Empty;
                        }

                    }
                }
            }
        }
    }
}
