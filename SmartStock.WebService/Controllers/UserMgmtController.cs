using SmartStock.Core.DBManager;
using SmartStock.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SmartStock.WebService.Controllers
{
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/UserMgmt")]
    public class UserMgmtController : ApiController
    {
        [HttpGet]
        [Route("UserInfo/{userId}")]
        public ResponseResult GetUserInfo(int userId)
        {
            ResponseResult result = null;

            try
            {
                var info = new UserMgmtManager().GetUserInfo(userId);

                var data = new
                {
                    UserInfo = info
                };

                result = new ResponseResult(true, null, data);
            }
            catch (Exception ex)
            {
                //Log
                result = new ResponseResult(false, ex.Message);
            }

            return result;
        }

        [HttpGet]
        [Route("AllUsers")]
        public ResponseResult GetAllUsers()
        {
            ResponseResult result = null;

            try
            {
                var info = new UserMgmtManager().GetUsers();

                var data = new
                {
                    Users = info
                };

                result = new ResponseResult(true, null, data);
            }
            catch (Exception ex)
            {
                //Log
                result = new ResponseResult(false, ex.Message);
            }

            return result;
        }

        //[HttpPost]
        //public ResponseResult UpdateUserInfo(dynamic user)
        //{

        //}
    }
}