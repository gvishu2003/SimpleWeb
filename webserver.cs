using System;
using System.Net;
using System.Threading;
using System.Linq;
using System.Text;

namespace SimpleWebServer
{
    public class WebServer
    {
        private readonly HttpListener _listener = new HttpListener();

        public WebServer()
        {
            if (!HttpListener.IsSupported)
                throw new NotSupportedException("Needs Windows XP SP2, Server 2003 or later.");

            _listener.Prefixes.Add("http://*:8080/");
        }

        public void start()
        {
            _listener.Start();
            for (;;)
            {
                HttpListenerContext ctx = _listener.GetContext();
                new Thread(new Worker(ctx).ProcessRequest).Start();
            }
        }

        public void Stop()
        {
            _listener.Stop();
            _listener.Close();
        }

        static public void Main()
        {
            WebServer webServer = new WebServer();
            webServer.start();
        }
    }

    public class Worker
    {
        private HttpListenerContext context;

        public Worker(HttpListenerContext context)
        {
            this.context = context;
        }

        public void ProcessRequest()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("&#60;html&#62;&#60;body&#62;&#60;h1&#62;Hello World from C# and Distelli&#60;/h1&#62;");
            byte[] b = Encoding.UTF8.GetBytes(sb.ToString());
            context.Response.ContentLength64 = b.Length;
            context.Response.OutputStream.Write(b, 0, b.Length);
            context.Response.OutputStream.Close();
        }
    }
}