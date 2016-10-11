using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SmartStock.Core.Models
{

    public class TradingStockSummary
    {
        public List<TradingStockInfo> TradingStocks { get; set; }
        public List<SelectListItem> Users { get; set; }
    }
    public class TradingStockInfo
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string LoginName { get; set; }
        public int TradingClientID { get; set; }
        public string StockCode { get; set; }
        public string StockName { get; set; }
        public Nullable<int> OrignalNumber { get; set; }
        public Nullable<decimal> ClosingPrice { get; set; }
        public string StockHolderCode { get; set; }
        public string CashAccountNo { get; set; }
    }
}
