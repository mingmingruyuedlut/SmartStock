using SmartStock.Core.DBManager;
using SmartStock.Core.Enums;
using SmartStock.Core.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartStock.Controllers
{
    public class BalanceController : Controller
    {
        // GET: Balance
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SplitTradingOrder()
        {
            string defaultStartDate = DateTime.Now.AddDays(-1).ToString(DateStringFormats.yyyyMMdd);
            string defaultEndDate = DateTime.Now.ToString(DateStringFormats.yyyyMMdd);

            TradingOrderSummary toSummary = new TradingOrderManager().GetTradingOrderSummary(defaultStartDate, defaultEndDate, 0);
            return View(toSummary);
        }

        public PartialViewResult ReloadSplitTradingOrderPartial(string startDate, string endDate, int userId)
        {
            string currentStartDate = DateTime.ParseExact(startDate.TrimEnd('/'), DateStringFormats.MMddyyyySlash, CultureInfo.InvariantCulture).ToString(DateStringFormats.yyyyMMdd);
            string currentEndDate = DateTime.ParseExact(endDate.TrimEnd('/'), DateStringFormats.MMddyyyySlash, CultureInfo.InvariantCulture).ToString(DateStringFormats.yyyyMMdd);
            TradingOrderSummary toSummary = new TradingOrderManager().GetTradingOrderSummary(currentStartDate, currentEndDate, userId);
            return PartialView("_SplitTradingOrderPartial", toSummary);
        }

        public JsonResult SaveSplitTradingOrder(int toId, string splitDate, int splitNumber)
        {
            string currentSplitDate = DateTime.ParseExact(splitDate.TrimEnd('/'), DateStringFormats.MMddyyyySlash, CultureInfo.InvariantCulture).ToString(DateStringFormats.yyyyMMdd);
            new TradingOrderManager().SaveSplitTradingOrder(toId, currentSplitDate, splitNumber);
            return Json("success", JsonRequestBehavior.AllowGet);
        }

        public ActionResult MergeTradingOrder()
        {
            string defaultStartDate = DateTime.Now.AddDays(-1).ToString(DateStringFormats.yyyyMMdd);
            string defaultEndDate = DateTime.Now.ToString(DateStringFormats.yyyyMMdd);

            TradingOrderSummary toSummary = new TradingOrderManager().GetSplitedTradingOrderSummary(defaultStartDate, defaultEndDate, 0);
            return View(toSummary);
        }

        public PartialViewResult ReloadMergeTradingOrderPartial(string startDate, string endDate, int userId)
        {
            string currentStartDate = DateTime.ParseExact(startDate.TrimEnd('/'), DateStringFormats.MMddyyyySlash, CultureInfo.InvariantCulture).ToString(DateStringFormats.yyyyMMdd);
            string currentEndDate = DateTime.ParseExact(endDate.TrimEnd('/'), DateStringFormats.MMddyyyySlash, CultureInfo.InvariantCulture).ToString(DateStringFormats.yyyyMMdd);
            TradingOrderSummary toSummary = new TradingOrderManager().GetSplitedTradingOrderSummary(currentStartDate, currentEndDate, userId);
            return PartialView("_MergeTradingOrderPartial", toSummary);
        }

        public JsonResult SaveMergeTradingOrder(int toId, int mergeNumber)
        {
            new TradingOrderManager().SaveMergeTradingOrder(toId, mergeNumber);
            return Json("success", JsonRequestBehavior.AllowGet);
        }
    }
}