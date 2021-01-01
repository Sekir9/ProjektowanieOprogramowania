using ConnecticoApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnecticoApplication.Services
{
    public interface IMountainGroupService
    {
        Task<IEnumerable<MountainGroup>> GetMountainGroups();
        Task<bool> CreateMountainGroup(MountainGroup mountainGroup);
    }
}
