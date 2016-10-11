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
    public class DashboardManager
    {
        private SSContext _context = new SSContext();

        public ClientViewerSummary GetClientViewerSummary(int userId, string staTradingDate, string endTradingDate)
        {
            return new ClientViewerSummary()
            {
                ClientViewChartSummary = GetClientViewChart(userId, staTradingDate, endTradingDate),
                ClientViewTableSummary = GetClientViewTable(userId, staTradingDate, endTradingDate)
            };
        }

        public ClientViewChart GetClientViewChart(int userId, string staTradingDate, string endTradingDate)
        {
            List<ClientTradingOrderTempInfo> tradingOrders = _context.TradingOrder
                .Include(nameof(TradingStock))
                .Where(x => x.TradingDate.CompareTo(staTradingDate) >= 0 && x.TradingDate.CompareTo(endTradingDate) <= 0 && x.OrderType == TradingOrderType.Normal && x.TradingStock.UserID == userId)
                .GroupBy(x => new
                {
                    x.TradingDate
                })
                .Select(g => new ClientTradingOrderTempInfo
                {
                    TradingDate = g.Key.TradingDate,
                    OperateProfit = g.Sum(x => x.SettleAmount).Value
                }).ToList();

            foreach (ClientTradingOrderTempInfo order in tradingOrders)
            {
                if (order.OperateProfit >= 0)
                {
                    decimal profitPercent = Math.Round(_context.TradingClient.Where(x => x.UserID == userId).FirstOrDefault().ProfitPercent.Value / 100, 2);
                    order.ClientProfit = Math.Round(order.OperateProfit * profitPercent, 3);
                }
                else
                {
                    List<TransferOrder> transferOrderList = _context.TransferOrder.Where(x => x.UserID == userId).ToList();
                    order.ClientProfit = order.OperateProfit;
                    if (transferOrderList.Any(x => x.TrasferDate.Equals(order.TradingDate)))
                    {
                        order.TransferAmount = transferOrderList.Where(x => x.TrasferDate.Equals(order.TradingDate)).Sum(x => x.Amount);
                        order.ClientProfit += order.TransferAmount;
                    }
                }
                
            }

            return new ClientViewChart()
            {
                TradingDateList = tradingOrders.Select(x => x.TradingDate).ToList(),
                DailyProfitList = tradingOrders.Select(x => x.ClientProfit).ToList()
            };
        }

        public List<ClientViewTable> GetClientViewTable(int userId, string staTradingDate, string endTradingDate)
        {
            List<ClientViewTable> clientViewList = new List<ClientViewTable>();
            List<ClientTradingOrderTempInfo> tradingOrders = _context.TradingOrder
                .Include(nameof(TradingStock))
                .Where(x => x.TradingDate.CompareTo(staTradingDate) >= 0 && x.TradingDate.CompareTo(endTradingDate) <= 0 && x.OrderType == TradingOrderType.Normal && x.TradingStock.UserID == userId)
                .GroupBy(x => new
                {
                    x.TradingDate,
                    x.StockHolderCode,
                    x.StockCode
                })
                .Select(g => new ClientTradingOrderTempInfo
                {
                    TradingDate = g.Key.TradingDate,
                    StockCode = g.Key.StockCode,
                    StockHolderCode = g.Key.StockHolderCode,
                    StockName = g.Max(x => x.StockName),
                    OperateProfit = g.Sum(x => x.SettleAmount).Value
                }).ToList();

            foreach (ClientTradingOrderTempInfo order in tradingOrders)
            {
                if (order.OperateProfit >= 0)
                {
                    decimal profitPercent = Math.Round(_context.TradingClient.Where(x => x.UserID == userId).FirstOrDefault().ProfitPercent.Value / 100, 2);
                    order.ClientProfit = Math.Round(order.OperateProfit * profitPercent, 3);
                }
                else
                {
                    List<TransferOrder> transferOrderList = _context.TransferOrder.Where(x => x.UserID == userId).ToList();
                    order.ClientProfit = order.OperateProfit;
                    if (transferOrderList.Any(x => x.TrasferDate.Equals(order.TradingDate)))
                    {
                        order.TransferAmount = transferOrderList.Where(x => x.TrasferDate.Equals(order.TradingDate)).Sum(x => x.Amount);
                        order.ClientProfit += order.TransferAmount;
                    }
                }

                clientViewList.Add(new ClientViewTable()
                {
                    TradingDate = order.TradingDate,
                    StockCode = order.StockCode,
                    StockName = order.StockName,
                    ProfitPerStock = order.ClientProfit
                });
            }

            return clientViewList;
        }

        public OperatorViewerSummary GetOperatorViewerSummary(int userId, string staTradingDate, string endTradingDate)
        {
            return new OperatorViewerSummary()
            {
                OperatorViewChartSummary = GetOperatorViewChart(userId, staTradingDate, endTradingDate),
                OperatorViewerTableSummary = GetOperatorViewTable(userId, staTradingDate, endTradingDate)
            };
        }

        public OperatorViewChart GetOperatorViewChart(int userId, string staTradingDate, string endTradingDate)
        {
            var tradingOrders = _context.TradingOrder
                .Where(x => x.TradingDate.CompareTo(staTradingDate) >= 0 && x.TradingDate.CompareTo(endTradingDate) <= 0 && x.OrderType == TradingOrderType.Normal && x.StockOperatorUserID == userId)
                .GroupBy(x => new
                {
                    x.TradingDate
                })
                .Select(g => new
                {
                    TradingDate = g.Key.TradingDate,
                    SumSettledAmount = g.Sum(x => x.SettleAmount)
                }).ToList();

            return new OperatorViewChart() { TradingDateList = tradingOrders.Select(x => x.TradingDate).ToList(), DailyProfitList = tradingOrders.Select(x => x.SumSettledAmount.Value).ToList() };
        }

        public List<OperatorViewerTable> GetOperatorViewTable(int userId, string staTradingDate, string endTradingDate)
        {
            List<OperatorViewerTable> operatorViewList = new List<OperatorViewerTable>();

            //need group by client??????
            var tradingOrders = _context.TradingOrder
                .Where(x => x.TradingDate.CompareTo(staTradingDate) >= 0 && x.TradingDate.CompareTo(endTradingDate) <= 0 && x.OrderType == TradingOrderType.Normal && x.StockOperatorUserID == userId)
                .GroupBy(x => new
                {
                    x.TradingDate,
                    x.StockCode
                })
                .Select(g => new
                {
                    TradingDate = g.Key.TradingDate,
                    StockCode = g.Key.StockCode,
                    StockName = g.FirstOrDefault().StockName,
                    SumSettledAmount = g.Sum(x => x.SettleAmount)
                }).ToList();

            foreach (var order in tradingOrders)
            {
                operatorViewList.Add(new OperatorViewerTable() { TradingDate = order.TradingDate, StockCode = order.StockCode, StockName = order.StockName, ProfitPerStock = order.SumSettledAmount.Value });
            }

            return operatorViewList;
        }

        public ManagerViewerSummary GetManagerViewerSummary(string staTradingDate, string endTradingDate, int userId)
        {
            return new ManagerViewerSummary()
            {
                ManagerViewChartSummary = GetManagerViewChart(staTradingDate, endTradingDate, userId),
                ManagerViewTableSummary = GetManagerViewTable(staTradingDate, endTradingDate, userId),
                Users = new AccountSettingManager().GetUsersDropDownList(RoleType.Client)
            };
        }

        public ManagerViewChart GetManagerViewChart(string staTradingDate, string endTradingDate, int userId)
        {
            if (userId > 0)
            {
                List<TradingOrderTempInfo> tradingOrders = _context.TradingOrder
                    .Where(x => x.TradingDate.CompareTo(staTradingDate) >= 0 && x.TradingDate.CompareTo(endTradingDate) <= 0 && x.OrderType == TradingOrderType.Normal && x.TradingStock.UserID == userId)
                    .GroupBy(x => new
                    {
                        x.TradingDate
                    })
                    .Select(g => new TradingOrderTempInfo
                    {
                        TradingDate = g.Key.TradingDate,
                        OperateProfit = g.Sum(x => x.SettleAmount).Value
                    }).ToList();

                foreach (TradingOrderTempInfo order in tradingOrders)
                {
                    if (order.OperateProfit >= 0)
                    {
                        decimal profitPercent = 1 - Math.Round(_context.TradingClient.Where(x => x.UserID == userId).FirstOrDefault().ProfitPercent.Value / 100, 2);
                        order.CompanyProfit = Math.Round(order.OperateProfit * profitPercent, 3);
                    }
                    else
                    {
                        List<TransferOrder> transferOrderList = _context.TransferOrder.Where(x => x.UserID == userId).ToList();
                        order.CompanyProfit = order.OperateProfit;
                        if (transferOrderList.Any(x => x.TrasferDate.Equals(order.TradingDate)))
                        {
                            order.TransferAmount = transferOrderList.Where(x => x.TrasferDate.Equals(order.TradingDate)).Sum(x => x.Amount);
                        }
                    }
                }
                return new ManagerViewChart()
                {
                    TradingDateList = tradingOrders.Select(x => x.TradingDate).ToList(),
                    DailyProfitList = tradingOrders.Select(x => x.CompanyProfit).ToList()
                };
            }
            else
            {
                List<TradingOrderTempInfo> tradingOrders = _context.TradingOrder
                    .Include(nameof(TradingStock))
                    .Where(x => x.TradingDate.CompareTo(staTradingDate) >= 0 && x.TradingDate.CompareTo(endTradingDate) <= 0 && x.OrderType == TradingOrderType.Normal)
                    .GroupBy(x => new
                    {
                        x.TradingDate,
                        x.TradingStock.UserID
                    })
                    .Select(g => new TradingOrderTempInfo
                    {
                        TradingDate = g.Key.TradingDate,
                        UserId = g.Key.UserID,
                        OperateProfit = g.Sum(x => x.SettleAmount).Value
                    }).ToList();

                foreach (TradingOrderTempInfo order in tradingOrders)
                {
                    if (order.OperateProfit >= 0)
                    {
                        List<TradingClient> clientList = _context.TradingClient.ToList();
                        decimal profitPercent = 1 - Math.Round(clientList.Find(x => x.UserID == order.UserId).ProfitPercent.Value / 100, 2);
                        order.CompanyProfit = Math.Round((order.OperateProfit * profitPercent), 3);
                    }
                    else
                    {
                        List<TransferOrder> transferOrderList = _context.TransferOrder.ToList();
                        order.CompanyProfit = order.OperateProfit;
                        if (transferOrderList.Any(x => x.TrasferDate.Equals(order.TradingDate) && x.UserID == order.UserId))
                        {
                            order.TransferAmount = transferOrderList.Where(x => x.TrasferDate.Equals(order.TradingDate) && x.UserID == order.UserId).Sum(x => x.Amount);
                        }
                    }
                }

                var finalTradingOrders = tradingOrders
                    .GroupBy(x => new
                    {
                        x.TradingDate
                    })
                    .Select(g => new
                    {
                        TradingDate = g.Key.TradingDate,
                        CompanyProfit = g.Sum(x => x.CompanyProfit)
                    }).ToList();

                return new ManagerViewChart()
                {
                    TradingDateList = finalTradingOrders.Select(x => x.TradingDate).ToList(),
                    DailyProfitList = finalTradingOrders.Select(x => x.CompanyProfit).ToList()
                };
            }
        }

        public List<ManagerViewTable> GetManagerViewTable(string staTradingDate, string endTradingDate, int userId)
        {
            List<ManagerViewTable> managerViewList = new List<ManagerViewTable>();

            if (userId > 0)
            {
                List<User> userList = _context.User.ToList();
                List<TradingOrderTempInfo> tradingOrders = _context.TradingOrder
                    .Include(nameof(TradingStock))
                    .Where(x => x.TradingDate.CompareTo(staTradingDate) >= 0 && x.TradingDate.CompareTo(endTradingDate) <= 0 && x.OrderType == TradingOrderType.Normal && x.TradingStock.UserID == userId)
                    .GroupBy(x => new
                    {
                        x.TradingDate
                    })
                    .Select(g => new TradingOrderTempInfo
                    {
                        TradingDate = g.Key.TradingDate,
                        OperateProfit = g.Sum(x => x.SettleAmount).Value
                    }).ToList();

                foreach (TradingOrderTempInfo order in tradingOrders)
                {
                    if (order.OperateProfit >= 0)
                    {
                        decimal profitPercent = 1 - Math.Round(_context.TradingClient.Where(x => x.UserID == userId).FirstOrDefault().ProfitPercent.Value / 100, 2);
                        order.CompanyProfit = Math.Round(order.OperateProfit * profitPercent, 3);
                    }
                    else
                    {
                        List<TransferOrder> transferOrderList = _context.TransferOrder.Where(x => x.UserID == userId).ToList();
                        order.CompanyProfit = order.OperateProfit;
                        if (transferOrderList.Any(x => x.TrasferDate.Equals(order.TradingDate)))
                        {
                            order.TransferAmount = transferOrderList.Where(x => x.TrasferDate.Equals(order.TradingDate)).Sum(x => x.Amount);
                        }
                    }
                    
                    managerViewList.Add(new ManagerViewTable()
                    {
                        TradingDate = order.TradingDate,
                        UserId = userId,
                        ClientName = userList.Find(x => x.ID == userId).LoginName,
                        OperateProfit = order.OperateProfit,
                        CompanyProfit = order.CompanyProfit,
                        TransferAmount = order.TransferAmount,
                        CanTransfer = order.CompanyProfit < 0
                    });
                }
            }
            else
            {
                List<TradingOrderTempInfo> tradingOrders = _context.TradingOrder
                    .Include(nameof(TradingStock))
                    .Where(x => x.TradingDate.CompareTo(staTradingDate) >= 0 && x.TradingDate.CompareTo(endTradingDate) <= 0 && x.OrderType == TradingOrderType.Normal)
                    .GroupBy(x => new
                    {
                        x.TradingDate,
                        x.TradingStock.UserID
                    })
                    .Select(g => new TradingOrderTempInfo
                    {
                        TradingDate = g.Key.TradingDate,
                        UserId = g.Key.UserID,
                        OperateProfit = g.Sum(x => x.SettleAmount).Value
                    }).ToList();

                foreach (TradingOrderTempInfo order in tradingOrders)
                {
                    if (order.OperateProfit >= 0)
                    {
                        List<TradingClient> clientList = _context.TradingClient.ToList();
                        decimal profitPercent = 1 - Math.Round(clientList.Find(x => x.UserID == order.UserId).ProfitPercent.Value / 100, 2);
                        order.CompanyProfit = Math.Round(order.OperateProfit * profitPercent, 3);
                    }
                    else
                    {
                        List<TransferOrder> transferOrderList = _context.TransferOrder.ToList();
                        order.CompanyProfit = order.OperateProfit;
                        if (transferOrderList.Any(x => x.TrasferDate.Equals(order.TradingDate) && x.UserID == order.UserId))
                        {
                            order.TransferAmount = transferOrderList.Where(x => x.TrasferDate.Equals(order.TradingDate) && x.UserID == order.UserId).Sum(x => x.Amount);
                        }
                    }
                    
                    
                }

                managerViewList = tradingOrders
                    .GroupBy(x => new
                    {
                        x.TradingDate
                    })
                    .Select(g => new ManagerViewTable
                    {
                        TradingDate = g.Key.TradingDate,
                        ClientName = "所有客户",
                        OperateProfit = g.Sum(x => x.OperateProfit),
                        CompanyProfit = g.Sum(x => x.CompanyProfit),
                    }).ToList();
            }

            return managerViewList;
        }

        public ManagerViewerDetailSummary GetManagerViewerDetailSummary(string staTradingDate, string endTradingDate, int userId)
        {
            return new ManagerViewerDetailSummary()
            {
                TradingOrders = new TradingOrderManager().GetTradingOrders(staTradingDate, endTradingDate, userId),
                Users = new AccountSettingManager().GetUsersDropDownList(RoleType.Client)
            };
        }

        public void SaveTradingOrderColumnValue(int id, TradingOrderColumn column, int value)
        {
            TradingOrder order = _context.TradingOrder.Where(x => x.ID == id).FirstOrDefault();
            switch (column)
            {
                case TradingOrderColumn.OrderType:
                    order.OrderType = value;
                    break;
                default:
                    //do nothing
                    break;
            }
            _context.Entry(order).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
