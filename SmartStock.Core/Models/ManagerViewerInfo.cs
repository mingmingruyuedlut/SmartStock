using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SmartStock.Core.Models
{
    public class ManagerViewerSummary
    {
        public ManagerViewChart ManagerViewChartSummary { get; set; }
        public List<ManagerViewTable> ManagerViewTableSummary { get; set; }
        public List<SelectListItem> Users { get; set; }
    }

    public class ManagerViewerDetailSummary
    {
        public List<TradingOrderInfo> TradingOrders { get; set; }
        public List<SelectListItem> Users { get; set; }
    }

    /// <summary>
    /// 定义用于画 Manager 盈利曲线图的变量
    /// </summary>
    public class ManagerViewChart
    {
        public List<string> TradingDateList { get; set; }
        public List<decimal> DailyProfitList { get; set; }
    }

    /// <summary>
    /// 定义用于写 Manager 盈利表格的变量
    /// </summary>
    public class ManagerViewTable
    {
        public string TradingDate { get; set; }
        public string ClientName { get; set; }
        public decimal OperateProfit { get; set; }
        public decimal CompanyProfit { get; set; }
        public decimal TransferAmount { get; set; }
        public int UserId { get; set; }
        public bool CanTransfer { get; set; }
    }
}
