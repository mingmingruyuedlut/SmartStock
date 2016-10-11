using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SmartStock.Core.Models
{
    public class TradingClientSummary
    {
        public List<TradingClientInfo> TradingClients { get; set; }
        public List<SelectListItem> Users { get; set; }
    }

    public class TradingClientInfo
    {
        public int ID { get; set; }
        public string LoginName { get; set; }
        public int UserID { get; set; }
        public string StockAccountLogin { get; set; }
        public string StockAccountPassword { get; set; }
        public Nullable<decimal> OriginalAmount { get; set; }
        public Nullable<decimal> StockAmount { get; set; }
        public Nullable<decimal> AvaliableAmount { get; set; }
        public string StartTradingDate { get; set; }
        public string EndTradingDate { get; set; }
        public string Operator { get; set; }
        public string SignDate { get; set; }
        public Nullable<decimal> ProfitPercent { get; set; }
    }
}
