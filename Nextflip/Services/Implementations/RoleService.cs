using Nextflip.Models.role;
using Nextflip.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Services.Implementations
{
    public class RoleService : IRoleService
    {
        private IRoleDAO _roleDao;
        public RoleService(IRoleDAO roleDao) => _roleDao = roleDao;
        public IEnumerable<Role> GetRoleNameList() => _roleDao.GetRoleNameList();
    }
}
