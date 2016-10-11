using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartStock.Models
{
    public class AccountViewModel
    {
    }

    public class UserViewModel
    {
        public List<UserModel> Users { get; set; }
    }

    public class UserModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPhotoUrl { get; set; }
        public string UserNickName { get; set; }
    }
}