using SmartStock.Core.Context;
using SmartStock.Core.Entities;
using SmartStock.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStock.Core.DBManager
{
    public class TransferOrderManager
    {
        private SSContext _context = new SSContext();

        public TransferOrderSummary GetTransferOrderSummary()
        {
            return new TransferOrderSummary() { TransferOrders = GetAllTransferOrder() };
        }

        public TransferOrderSummary GetTransferOrderSummaryByUser(int userId)
        {
            return new TransferOrderSummary() { TransferOrders = GetAllTransferOrderByUser(userId) };
        }

        public List<TransferOrderInfo> GetAllTransferOrder()
        {
            return _context.TransferOrder.Include(nameof(User)).Select(x => new TransferOrderInfo
            {
                ID = x.ID,
                UserID = x.UserID,
                UserName = string.IsNullOrEmpty(x.User.UserName) ? x.User.LoginName : x.User.UserName,
                TrasferDate = x.TrasferDate,
                Amount = x.Amount
            }).ToList();
        }

        public List<TransferOrderInfo> GetAllTransferOrderByUser(int userId)
        {
            return _context.TransferOrder.Include(nameof(User)).Where(x => x.UserID == userId ).Select(x => new TransferOrderInfo
            {
                ID = x.ID,
                UserID = x.UserID,
                UserName = string.IsNullOrEmpty(x.User.UserName) ? x.User.LoginName : x.User.UserName,
                TrasferDate = x.TrasferDate,
                Amount = x.Amount
            }).ToList();
        }

        public void SaveTransferOrderInfo(TransferOrderInfo transferInfo)
        {
            if (_context.TransferOrder.Any(x => x.UserID == transferInfo.UserID && x.TrasferDate.Equals(transferInfo.TrasferDate)))
            {
                EditTransferOrderInfo(transferInfo);
            }
            else
            {
                AddTransferOrderInfo(transferInfo);
            }
        }

        public void AddTransferOrderInfo(TransferOrderInfo transferInfo)
        {
            TransferOrder tranfer = new TransferOrder()
            {
                UserID = transferInfo.UserID,
                TrasferDate = transferInfo.TrasferDate,
                Amount = transferInfo.Amount
            };

            _context.TransferOrder.Add(tranfer);
            _context.SaveChanges();
        }

        public void EditTransferOrderInfo(TransferOrderInfo transferInfo)
        {
            TransferOrder tranfer = _context.TransferOrder.FirstOrDefault(x => x.UserID == transferInfo.UserID && x.TrasferDate.Equals(transferInfo.TrasferDate));
            tranfer.Amount = transferInfo.Amount;

            _context.Entry(tranfer).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
