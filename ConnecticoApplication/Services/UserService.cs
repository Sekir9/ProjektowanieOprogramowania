using ConnecticoApplication.Utils;
using ConnecticoApplication.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace ConnecticoApplication.Services
{
    public class UserService : IUserService
    {
        public async Task<bool> Login(string login, string password)
        {
            AuthenticateRequest request = new AuthenticateRequest() { Login = login, Password = password };

            HttpClient client = HttpsApiClientProvider.httpClient;
            HttpResponseMessage response;
            try
            {
                response = await client.PostAsJsonAsync("authenticate", request);
            }
            catch
            {
                return false;
            }


            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            AuthenticateResponse authenticateResponse = await response.Content.ReadAsAsync<AuthenticateResponse>();

            //set token to httpClient
            HttpsApiClientProvider.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticateResponse.Token);

            return true;
        }
    }
}
