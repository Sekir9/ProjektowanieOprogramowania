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
    }
}
