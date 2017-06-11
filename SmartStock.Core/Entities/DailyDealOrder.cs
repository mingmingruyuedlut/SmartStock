
namespace SmartStock.Core.Entities
{
    using Enums;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table(nameof(DailyDealOrder))]
    public class DailyDealOrder
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("TradingStock")]
        public int TradingStockID { get; set; }

        public virtual TradingStock TradingStock { get; set; }

        [ForeignKey("StockOperatorUser")]
        public Nullable<int> StockOperatorUserID { get; set; }

        public virtual User StockOperatorUser { get; set; }

        [StringLength(20)]
        public string TradingDate { get; set; }

        public TradingOrderBuySellType BuySellType { get; set; }

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

        public Nullable<int> OrderType { get; set; }
        public UploadResourceType ResourceType { get; set; }
    }
}
