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

    public enum TradingOrderBuySellType
    {
        Buy, //证券买入
        Sell //证券卖出
    }

    public class TradingOrderBuySellTypeDes
    {
        public const string TradingBuy = "证券买入";
        public const string TradingSell = "证券卖出";
        public const string Buy = "买入";
        public const string Sell = "卖出";
    }
}
