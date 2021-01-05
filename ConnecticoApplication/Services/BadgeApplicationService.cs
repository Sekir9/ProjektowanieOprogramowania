using ConnecticoApplication.Models;
using ConnecticoApplication.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ConnecticoApplication.Services
{
    public class BadgeApplicationService : IBadgeApplicationService
    {
        public async Task<IEnumerable<BadgeApplication>> GetBadgeApplication()
        {
            HttpClient client = HttpsApiClientProvider.httpClient;
            HttpResponseMessage response;
            try
            {
                response = await client.GetAsync("badgeapplications");
            }
            catch
            {
                return null;
            }


            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadAsAsync<IEnumerable<BadgeApplication>>();
        }

        public async Task<IEnumerable<BadgeApplication>> GetApprovedBadgeApplicationForTurist(int turistId)
        {
            HttpClient client = HttpsApiClientProvider.httpClient;
            HttpResponseMessage response;
            try
            {
                response = await client.GetAsync($"/badgeapplications/approved/{turistId}");
            }
            catch
            {
                return null;
            }


            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadAsAsync<IEnumerable<BadgeApplication>>();
        }

        public async Task<bool> UpdateBadgeApplication(BadgeApplication badgeApplication)
        {
            HttpClient client = HttpsApiClientProvider.httpClient;
            HttpResponseMessage response;
            try
            {
                response = await client.PostAsJsonAsync("badgeapplication/update", badgeApplication);
            }
            catch
            {
                return false;
            }


            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            return true;
        }
    }
}
