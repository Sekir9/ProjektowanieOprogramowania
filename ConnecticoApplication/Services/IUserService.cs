using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnecticoApplication.Services
{
    public interface IUserService
    {
        Task<bool> Login(string login, string password);
    }
}
