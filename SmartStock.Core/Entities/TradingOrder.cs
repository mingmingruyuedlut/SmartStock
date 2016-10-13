
namespace SmartStock.Core.Entities
{
    using Enums;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table(nameof(TradingOrder))]
    public class TradingOrder
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("TradingStock")]
        public int TradingStockID { get; set; }

        public virtual TradingStock TradingStock { get; set; }

        [ForeignKey("StockOperatorUser")]
        public int StockOperatorUserID { get; set; }

        public virtual User StockOperatorUser { get; set; }

        [StringLength(10)]
        public string TradingDate { get; set; }

        public TradingOrderBuySellType BuySellType { get; set; }

        [StringLength(50)]
        public string TradingCode { get; set; }

        public Nullable<decimal> TradingPrice { get; set; }

        public Nullable<int> TradingNumber { get; set; }

        public Nullable<decimal> TradingAmount { get; set; }

        public Nullable<decimal> SettleAmount { get; set; }

        public Nullable<int> OrderType { get; set; }

        public UploadResourceType ResourceType { get; set; }

        public Nullable<int> ParentID { get; set; }

    }
}
