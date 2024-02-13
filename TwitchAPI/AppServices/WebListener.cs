using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TwitchAPI.AppServices
{
    internal class WebListener
    {
        internal string Listener(string baseUrl)
        {
            string prefix;
            if (baseUrl.EndsWith('/'))
            {
                prefix = baseUrl;
            }
            else
            {
                    prefix = baseUrl + "/"; 
            }
            // URI prefixes are required,
            // for example "http://contoso.com:8080/index/".
            if (prefix == null || prefix.Length == 0)
                throw new ArgumentException("prefixes");
            // Create a listener.
            HttpListener listener = new HttpListener();
            // Add the prefixes.
            listener.Prefixes.Add(prefix);
            listener.Start();
            // Note: The GetContext method blocks while waiting for a request.
            HttpListenerContext context = listener.GetContext();
            HttpListenerRequest request = context.Request;
            // You must close the output stream
            listener.Stop();
            return request.RawUrl;
        }

        internal string Listener(string baseUrl,string redirect)
        {
            string prefix;
            if (baseUrl.EndsWith('/'))
            {
                prefix = baseUrl;
            }
            else
            {
                prefix = baseUrl + "/";
            }
            // URI prefixes are required,
            // for example "http://contoso.com:8080/index/".
            if (prefix == null || prefix.Length == 0)
                throw new ArgumentException("prefixes");
            // Create a listener.
            HttpListener listener = new HttpListener();
            // Add the prefixes.
            listener.Prefixes.Add(prefix);
            listener.Start();
            // Note: The GetContext method blocks while waiting for a request.
            HttpListenerContext context = listener.GetContext();
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;
            response.Redirect(redirect);
            // You must close the output stream
            listener.Stop();
            return request.RawUrl;
        }
    }
}
