using Newtonsoft.Json;
using SmartStock.Core.DBManager;
using SmartStock.Core.Enums;
using SmartStock.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SmartStock.Controllers
{
    public class StockController : Controller
    {
        // GET: Stock
        public ActionResult Index()
        {
            List<TradingStockInfo> stockInfoList = new TradingStockManager().GetAllTradingStock();
            return View(stockInfoList);
        }

        public ActionResult TradingStock()
        {
            LoginUser loginUser = (LoginUser)Session["CurrentLoginUser"];
            TradingStockSummary tsSummary = new TradingStockSummary();
            if (loginUser.RoleType == RoleType.Manager)
            {
                tsSummary = new TradingStockManager().GetTradingStockSummary();
            }
            else
            {
                tsSummary = new TradingStockManager().GetTradingStockSummaryByUser(loginUser.UserId);
            }
            return View(tsSummary);
        }

        public PartialViewResult ReloadTradingStock()
        {
            LoginUser loginUser = (LoginUser)Session["CurrentLoginUser"];
            List<TradingStockInfo> stockInfoList = new List<TradingStockInfo>();
            if (loginUser.RoleType == RoleType.Manager)
            {
                stockInfoList = new TradingStockManager().GetAllTradingStock();
            }
            else
            {
                stockInfoList = new TradingStockManager().GetAllTradingStockByUser(loginUser.UserId);
            }
            return PartialView("_TradingStockPartial", stockInfoList);
        }

        public JsonResult SaveTradingStock(string tradingStockJsonStr, bool isEdit)
        {
            TradingStockInfo tradingStockInfo = JsonConvert.DeserializeObject<TradingStockInfo>(tradingStockJsonStr);
            //notificationInfo.NotificateTime = notificationInfo.NotificateTime.ToLocalTime(); // convert UAT to Local Time because of JSON.stringify convert the datetime object to UAT
            if (isEdit)
            {
                new TradingStockManager().EditTradingStock(tradingStockInfo);
            }
            else
            {
                new TradingStockManager().SaveTradingStock(tradingStockInfo);
            }
            return Json("success", JsonRequestBehavior.AllowGet);
        }

        public ActionResult TradingClient()
        {
            LoginUser loginUser = (LoginUser)Session["CurrentLoginUser"];
            TradingClientSummary tcSummary = new TradingClientSummary();
            if (loginUser.RoleType == RoleType.Manager)
            {
                tcSummary = new TradingClientManager().GetTradingClientSummary();
            }
            else
            {
                tcSummary = new TradingClientManager().GetTradingClientSummaryByUser(loginUser.UserId);
            }
            return View(tcSummary);
        }

        public PartialViewResult ReloadTradingClient()
        {
            LoginUser loginUser = (LoginUser)Session["CurrentLoginUser"];
            List<TradingClientInfo> clientInfoList = new List<TradingClientInfo>();
            if (loginUser.RoleType == RoleType.Manager)
            {
                clientInfoList = new TradingClientManager().GetAllTradingClient();
            }
            else
            {
                clientInfoList = new TradingClientManager().GetAllTradingClientByUser(loginUser.UserId);
            }
            return PartialView("_TradingClientPartial", clientInfoList);
        }

        public JsonResult SaveTradingClient(string tradingClientJsonStr, bool isEdit)
        {
            TradingClientInfo tradingClientInfo = JsonConvert.DeserializeObject<TradingClientInfo>(tradingClientJsonStr);
            //notificationInfo.NotificateTime = notificationInfo.NotificateTime.ToLocalTime(); // convert UAT to Local Time because of JSON.stringify convert the datetime object to UAT
            if (isEdit)
            {
                new TradingClientManager().EditTradingClient(tradingClientInfo);
            }
            else
            {
                new TradingClientManager().SaveTradingClient(tradingClientInfo);
            }
            return Json("success", JsonRequestBehavior.AllowGet);
        }

        public ActionResult UploadTradingOrder()
        {
            return View();
        }

        public JsonResult UploadTradingOrderDataFile()
        {
            //using (var fileStream = Request.Files[0].InputStream)
            //{
            //    int fileLength = (int)fileStream.Length;
            //    fileStream.Position = 0;
            //    byte[] fileArray = new byte[fileLength];
            //    fileStream.Read(fileArray, 0, fileLength);
            //}

            string uResult = "success";
            LoginUser loginUser = (LoginUser)Session["CurrentLoginUser"];
            UploadResourceType urType = (UploadResourceType)Enum.Parse(typeof(UploadResourceType), Request.Form["resourcetype"]);
            using (StreamReader sr = new StreamReader(Request.Files[0].InputStream, Encoding.GetEncoding("gb2312")))
            {
                uResult = new TradingOrderManager().UploadTradingOrder(sr, urType, loginUser.UserId);
            }
            return Json(uResult, JsonRequestBehavior.AllowGet);
        }
    }
}