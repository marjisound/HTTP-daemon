using System;
using System.Linq;
using System.Net;

namespace IstHttpServer
{
    public class RandomGenerator
    {
        private int _min;
        private int _max;
        private int _count;

        public RandomGenerator(int min, int max, int count)
        {
            this._min = min;
            this._max = max;
            this._count = count;
        }

        public string GetRandom(HttpListenerRequest request)
        {
            if (request.QueryString.AllKeys.Contains("min"))
            {
                _min = Convert.ToInt32(request.QueryString["min"]);
            }

            if (request.QueryString.AllKeys.Contains("max"))
            {
                _max = Convert.ToInt32(request.QueryString["max"]);
            }

            if (request.QueryString.AllKeys.Contains("count"))
            {
                _count = Convert.ToInt32(request.QueryString["count"]);
            }

            var rand = new Random();
            var randomNumberList = new int[_count];

            for (var i = 0; i < _count; i++)
            {
                randomNumberList[i] = rand.Next(_min, _max);
            }

            string result = string.Join(",", randomNumberList);
            return result;
        }
    }
}