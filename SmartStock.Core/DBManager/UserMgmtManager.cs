using SmartStock.Core.Context;
using SmartStock.Core.Entities;
using SmartStock.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStock.Core.DBManager
{
    public class UserMgmtManager
    {
        public UserInfo GetUserInfo(int userId)
        {
            using (SSContext _context = new SSContext())
            {
                UserInfo currentUserInfo = new UserInfo();
                User currentUser = _context.User.Include("UserRoles").Where(x => x.ID == userId).FirstOrDefault();
                if (currentUser != null)
                {
                    currentUserInfo.ID = currentUser.ID;
                    currentUserInfo.LoginName = currentUser.LoginName;
                    currentUserInfo.DisplayName = currentUser.UserName;
                    currentUserInfo.Email = currentUser.Email;
                    currentUserInfo.Phone = currentUser.Phone;
                    currentUserInfo.Status = currentUser.Status;
                    List<RoleInfo> ris = new List<RoleInfo>();
                    foreach (UserRole ur in currentUser.UserRoles)
                    {
                        RoleInfo ri = new RoleInfo()
                        {
                            ID = ur.RoleID
                        };
                        ris.Add(ri);
                    }
                    currentUserInfo.UserRoles = ris;
                }
                return currentUserInfo;
            }
        }

        public List<UserInfo> GetUsers()
        {
            using (SSContext _context = new SSContext())
            {
                List<UserInfo> currentUserInfos = new List<UserInfo>();
                List<User> currentUsers = _context.User.Include("UserRoles").ToList();
                foreach (User u in currentUsers)
                {
                    UserInfo currentUserInfo = new UserInfo();
                    currentUserInfo.ID = u.ID;
                    currentUserInfo.LoginName = u.LoginName;
                    currentUserInfo.DisplayName = u.UserName;
                    currentUserInfo.Email = u.Email;
                    currentUserInfo.Phone = u.Phone;
                    currentUserInfo.Status = u.Status;
                    List<RoleInfo> ris = new List<RoleInfo>();
                    foreach (UserRole ur in u.UserRoles)
                    {
                        RoleInfo ri = new RoleInfo()
                        {
                            ID = ur.RoleID
                        };
                        ris.Add(ri);
                    }
                    currentUserInfo.UserRoles = ris;

                    currentUserInfos.Add(currentUserInfo);
                }
                return currentUserInfos;
            }
        }
    }
}
