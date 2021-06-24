using Nextflip.Models.role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Services.Interfaces
{
    public interface IRoleService
    {
        IEnumerable<Role> GetRoleNameList();
    }
}
