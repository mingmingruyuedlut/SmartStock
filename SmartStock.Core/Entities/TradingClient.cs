using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStock.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table(nameof(TradingClient))]
    public class TradingClient
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }

        public virtual User User { get; set; }

        [StringLength(50)]
        public string StockAccountLogin { get; set; }

        [StringLength(50)]
        public string StockAccountPassword { get; set; }

        public Nullable<decimal> OriginalAmount { get; set; }

        public Nullable<decimal> StockAmount { get; set; }

        public Nullable<decimal> AvaliableAmount { get; set; }

        [StringLength(10)]
        public string StartTradingDate { get; set; }

        [StringLength(10)]
        public string EndTradingDate { get; set; }

        [StringLength(50)]
        public string Operator { get; set; }

        [StringLength(10)]
        public string SignDate { get; set; }

        public Nullable<decimal> ProfitPercent { get; set; }
    }
}
