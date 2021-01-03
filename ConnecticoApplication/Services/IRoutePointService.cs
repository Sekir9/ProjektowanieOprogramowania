using ConnecticoApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnecticoApplication.Services
{
    public interface IRoutePointService
    {
        Task<IEnumerable<RoutePoint>> GetRoutePoints();
        Task<bool> CreateRoutePoint(RoutePoint routePoint);
        Task<bool> EditRoutePoint(RoutePoint routePoint);
    }
}
