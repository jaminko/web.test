using System;
using System.Net.Http;
using System.Text;

namespace DepositeCalcTests.Utilities
{
    internal class ApiHelper
    {
        public void Delete(string name)
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage
            {
                Content = new StringContent($"{{ \"login\": \"{name}\" }}", Encoding.UTF8, "application/json"),
                Method = HttpMethod.Delete,
                RequestUri = new Uri("https://localhost:5001/api/register/delete")
            };
            client.SendAsync(request).Wait();
        }
    }
}
