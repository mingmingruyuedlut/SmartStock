using SmartStock.Core.Context;
using SmartStock.Core.Entities;
using SmartStock.Core.Enums;
using SmartStock.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStock.Core.DBManager
{
    public class TradingStockManager
    {
        private SSContext _context = new SSContext();

        public TradingStockSummary GetTradingStockSummary()
        {
            return new TradingStockSummary()
            {
                TradingStocks = GetAllTradingStock(),
                Users = new AccountSettingManager().GetUsersDropDownList(RoleType.Client)
            };
        }

        public List<TradingStockInfo> GetAllTradingStock()
        {
            return _context.TradingStock.Include(nameof(User)).Select(x => new TradingStockInfo
            {
                ID = x.ID,
                UserID = x.UserID,
                LoginName = x.User.LoginName,
                TradingClientID = x.TradingClientID,
                StockCode = x.StockCode,
                StockName = x.StockName,
                OrignalNumber = x.OrignalNumber,
                ClosingPrice = x.ClosingPrice,
                StockHolderCode = x.StockHolderCode,
                CashAccountNo = x.CashAccountNo
            }).ToList();
        }

        public TradingStockSummary GetTradingStockSummaryByUser(int userId)
        {
            return new TradingStockSummary()
            {
                TradingStocks = GetAllTradingStockByUser(userId),
                Users = new AccountSettingManager().GetUsersDropDownList(RoleType.Client)
            };
        }

        public List<TradingStockInfo> GetAllTradingStockByUser(int userId)
        {
            return _context.TradingStock.Include(nameof(User)).Where(x => x.UserID == userId).Select(x => new TradingStockInfo
            {
                ID = x.ID,
                UserID = x.UserID,
                TradingClientID = x.TradingClientID,
                LoginName = x.User.LoginName,
                StockCode = x.StockCode,
                StockName = x.StockName,
                OrignalNumber = x.OrignalNumber,
                ClosingPrice = x.ClosingPrice,
                StockHolderCode = x.StockHolderCode,
                CashAccountNo = x.CashAccountNo
            }).ToList();
        }

        public void SaveTradingStock(TradingStockInfo tradingStockInfo)
        {

            TradingStock stock = new TradingStock()
            {
                UserID = tradingStockInfo.UserID,
                StockCode = tradingStockInfo.StockCode,
                StockName = tradingStockInfo.StockName,
                OrignalNumber = tradingStockInfo.OrignalNumber,
                ClosingPrice = tradingStockInfo.ClosingPrice,
                StockHolderCode = tradingStockInfo.StockHolderCode,
                CashAccountNo = tradingStockInfo.CashAccountNo
            };
            string currentDateStr = DateTime.Now.ToString(DateStringFormats.yyyyMMdd);
            stock.TradingClientID = _context.TradingClient.FirstOrDefault(x => x.UserID == tradingStockInfo.UserID && x.StartTradingDate.CompareTo(currentDateStr) <= 0 && x.EndTradingDate.CompareTo(currentDateStr) >= 0).ID;

            _context.TradingStock.Add(stock);
            _context.SaveChanges();
        }

        public void EditTradingStock(TradingStockInfo tradingStockInfo)
        {
            TradingStock stock = _context.TradingStock.Find(tradingStockInfo.ID);
            stock.UserID = tradingStockInfo.UserID;
            string currentDateStr = DateTime.Now.ToString(DateStringFormats.yyyyMMdd);
            stock.TradingClientID = _context.TradingClient.FirstOrDefault(x => x.UserID == tradingStockInfo.UserID && x.StartTradingDate.CompareTo(currentDateStr) <= 0 && x.EndTradingDate.CompareTo(currentDateStr) >= 0).ID;
            stock.StockCode = tradingStockInfo.StockCode;
            stock.StockName = tradingStockInfo.StockName;
            stock.OrignalNumber = tradingStockInfo.OrignalNumber;
            stock.ClosingPrice = tradingStockInfo.ClosingPrice;
            stock.StockHolderCode = tradingStockInfo.StockHolderCode;
            stock.CashAccountNo = tradingStockInfo.CashAccountNo;

            _context.Entry(stock).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
