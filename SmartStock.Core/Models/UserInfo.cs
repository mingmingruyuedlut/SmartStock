using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStock.Core.Models
{
    public class UserInfo
    {
        public int ID { get; set; }
        public string DisplayName { get; set; }
        public string LoginName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int Status { get; set; }
        public List<RoleInfo> UserRoles { get; set; }
    }
}
