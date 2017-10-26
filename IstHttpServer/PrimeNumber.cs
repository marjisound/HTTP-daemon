using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace IstHttpServer
{
    public class PrimeNumber
    {
        public static string GetPrime(HttpListenerRequest request, Dictionary<int, bool> _primeList)
        {
            bool prime = true;

            if (request.QueryString.AllKeys.Contains("arg"))
            {
                try
                {
                    var arg = Convert.ToInt32(request.QueryString["arg"]);

                    if (_primeList.ContainsKey(arg))
                    {
                        prime = _primeList[arg];
                    }
                    else
                    {
                        if (arg == 2)
                        {
                            prime = false;
                        }
                        else if (arg % 2 == 0)
                        {
                            prime = false;
                        }
                        else
                        {
                            for (int i = 3; i * i <= arg; i += 2)
                            {
                                if (arg % i == 0)
                                {
                                    prime = false;
                                }
                            }
                        }

                        _primeList.Add(arg, prime);
                    }

                }
                catch (FormatException e)
                {
                    Console.WriteLine(e);
                }
                catch (OverflowException e)
                {
                    Console.WriteLine(e);
                }

            }
            return Convert.ToString(prime);

        }
    }
}