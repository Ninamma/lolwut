using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace ConsoleApp1
{
    class Program
    {
        private static HttpListener _httpListener = new HttpListener();
        private static HttpClient client = new HttpClient();

        private static async Task<string> ProcessSomething()
        {
            var lolwut =
                client.GetStringAsync(
                    "https://api.airtable.com/v0/appcy83lIINziIdu5/Marketplace?api_key=" + Credentials.ApiKey);
            var msg = await lolwut;
            return msg;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Starting server...");
            _httpListener.Prefixes.Add("http://localhost:5000/"); // add prefix "http://localhost:5000/"
            _httpListener.Start(); // start server (Run application as Administrator!)
            Console.WriteLine("Server started.");
            Thread _responseThread = new Thread(ResponseThread);
            _responseThread.Start(); // start the response thread
        }

        static void ResponseThread()
        {
            while (true)
            {
                HttpListenerContext context = _httpListener.GetContext(); // get a context
                // Now, you'll find the request URL in context.Request.Url
                
                var msg = ProcessSomething().Result;

                byte[] _responseArray = Encoding.UTF8.GetBytes(msg); // get the bytes to response
                context.Response.OutputStream.Write(_responseArray, 0,
                    _responseArray.Length); // write bytes to the output stream
                context.Response.KeepAlive = false; // set the KeepAlive bool to false
                context.Response.Close(); // close the connection
                Console.WriteLine("Response given to a request.");
            }
        }
    }
}