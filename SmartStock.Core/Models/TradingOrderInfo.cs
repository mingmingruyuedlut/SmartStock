using SmartStock.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SmartStock.Core.Models
{
    public class TradingOrderSummary
    {
        public List<TradingOrderInfo> TradingOrders { get; set; }
        public List<SelectListItem> Users { get; set; }
    }

    public class TradingOrderInfo
    {
        public int ID { get; set; }
        public string TradingDate { get; set; }
        public string StockCode { get; set; }
        public string StockName { get; set; }
        public TradingOrderBuySellType BuySellType { get; set; }
        public string BuySellTypeDescription { get; set; }
        public string TradingCode { get; set; }
        public decimal TradingPrice { get; set; }
        public int TradingNumber { get; set; }
        public decimal TradingAmount { get; set; }
        public decimal SettleAmount { get; set; }
        public string StockHolderCode { get; set; }
        public string StockOperator { get; set; }
        public int OrderType { get; set; }
        public int ParentID { get; set; }
    }

    public class TradingOrderTempInfo
    {
        public string TradingDate { get; set; }
        public int UserId { get; set; }
        public decimal TransferAmount { get; set; }
        public decimal OperateProfit { get; set; }
        public decimal CompanyProfit { get; set; }
    }

    public class ClientTradingOrderTempInfo
    {
        public string TradingDate { get; set; }
        public int UserId { get; set; }
        public decimal TransferAmount { get; set; }
        public decimal OperateProfit { get; set; }
        public decimal ClientProfit { get; set; }
        public string StockCode { get; set; }
        public string StockHolderCode { get; set; }
        public string StockName { get; set; }
    }
}
