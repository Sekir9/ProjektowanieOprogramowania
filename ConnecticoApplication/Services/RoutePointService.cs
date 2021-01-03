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
    public class RoutePointService : IRoutePointService
    {
        public async Task<bool> CreateRoutePoint(RoutePoint routePoint)
        {
            HttpClient client = HttpsApiClientProvider.httpClient;
            HttpResponseMessage response;
            try
            {
                response = await client.PostAsJsonAsync("routepoint/create", routePoint);
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

        public async Task<bool> EditRoutePoint(RoutePoint routePoint)
        {
            HttpClient client = HttpsApiClientProvider.httpClient;
            HttpResponseMessage response;
            try
            {
                response = await client.PostAsJsonAsync("routepoint/edit", routePoint);
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

        public async Task<IEnumerable<RoutePoint>> GetRoutePoints()
        {
            HttpClient client = HttpsApiClientProvider.httpClient;
            HttpResponseMessage response;
            try
            {
                response = await client.GetAsync("routepoints");
            }
            catch
            {
                return null;
            }


            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadAsAsync<IEnumerable<RoutePoint>>();
        }
    }
}
