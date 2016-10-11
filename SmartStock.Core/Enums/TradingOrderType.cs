using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStock.Core.Enums
{
    public class TradingOrderType
    {
        public const int Normal = 0;
        public const int ClientOperator = 1;
        public const int BuyFault = 2;
        public const int SellFault = 3;
        public const int OtherFault = 4;
    }

    public class BuySellType
    {
        public const int Buy = 0;
        public const int Sell = 1;
    }
}
