using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Newtonsoft.Json;

namespace IstHttpServer
{
    // This HTTP server can be run on any port that is passed as a Command line argument. Default is 5000
    class Program
    {
        //Global cache dictionary. Dictionary in C# is a hash table so searching would have a O(1) time complexity
        private static IDictionary<string, string> _cache = new Dictionary<string, string>();

        static void Main(string[] args)
        {            
            string[] _webUrl = {$"http://localhost:{args[0]}/", $"http://127.0.0.1:{args[0]}/"};

            try
            {
                HttpServer ws = new HttpServer(_webUrl, SendResponse);
                ws.Run();
                Console.WriteLine("A simple webserver. Press a key to quit.");
                Console.ReadKey();
                ws.Stop();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            

        }

        // Processes the http request and make calls based on the requested resource
        public static string SendResponse(HttpListenerRequest request)
        {
            string result = "";
            switch (request.Url.AbsolutePath)
            {
                case "/time":
                    result = HttpRequestTime(request);
                    break;
                case "/list":

                    result = HttpRequestList(request);
                    break;
                case "/random":

                    result = HttpRequestRandom(request);                                     
                    break;
                case "/prime":

                    result = HttpRequestPrime(request);
                    break;
                case "/cache":
                    result = HttpRequestCache(request);

                    break;
                default:
                    throw new HttpException(404, "Page Not Found");
            }
           
            return $"{result}";
        }

        // Time Resource call
        private static string HttpRequestTime(HttpListenerRequest request)
        {
            if (request.HttpMethod == "GET")
            {
                return UnixTime.GetTime();
            }
            throw new HttpException(404, "Page Not Found");
        }

        // List Resource call
        private static string HttpRequestList(HttpListenerRequest request)
        {
            if (request.HttpMethod == "POST")
            {
                return JsonList.PostList(request);
            }
            throw new HttpException(404, "Page Not Found");
        }

        // Random Resource call
        private static string HttpRequestRandom(HttpListenerRequest request)
        {
            if (request.HttpMethod == "GET")
            {
                RandomGenerator randomGenerator = new RandomGenerator(1, 100, 5);
                return randomGenerator.GetRandom(request);
            }
            throw new HttpException(404, "Page Not Found");
        }

        // Prime Resource call
        private static string HttpRequestPrime(HttpListenerRequest request)
        {
            Dictionary<int, bool> _primeList = new Dictionary<int, bool>();
            if (request.HttpMethod == "GET")
            {
                return PrimeNumber.GetPrime(request, _primeList);
            }
            throw new HttpException(404, "Page Not Found");
        }

        // Cache call
        private static string HttpRequestCache(HttpListenerRequest request)
        {
            string result = "";

            if (request.HttpMethod == "POST")
            {
                if (request.QueryString.AllKeys.Contains("item"))
                {
                    if (!_cache.ContainsKey(request.QueryString["item"]))
                    {
                        _cache.Add(request.QueryString["item"], "");
                        result = "item added";
                    }
                    else
                    {
                        result = "Item already existed in cache!";
                    }                   
                }

                if (request.QueryString.AllKeys.Contains("clear") && request.QueryString["clear"] == "true")
                {
                    _cache.Clear();
                }
            }
            else if (request.HttpMethod == "GET")
            {
                if (request.QueryString.AllKeys.Contains("item"))
                {
                    if (_cache.ContainsKey(request.QueryString["item"]))
                    {
                        result = "true";
                    }
                    else
                    {
                        result = "false";
                    }                 
                }else if (request.QueryString.AllKeys.Length == 0)
                {
                    var resultList = _cache.Keys;
                    var jsonList = JsonConvert.SerializeObject(resultList);
                    return jsonList;
                }
            }
            else
            {
                throw new HttpException(404, "Page Not Found");
            }

            return result;
        }
    }
}
