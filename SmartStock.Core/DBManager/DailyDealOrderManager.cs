using SmartStock.Core.Context;
using SmartStock.Core.Entities;
using SmartStock.Core.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SmartStock.Core.DBManager
{
    public class DailyDealOrderManager
    {
        private SSContext _context = new SSContext();

        public string UploadDailyDealOrder(StreamReader sr, UploadResourceType urType)
        {
            string resultMsg = string.Empty;
            if (urType == UploadResourceType.TDX)
            {
                resultMsg = UploadDailyDealOrderTDX(sr);
            }
            else if (urType == UploadResourceType.THS)
            {
                resultMsg = UploadDailyDealOrderTHS(sr);
            }
            return resultMsg;
        }

        public string UploadDailyDealOrderTDX(StreamReader sr)
        {
            string resultMsg = string.Empty;
            List<TradingStock> tStockList = _context.TradingStock.ToList();
            List<User> userList = _context.User.ToList();
            string tradingLine = string.Empty; //读文件中的一行
            while ((tradingLine = sr.ReadLine()) != null)
            {
                string[] tradingFields = Regex.Split(tradingLine, @"\s\s+");
                if (Regex.IsMatch(tradingFields[0], @"^\d+$"))
                {
                    TradingStock tStock = new TradingStock()
                    {
                        StockCode = tradingFields[1],
                        StockName = tradingFields[2]
                    };
                    if (tStockList.Any(x => x.StockCode.Equals(tStock.StockCode)))
                    {
                        DailyDealOrder ddOrder = new DailyDealOrder()
                        {
                            TradingDate = DateTime.Now.ToString(DateStringFormats.yyyyMMdd) + tradingFields[0],
                            BuySellType = tradingFields[3].Equals("买") ? TradingOrderBuySellType.Buy : TradingOrderBuySellType.Sell,
                            TradingNumber = Int32.Parse(tradingFields[4]),
                            TradingPrice = decimal.Parse(tradingFields[5]),
                            TradingAmount = decimal.Parse(tradingFields[6]),
                            TradingCode = tradingFields[8],
                            OrderType = TradingOrderType.Normal
                        };
                        ddOrder.TradingStockID = tStockList.FirstOrDefault(x => x.StockCode.Equals(tStock.StockCode)).ID;

                        if (!IsDailyDealOrderExist(ddOrder))
                        {
                            _context.DailyDealOrder.Add(ddOrder);
                            _context.SaveChanges();
                            resultMsg = "success";
                        }
                    }
                }
            }
            return resultMsg;
        }

        public string UploadDailyDealOrderTHS(StreamReader sr)
        {
            string resultMsg = string.Empty;
            List<TradingStock> tStockList = _context.TradingStock.ToList();
            List<User> userList = _context.User.ToList();
            string tradingLine = string.Empty; //读文件中的一行
            while ((tradingLine = sr.ReadLine()) != null)
            {
                string[] tradingFields = Regex.Split(tradingLine, @"\t");
                if (Regex.IsMatch(tradingFields[0], @"^\d+$"))
                {
                    TradingStock tStock = new TradingStock()
                    {
                        StockCode = tradingFields[1],
                        StockName = tradingFields[2]
                    };
                    if (tStockList.Any(x => x.StockCode.Equals(tStock.StockCode)))
                    {
                        DailyDealOrder ddOrder = new DailyDealOrder()
                        {
                            TradingDate = DateTime.Now.ToString(DateStringFormats.yyyyMMdd) + tradingFields[0],
                            BuySellType = tradingFields[3].Equals("买") ? TradingOrderBuySellType.Buy : TradingOrderBuySellType.Sell,
                            TradingNumber = Int32.Parse(tradingFields[4]),
                            TradingPrice = decimal.Parse(tradingFields[5]),
                            TradingAmount = decimal.Parse(tradingFields[6]),
                            TradingCode = tradingFields[8],
                            OrderType = TradingOrderType.Normal
                        };
                        ddOrder.TradingStockID = tStockList.FirstOrDefault(x => x.StockCode.Equals(tStock.StockCode)).ID;

                        if (!IsDailyDealOrderExist(ddOrder))
                        {
                            _context.DailyDealOrder.Add(ddOrder);
                            _context.SaveChanges();
                            resultMsg = "success";
                        }
                    }
                }
            }
            return resultMsg;
        }

        private bool IsDailyDealOrderExist(DailyDealOrder ddOrder)
        {
            return _context.DailyDealOrder.Any(x => x.TradingStockID == ddOrder.TradingStockID &&
                x.TradingCode.Equals(ddOrder.TradingCode));
        }
    }
}
