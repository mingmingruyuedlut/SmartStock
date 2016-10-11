
namespace SmartStock.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table(nameof(TradingStock))]
    public class TradingStock
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }

        public virtual User User { get; set; }

        [ForeignKey("TradingClient")]
        public int TradingClientID { get; set; }

        public virtual TradingClient TradingClient { get; set; }

        [StringLength(10)]
        public string StockCode { get; set; }

        [StringLength(10)]
        public string StockName { get; set; }

        public Nullable<int> OrignalNumber { get; set; }

        public Nullable<decimal> ClosingPrice { get; set; }

        [StringLength(50)]
        public string StockHolderCode { get; set; }

        [StringLength(50)]
        public string CashAccountNo { get; set; }
    }
}
