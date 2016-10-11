
namespace SmartStock.Core.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table(nameof(TransferOrder))]
    public partial class TransferOrder
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }

        public virtual User User { get; set; }

        public string TrasferDate { get; set; }

        public decimal Amount { get; set; }
    }
}
