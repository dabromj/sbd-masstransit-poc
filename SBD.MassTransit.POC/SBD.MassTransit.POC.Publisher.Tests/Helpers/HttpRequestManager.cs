using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace SBD.MassTransit.POC.Publisher.Tests.Helpers
{
    /// <summary>
    /// HttpRequestManager provides an in-memory implementation of HttpServer for unit testing.
    /// </summary>
    public class HttpRequestManager
    {

        /// <summary>
        /// Handles creating server and client, as well as building and submitting request.
        /// </summary>
        /// <param name="requestUrl">string Request URL</param>
        /// <param name="authHeader">AuthenticationHeaderValue</param>
        /// <param name="httpMethod">HttpMethod</param>
        /// <param name="body">string</param>
        /// <returns>HttpResponseMessage</returns>
        public static async Task<HttpResponseMessage> SubmitRequest(string requestUrl, AuthenticationHeaderValue authHeader, HttpMethod httpMethod, string body = null)
        {
            try
            {
                HttpResponseMessage response;
                using (var server = new HttpServer(new HttpConfiguration()))
                {
                    // Use PublishAPI configuration
                    var config = server.Configuration;
                    WebApiConfig.Register(config);

                    // Client
                    var client = new HttpClient(server);
                    client.DefaultRequestHeaders.Authorization = authHeader;

                    // Request
                    var request = new HttpRequestMessage
                    {
                        RequestUri = new Uri(requestUrl),
                        Method = httpMethod,
                    };
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    if (body != null)
                        request.Content = new StringContent(body, Encoding.UTF8, "application/json");

                    // Submit request
                    response = await client.SendAsync(request, CancellationToken.None);
                }
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("HttpRequestManager.SubmitRequest: " + ex.Message);
            }
        }
    }
}
