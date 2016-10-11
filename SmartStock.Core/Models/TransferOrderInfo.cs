using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStock.Core.Models
{
    public class TransferOrderSummary
    {
        public List<TransferOrderInfo> TransferOrders { get; set; }
    }

    public class TransferOrderInfo
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string TrasferDate { get; set; }
        public decimal Amount { get; set; }
    }
}
