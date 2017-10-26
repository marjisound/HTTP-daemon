using System;
using System.Net;
using System.Threading;
using System.Text;
using System.Web;

namespace IstHttpServer
{
    // This part of the code was 
    public class HttpServer
    {
        private readonly HttpListener _listener = new HttpListener();
        private readonly Func<HttpListenerRequest, string> _responderMethod;

        public HttpServer(string[] prefixes, Func<HttpListenerRequest, string> method)
        {
            if (!HttpListener.IsSupported)
                throw new NotSupportedException("Needs Windows XP SP2, Server 2003 or later.");

            // URI prefixes are required, for example 

            if (prefixes == null || prefixes.Length == 0)
                throw new ArgumentException("prefixes");

            // A responder method is required
            if (method == null)
                throw new ArgumentException("method");

            foreach (string s in prefixes)
            {
                _listener.Prefixes.Add(s);
            }

            _responderMethod = method;

            try
            {
                _listener.Start();
            }
            catch (HttpListenerException e)
            {
                Console.WriteLine(e);
            }
            
        }

        public HttpServer(Func<HttpListenerRequest, string> method, params string[] prefixes)
            : this(prefixes, method) { }

        public void Run()
        {
            ThreadPool.QueueUserWorkItem((o) =>
            {
                Console.WriteLine("Webserver running...");
                try
                {
                    while (_listener.IsListening)
                    {
                        ThreadPool.QueueUserWorkItem((c) =>
                        {
                            var ctx = c as HttpListenerContext;
                            try
                            {
                                string rstr = _responderMethod(ctx.Request);
                                SetContent(ctx, rstr);


                            }
                            catch (HttpException e)
                            {
                                if (e.GetHttpCode() == 404)
                                {
                                    ctx.Response.StatusCode = 404;
                                    string rstr = e.Message;
                                    SetContent(ctx, rstr);
                                }
                            }
                            catch (Exception e)
                            {
                                ctx.Response.StatusCode = 500;
                                string rstr = e.Message;
                                SetContent(ctx, rstr);
                            } // suppress any exceptions
                            finally
                            {
                                // always close the stream
                                ctx.Response.OutputStream.Close();
                            }
                        }, _listener.GetContext());
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                } // suppress any exceptions
            });
        }

        private void SetContent(HttpListenerContext ctx, string rstr)
        {
            byte[] buf = Encoding.UTF8.GetBytes(rstr);
            ctx.Response.ContentLength64 = buf.Length;
            ctx.Response.OutputStream.Write(buf, 0, buf.Length);
        }

        public void Stop()
        {
            _listener.Stop();
            _listener.Close();
        }
    }
}