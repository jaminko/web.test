using System;
using System.Net.Http;
using System.Text;

namespace DepositeCalcTests.Utilities
{
    public static class ApiHelper
    {
        public static void Delete(string name)
        {
            var clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            var client = new HttpClient(clientHandler);
            var request = new HttpRequestMessage
            {
                Content = new StringContent($"{{ \"login\": \"{name}\" }}", Encoding.UTF8, "application/json"),
                Method = HttpMethod.Delete,
                RequestUri = new Uri("https://localhost:5001/api/register/delete")
            };
            client.SendAsync(request).Wait();
        }
    }
}
