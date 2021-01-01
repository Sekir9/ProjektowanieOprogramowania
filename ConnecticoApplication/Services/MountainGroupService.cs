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
    public class MountainGroupService : IMountainGroupService
    {
        public async Task<IEnumerable<MountainGroup>> GetMountainGroups()
        {
            HttpClient client = HttpsApiClientProvider.httpClient;
            HttpResponseMessage response;
            try
            {
                response = await client.GetAsync("mountaingroups");
            }
            catch
            {
                return null;
            }


            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadAsAsync<IEnumerable<MountainGroup>>();
        }

        public async Task<bool> CreateMountainGroup(MountainGroup mountainGroup)
        {
            HttpClient client = HttpsApiClientProvider.httpClient;
            HttpResponseMessage response;
            try
            {
                response = await client.PostAsJsonAsync("mountaingroup/create", mountainGroup);
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
