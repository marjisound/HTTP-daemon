using System;

namespace IstHttpServer
{
    public class UnixTime
    {

        public static string GetTime()
        {
            return DateTimeOffset.Now.ToUnixTimeSeconds().ToString();

        }


    }
}