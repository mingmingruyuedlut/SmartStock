using Newtonsoft.Json;
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
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            LoginUser loginUser = (LoginUser)Session["CurrentLoginUser"];
            if (loginUser.RoleType == RoleType.Operator)
            {
                return RedirectToAction("OperatorDashboard");
            }
            else if (loginUser.RoleType == RoleType.Manager)
            {
                return RedirectToAction("ManagerDashboard");
            }
            else
            {
                return RedirectToAction("ClientDashboard");
            }
        }

        public ActionResult ClientDashboard()
        {
            int loginUserId = ((LoginUser)Session["CurrentLoginUser"]).UserId;
            string defaultStartDate = DateTime.Now.AddDays(-30).ToString(DateStringFormats.yyyyMMdd);
            string defaultEndDate = DateTime.Now.ToString(DateStringFormats.yyyyMMdd);
            ClientViewerSummary cvSummary = new DashboardManager().GetClientViewerSummary(loginUserId, defaultStartDate, defaultEndDate);
            return View(cvSummary);
        }

        public ActionResult OperatorDashboard()
        {
            int loginUserId = ((LoginUser)Session["CurrentLoginUser"]).UserId;
            string defaultStartDate = DateTime.Now.AddDays(-30).ToString(DateStringFormats.yyyyMMdd);
            string defaultEndDate = DateTime.Now.ToString(DateStringFormats.yyyyMMdd);
            OperatorViewerSummary ovSummary = new DashboardManager().GetOperatorViewerSummary(loginUserId, defaultStartDate, defaultEndDate);
            return View(ovSummary);
        }

        public ActionResult ManagerDashboard()
        {
            string defaultStartDate = DateTime.Now.AddDays(-30).ToString(DateStringFormats.yyyyMMdd);
            string defaultEndDate = DateTime.Now.ToString(DateStringFormats.yyyyMMdd);
            ManagerViewerSummary mvSummary = new DashboardManager().GetManagerViewerSummary(defaultStartDate, defaultEndDate, 0);
            return View(mvSummary);
        }

        public ActionResult ManagerDashboardDetail()
        {
            string defaultStartDate = DateTime.Now.AddDays(-1).ToString(DateStringFormats.yyyyMMdd);
            string defaultEndDate = DateTime.Now.ToString(DateStringFormats.yyyyMMdd);
            ManagerViewerDetailSummary mvdSummary = new DashboardManager().GetManagerViewerDetailSummary(defaultStartDate, defaultEndDate, 0);
            return View(mvdSummary);
        }

        public PartialViewResult ReloadClientDashboardPartial(string startDate, string endDate)
        {
            int loginUserId = ((LoginUser)Session["CurrentLoginUser"]).UserId;
            string currentStartDate = DateTime.ParseExact(startDate.TrimEnd('/'), DateStringFormats.MMddyyyySlash, CultureInfo.InvariantCulture).ToString(DateStringFormats.yyyyMMdd);
            string currentEndDate = DateTime.ParseExact(endDate.TrimEnd('/'), DateStringFormats.MMddyyyySlash, CultureInfo.InvariantCulture).ToString(DateStringFormats.yyyyMMdd);
            ClientViewerSummary cvSummary = new DashboardManager().GetClientViewerSummary(loginUserId, currentStartDate, currentEndDate);
            return PartialView("_ClientDashboardPartial", cvSummary);
        }

        public JsonResult ReloadClientDashboardLineChart(string startDate, string endDate)
        {
            int loginUserId = ((LoginUser)Session["CurrentLoginUser"]).UserId;
            string currentStartDate = DateTime.ParseExact(startDate.TrimEnd('/'), DateStringFormats.MMddyyyySlash, CultureInfo.InvariantCulture).ToString(DateStringFormats.yyyyMMdd);
            string currentEndDate = DateTime.ParseExact(endDate.TrimEnd('/'), DateStringFormats.MMddyyyySlash, CultureInfo.InvariantCulture).ToString(DateStringFormats.yyyyMMdd);
            ClientViewChart cvChart = new DashboardManager().GetClientViewChart(loginUserId, currentStartDate, currentEndDate);
            return Json(cvChart, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult ReloadOperatorDashboardPartial(string startDate, string endDate)
        {
            int loginUserId = ((LoginUser)Session["CurrentLoginUser"]).UserId;
            string currentStartDate = DateTime.ParseExact(startDate.TrimEnd('/'), DateStringFormats.MMddyyyySlash, CultureInfo.InvariantCulture).ToString(DateStringFormats.yyyyMMdd);
            string currentEndDate = DateTime.ParseExact(endDate.TrimEnd('/'), DateStringFormats.MMddyyyySlash, CultureInfo.InvariantCulture).ToString(DateStringFormats.yyyyMMdd);
            OperatorViewerSummary ovSummary = new DashboardManager().GetOperatorViewerSummary(loginUserId, currentStartDate, currentEndDate);
            return PartialView("_OperatorDashboardPartial", ovSummary);
        }

        public JsonResult ReloadOperatorDashboardLineChart(string startDate, string endDate)
        {
            int loginUserId = ((LoginUser)Session["CurrentLoginUser"]).UserId;
            string currentStartDate = DateTime.ParseExact(startDate.TrimEnd('/'), DateStringFormats.MMddyyyySlash, CultureInfo.InvariantCulture).ToString(DateStringFormats.yyyyMMdd);
            string currentEndDate = DateTime.ParseExact(endDate.TrimEnd('/'), DateStringFormats.MMddyyyySlash, CultureInfo.InvariantCulture).ToString(DateStringFormats.yyyyMMdd);
            OperatorViewChart ovChart = new DashboardManager().GetOperatorViewChart(loginUserId, currentStartDate, currentEndDate);
            return Json(ovChart, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ReloadManagerDashboardLineChart(string startDate, string endDate, int userId)
        {
            string currentStartDate = DateTime.ParseExact(startDate.TrimEnd('/'), DateStringFormats.MMddyyyySlash, CultureInfo.InvariantCulture).ToString(DateStringFormats.yyyyMMdd);
            string currentEndDate = DateTime.ParseExact(endDate.TrimEnd('/'), DateStringFormats.MMddyyyySlash, CultureInfo.InvariantCulture).ToString(DateStringFormats.yyyyMMdd);
            ManagerViewChart mvChart = new DashboardManager().GetManagerViewChart(currentStartDate, currentEndDate, userId);
            return Json(mvChart, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult ReloadManagerDashboardPartial(string startDate, string endDate, int userId)
        {
            string currentStartDate = DateTime.ParseExact(startDate.TrimEnd('/'), DateStringFormats.MMddyyyySlash, CultureInfo.InvariantCulture).ToString(DateStringFormats.yyyyMMdd);
            string currentEndDate = DateTime.ParseExact(endDate.TrimEnd('/'), DateStringFormats.MMddyyyySlash, CultureInfo.InvariantCulture).ToString(DateStringFormats.yyyyMMdd);
            ManagerViewerSummary mvSummary = new DashboardManager().GetManagerViewerSummary(currentStartDate, currentEndDate, userId);
            return PartialView("_ManagerDashboardPartial", mvSummary);
        }

        public PartialViewResult ReloadManagerDashboardDetailPartial(string startDate, string endDate, int userId)
        {
            string currentStartDate = DateTime.ParseExact(startDate.TrimEnd('/'), DateStringFormats.MMddyyyySlash, CultureInfo.InvariantCulture).ToString(DateStringFormats.yyyyMMdd);
            string currentEndDate = DateTime.ParseExact(endDate.TrimEnd('/'), DateStringFormats.MMddyyyySlash, CultureInfo.InvariantCulture).ToString(DateStringFormats.yyyyMMdd);
            ManagerViewerDetailSummary mvdSummary = new DashboardManager().GetManagerViewerDetailSummary(currentStartDate, currentEndDate, userId);
            return PartialView("_ManagerDashboardDetailPartial", mvdSummary);
        }

        public JsonResult saveTradingOrderColumnValue(int id, string columnId, int value)
        {
            TradingOrderColumn column = (TradingOrderColumn)Enum.Parse(typeof(TradingOrderColumn), columnId);
            new DashboardManager().SaveTradingOrderColumnValue(id, column, value);
            return Json("success", JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveTransferOrder(string transferOrderJsonStr)
        {
            TransferOrderInfo transferOrderInfo = JsonConvert.DeserializeObject<TransferOrderInfo>(transferOrderJsonStr);
            new TransferOrderManager().SaveTransferOrderInfo(transferOrderInfo);
            return Json("success", JsonRequestBehavior.AllowGet);
        }

        public ActionResult TransferOrderDashboard()
        {
            TransferOrderSummary toSummary = new TransferOrderManager().GetTransferOrderSummary();
            return View(toSummary);
        }

        public ActionResult TransferOrderDashboardForClient()
        {
            int loginUserId = ((LoginUser)Session["CurrentLoginUser"]).UserId;
            TransferOrderSummary toSummary = new TransferOrderManager().GetTransferOrderSummaryByUser(loginUserId);
            return View("TransferOrderDashboard", toSummary);
        }

        public ActionResult EmployeePerformanceDashboard()
        {
            return View();
        }
    }
}