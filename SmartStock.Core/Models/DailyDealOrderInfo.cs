using SmartStock.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStock.Core.Models
{
    public class DailyDealOrderSummary
    {
        public List<DailyDealOrderInfo> AllDailyDealOrders { get; set; }
        public List<DailyDealOrderInfo> MyDailyDealOrders { get; set; }
    }

    public class DailyDealOrderInfo
    {
        public int ID { get; set; }
        public string StockCode { get; set; }
        public string StockName { get; set; }
        public string StockOperatorUserName { get; set; }
        public string TradingDate { get; set; }
        public TradingOrderBuySellType BuySellType { get; set; }
        public string BuySellTypeDescription { get; set; }
        /// <summary>
        /// 成交编号，唯一标识
        /// </summary>
        [StringLength(50)]
        public string TradingCode { get; set; }
        /// <summary>
        /// 成交均价
        /// </summary>
        public Nullable<decimal> TradingPrice { get; set; }
        /// <summary>
        /// 成交数量
        /// </summary>
        public Nullable<int> TradingNumber { get; set; }
        /// <summary>
        /// 成交金额
        /// </summary>
        public Nullable<decimal> TradingAmount { get; set; }
    }
}
