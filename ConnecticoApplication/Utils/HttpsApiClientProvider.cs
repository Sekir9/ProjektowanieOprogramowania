using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ConnecticoApplication.Utils
{
    public class HttpsApiClientProvider
    {
        public static HttpClient httpClient { get; private set; }

        static HttpsApiClientProvider()
        {
            httpClient = new HttpClient();
            httpClient.Timeout = new TimeSpan(0, 0, 15); //15s
            httpClient.BaseAddress = new Uri("https://localhost:44301/");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private HttpsApiClientProvider() {}
    }
}
