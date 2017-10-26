using System.Net;
using Newtonsoft.Json;

namespace IstHttpServer
{
    public class JsonList
    {

        public static string PostList(HttpListenerRequest request)
        {
            System.IO.Stream body = request.InputStream;
            System.Text.Encoding encoding = request.ContentEncoding;
            System.IO.StreamReader reader = new System.IO.StreamReader(body, encoding);
            string s = reader.ReadToEnd();

            string[] data = s.Split(',');
            var jsonFile = JsonConvert.SerializeObject(data);
            return jsonFile;
        }
        
    }
}