using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Models.role
{
    public interface IRoleDAO
    {
        IEnumerable<Role> GetRoleNameList();
    }
}
