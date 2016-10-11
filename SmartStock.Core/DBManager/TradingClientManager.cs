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
using System.Web.Mvc;

namespace SmartStock.Core.DBManager
{
    public class TradingClientManager
    {
        private SSContext _context = new SSContext();

        public TradingClientSummary GetTradingClientSummary()
        {
            return new TradingClientSummary()
            {
                TradingClients = GetAllTradingClient(),
                Users = new AccountSettingManager().GetUsersDropDownList(RoleType.Client)
            };
        }

        public List<TradingClientInfo> GetAllTradingClient()
        {
            return _context.TradingClient.Include(nameof(User)).Select(x => new TradingClientInfo
            {
                ID = x.ID,
                UserID = x.UserID,
                LoginName = x.User.LoginName,
                StockAccountLogin = x.StockAccountLogin,
                StockAccountPassword = x.StockAccountPassword,
                OriginalAmount = x.OriginalAmount,
                StockAmount = x.StockAmount,
                AvaliableAmount = x.AvaliableAmount,
                StartTradingDate = x.StartTradingDate,
                EndTradingDate = x.EndTradingDate,
                Operator = x.Operator,
                SignDate = x.SignDate,
                ProfitPercent = x.ProfitPercent
            }).ToList();
        }

        public TradingClientSummary GetTradingClientSummaryByUser(int userId)
        {
            return new TradingClientSummary()
            {
                TradingClients = GetAllTradingClientByUser(userId),
                Users = new AccountSettingManager().GetUsersDropDownList(RoleType.Client)
            };
        }

        public List<TradingClientInfo> GetAllTradingClientByUser(int userId)
        {
            return _context.TradingClient.Include(nameof(User)).Where(x => x.UserID == userId).Select(x => new TradingClientInfo
            {
                ID = x.ID,
                UserID = x.UserID,
                LoginName = x.User.LoginName,
                StockAccountLogin = x.StockAccountLogin,
                StockAccountPassword = x.StockAccountPassword,
                OriginalAmount = x.OriginalAmount,
                StockAmount = x.StockAmount,
                AvaliableAmount = x.AvaliableAmount,
                StartTradingDate = x.StartTradingDate,
                EndTradingDate = x.EndTradingDate,
                Operator = x.Operator,
                SignDate = x.SignDate,
                ProfitPercent = x.ProfitPercent
            }).ToList();
        }

        public void SaveTradingClient(TradingClientInfo tradingClientInfo)
        {
            TradingClient client = new TradingClient()
            {
                UserID = tradingClientInfo.UserID,
                StockAccountLogin = tradingClientInfo.StockAccountLogin,
                StockAccountPassword = tradingClientInfo.StockAccountPassword,
                OriginalAmount = tradingClientInfo.OriginalAmount,
                StockAmount = tradingClientInfo.StockAmount,
                AvaliableAmount = tradingClientInfo.AvaliableAmount,
                StartTradingDate = tradingClientInfo.StartTradingDate,
                EndTradingDate = tradingClientInfo.EndTradingDate,
                SignDate = tradingClientInfo.SignDate,
                Operator = tradingClientInfo.Operator,
                ProfitPercent = tradingClientInfo.ProfitPercent
            };

            _context.TradingClient.Add(client);
            _context.SaveChanges();
        }

        public void EditTradingClient(TradingClientInfo tradingClientInfo)
        {
            TradingClient client = _context.TradingClient.Find(tradingClientInfo.ID);
            client.UserID = tradingClientInfo.UserID;
            client.StockAccountLogin = tradingClientInfo.StockAccountLogin;
            client.StockAccountPassword = tradingClientInfo.StockAccountPassword;
            client.OriginalAmount = tradingClientInfo.OriginalAmount;
            client.StockAmount = tradingClientInfo.StockAmount;
            client.AvaliableAmount = tradingClientInfo.AvaliableAmount;
            client.StartTradingDate = tradingClientInfo.StartTradingDate;
            client.EndTradingDate = tradingClientInfo.EndTradingDate;
            client.SignDate = tradingClientInfo.SignDate;
            client.Operator = tradingClientInfo.Operator;
            client.ProfitPercent = tradingClientInfo.ProfitPercent;

            _context.Entry(client).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
