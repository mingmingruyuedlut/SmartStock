using SmartStock.Core.Context;
using SmartStock.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStock.Core.DBManager
{
    public class RoleManager
    {
        private SSContext _context = new SSContext();

        public List<RoleInfo> GetAllRoles()
        {
            return _context.Role.Select(x => new RoleInfo
            {
                ID = x.ID,
                RoleCode = x.RoleCode,
                Description = x.Description
            }).ToList();
        }
    }
}
