using Common.Helpers;
using System;
using System.Net;
using System.Text;
using System.Threading;

namespace SimpleWebServer
{
    public class WebServer
    {
        private readonly HttpListener listener;
        private readonly Func<HttpListenerRequest, string> responderMethod;
        private bool isConnectionOpen;

        public WebServer(Func<HttpListenerRequest, string> method, params string[] prefixes)
        {
            if (!HttpListener.IsSupported)
            {
                throw new NotSupportedException("Needs windows XP SP2 or newer!");
            }

            prefixes.ThrowIfNull(nameof(prefixes));
            method.ThrowIfNull(nameof(method));

            isConnectionOpen = false;

            listener = new HttpListener();
            responderMethod = method;

            foreach (string s in prefixes)
            {
                listener.Prefixes.Add(s);
            }

            listener.Start();
        }

        public void Run()
        {
            isConnectionOpen = true;
            ThreadPool.QueueUserWorkItem((o) =>
            {
                BotLogger.Logger.Log(Common.Models.LoggingType.Info, "Webserver running...");
                try
                {
                    if (!isConnectionOpen)
                    {
                        return;
                    }
                    while (listener.IsListening)
                    {
                        ThreadPool.QueueUserWorkItem((c) =>
                        {
                            var ctx = c as HttpListenerContext;
                            try
                            {
                                string rstr = responderMethod(ctx.Request);
                                byte[] buf = Encoding.UTF8.GetBytes(rstr);
                                ctx.Response.ContentLength64 = buf.Length;
                                ctx.Response.OutputStream.Write(buf, 0, buf.Length);
                            }
                            catch (Exception ex)
                            {
                                BotLogger.Logger.Log(Common.Models.LoggingType.Warning, ex);
                            }
                            finally
                            {
                                // always close the stream
                                ctx.Response.OutputStream.Close();
                            }
                        }, 
                        listener.GetContext());
                    }
                }
                catch(Exception e)
                {
                    BotLogger.Logger.Log(Common.Models.LoggingType.Warning, e);
                } 
            });
        }

        public void Stop()
        {
            isConnectionOpen = false;
            listener.Stop();
            listener.Close();
        }
    }
}
