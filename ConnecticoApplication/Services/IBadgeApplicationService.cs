using ConnecticoApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnecticoApplication.Services
{
    public interface IBadgeApplicationService
    {
        Task<IEnumerable<BadgeApplication>> GetBadgeApplication();

        Task<IEnumerable<BadgeApplication>> GetApprovedBadgeApplicationForTurist(int turistId);

        Task<bool> UpdateBadgeApplication(BadgeApplication badgeApplication);
    }
}
