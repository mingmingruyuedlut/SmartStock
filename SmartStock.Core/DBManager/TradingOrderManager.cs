﻿using SmartStock.Core.Context;
using SmartStock.Core.Entities;
using SmartStock.Core.Enums;
using SmartStock.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SmartStock.Core.DBManager
{
    public class TradingOrderManager
    {
        private SSContext _context = new SSContext();

        public TradingOrderSummary GetTradingOrderSummary(string staTradingDate, string endTradingDate, int userId)
        {
            return new TradingOrderSummary()
            {
                TradingOrders = GetTradingOrders(staTradingDate, endTradingDate, userId),
                Users = new AccountSettingManager().GetUsersDropDownList(RoleType.Client)
            };
        }

        public TradingOrderSummary GetSplitedTradingOrderSummary(string staTradingDate, string endTradingDate, int userId)
        {
            return new TradingOrderSummary()
            {
                TradingOrders = GetSplitedTradingOrders(staTradingDate, endTradingDate, userId),
                Users = new AccountSettingManager().GetUsersDropDownList(RoleType.Client)
            };
        }

        public List<TradingOrderInfo> GetTradingOrders(string staTradingDate, string endTradingDate, int userId)
        {
            if (userId > 0)
            {
                return _context.TradingOrder
                .Include(nameof(TradingStock))
                .Include(nameof(User))
                .Where(x => x.TradingDate.CompareTo(staTradingDate) >= 0 && x.TradingDate.CompareTo(endTradingDate) <= 0 && x.TradingStock.UserID == userId)
                .Select(x => new TradingOrderInfo
                {
                    ID = x.ID,
                    TradingDate = x.TradingDate,
                    BuySellType = x.BuySellType,
                    BuySellTypeDescription = x.BuySellType == TradingOrderBuySellType.Buy ? TradingOrderBuySellTypeDes.TradingBuy : TradingOrderBuySellTypeDes.TradingSell,
                    StockCode = x.TradingStock.StockCode,
                    StockName = x.TradingStock.StockName,
                    TradingPrice = x.TradingPrice.Value,
                    TradingNumber = x.TradingNumber.Value,
                    TradingAmount = x.TradingAmount.Value,
                    SettleAmount = x.SettleAmount.Value,
                    TradingCode = x.TradingCode,
                    StockHolderCode = x.TradingStock.StockHolderCode,
                    StockOperator = x.StockOperatorUser.LoginName,
                    OrderType = x.OrderType.Value,
                    ParentID = x.ParentID.HasValue ? x.ParentID.Value : 0
                }).ToList();
            }
            else
            {
                return _context.TradingOrder
                .Include(nameof(TradingStock))
                .Include(nameof(User))
                .Where(x => x.TradingDate.CompareTo(staTradingDate) >= 0 && x.TradingDate.CompareTo(endTradingDate) <= 0)
                .Select(x => new TradingOrderInfo
                {
                    ID = x.ID,
                    TradingDate = x.TradingDate,
                    BuySellType = x.BuySellType,
                    BuySellTypeDescription = x.BuySellType == TradingOrderBuySellType.Buy ? TradingOrderBuySellTypeDes.TradingBuy : TradingOrderBuySellTypeDes.TradingSell,
                    StockCode = x.TradingStock.StockCode,
                    StockName = x.TradingStock.StockName,
                    TradingPrice = x.TradingPrice.Value,
                    TradingNumber = x.TradingNumber.Value,
                    TradingAmount = x.TradingAmount.Value,
                    SettleAmount = x.SettleAmount.Value,
                    TradingCode = x.TradingCode,
                    StockHolderCode = x.TradingStock.StockHolderCode,
                    StockOperator = x.StockOperatorUser.LoginName,
                    OrderType = x.OrderType.Value,
                    ParentID = x.ParentID.HasValue ? x.ParentID.Value : 0
                }).ToList();
            }
        }

        public List<TradingOrderInfo> GetSplitedTradingOrders(string staTradingDate, string endTradingDate, int userId)
        {
            List<int> splitedIds = new List<int>();
            if (userId > 0)
            {
                var orderIdObjList = _context.TradingOrder
                    .Where(x => x.TradingDate.CompareTo(staTradingDate) >= 0 && x.TradingDate.CompareTo(endTradingDate) <= 0 && x.TradingStock.UserID == userId && x.ParentID.HasValue && x.ParentID.Value > 0)
                    .Select(x => new
                    {
                        orderId = x.ID,
                        parentOrderId = x.ParentID.Value
                    }).ToList();

                foreach (var orderIdObj in orderIdObjList)
                {
                    if (!splitedIds.Contains(orderIdObj.orderId))
                    {
                        splitedIds.Add(orderIdObj.orderId);
                    }
                    if (!splitedIds.Contains(orderIdObj.parentOrderId))
                    {
                        splitedIds.Add(orderIdObj.parentOrderId);
                    }
                }
            }
            else
            {
                var orderIdObjList = _context.TradingOrder
                    .Where(x => x.TradingDate.CompareTo(staTradingDate) >= 0 && x.TradingDate.CompareTo(endTradingDate) <= 0 && x.ParentID.HasValue && x.ParentID.Value > 0)
                    .Select(x => new
                    {
                        orderId = x.ID,
                        parentOrderId = x.ParentID.Value
                    }).ToList();

                foreach (var orderIdObj in orderIdObjList)
                {
                    if (!splitedIds.Contains(orderIdObj.orderId))
                    {
                        splitedIds.Add(orderIdObj.orderId);
                    }
                    if (!splitedIds.Contains(orderIdObj.parentOrderId))
                    {
                        splitedIds.Add(orderIdObj.parentOrderId);
                    }
                }
            }

            return _context.TradingOrder
                .Include(nameof(TradingStock))
                .Include(nameof(User))
                .Where(x => splitedIds.Contains(x.ID))
                .Select(x => new TradingOrderInfo
                {
                    ID = x.ID,
                    TradingDate = x.TradingDate,
                    BuySellType = x.BuySellType,
                    BuySellTypeDescription = x.BuySellType == TradingOrderBuySellType.Buy ? TradingOrderBuySellTypeDes.TradingBuy : TradingOrderBuySellTypeDes.TradingSell,
                    StockCode = x.TradingStock.StockCode,
                    StockName = x.TradingStock.StockName,
                    TradingPrice = x.TradingPrice.Value,
                    TradingNumber = x.TradingNumber.Value,
                    TradingAmount = x.TradingAmount.Value,
                    SettleAmount = x.SettleAmount.Value,
                    TradingCode = x.TradingCode,
                    StockHolderCode = x.TradingStock.StockHolderCode,
                    StockOperator = x.StockOperatorUser.LoginName,
                    OrderType = x.OrderType.Value,
                    ParentID = x.ParentID.HasValue ? x.ParentID.Value : 0
                }).ToList();
        }

        public void SaveSplitTradingOrder(int toId, string splitDate, int splitNumber)
        {
            TradingOrder parentTradingOrder = _context.TradingOrder.FirstOrDefault(x => x.ID == toId);
            parentTradingOrder.TradingNumber -= splitNumber;
            parentTradingOrder.TradingAmount = parentTradingOrder.TradingNumber * parentTradingOrder.TradingPrice;
            parentTradingOrder.SettleAmount = parentTradingOrder.SettleAmount > 0 ? parentTradingOrder.SettleAmount - parentTradingOrder.TradingPrice * splitNumber : parentTradingOrder.SettleAmount + parentTradingOrder.TradingPrice * splitNumber;
            _context.Entry(parentTradingOrder).State = EntityState.Modified;

            TradingOrder splitTradingOrder = new TradingOrder()
            {
                TradingStockID = parentTradingOrder.TradingStockID,
                StockOperatorUserID = parentTradingOrder.StockOperatorUserID,
                TradingDate = splitDate,
                BuySellType = parentTradingOrder.BuySellType,
                TradingCode = parentTradingOrder.TradingCode,
                TradingPrice = parentTradingOrder.TradingPrice,
                TradingNumber = splitNumber,
                TradingAmount = splitNumber * parentTradingOrder.TradingPrice,
                SettleAmount = parentTradingOrder.SettleAmount > 0 ? splitNumber * parentTradingOrder.TradingPrice : -splitNumber * parentTradingOrder.TradingPrice,
                OrderType = parentTradingOrder.OrderType,
                ParentID = toId
            };

            _context.TradingOrder.Add(splitTradingOrder);

            _context.SaveChanges();
        }

        public void SaveMergeTradingOrder(int toId, int mergeNumber)
        {
            TradingOrder currentTradingOrder = _context.TradingOrder.FirstOrDefault(x => x.ID == toId);
            TradingOrder parentTradingOrder = _context.TradingOrder.FirstOrDefault(x => x.ID == currentTradingOrder.ParentID);
            parentTradingOrder.TradingNumber += mergeNumber;
            parentTradingOrder.TradingAmount += currentTradingOrder.TradingPrice * mergeNumber;
            parentTradingOrder.SettleAmount += currentTradingOrder.SettleAmount > 0 ? currentTradingOrder.TradingPrice * mergeNumber : -currentTradingOrder.TradingPrice * mergeNumber;
            _context.Entry(parentTradingOrder).State = EntityState.Modified;

            if (currentTradingOrder.TradingNumber == mergeNumber)
            {
                //delete current order
                _context.Entry(currentTradingOrder).State = EntityState.Deleted;
            }
            else
            {
                currentTradingOrder.TradingNumber -= mergeNumber;
                currentTradingOrder.TradingAmount -= currentTradingOrder.TradingPrice * mergeNumber;
                currentTradingOrder.SettleAmount -= currentTradingOrder.SettleAmount > 0 ? currentTradingOrder.TradingPrice * mergeNumber : -currentTradingOrder.TradingPrice * mergeNumber;
                _context.Entry(currentTradingOrder).State = EntityState.Modified;
            }

            _context.SaveChanges();
        }

        public string UploadTradingOrder(string filePath, string operatorName)
        {
            return new TradingReader().GetTradingOrder(filePath, operatorName);
        }

        public string UploadTradingOrder(StreamReader sr, UploadResourceType urType, int operatorUserId)
        {
            string resultMsg = string.Empty;
            if (urType == UploadResourceType.TDX)
            {
                resultMsg = UploadTradingOrderTDX(sr, operatorUserId);
            }
            else if (urType == UploadResourceType.THS)
            {
                resultMsg = UploadTradingOrderTHS(sr, operatorUserId);
            }
            else if (urType == UploadResourceType.HT)
            {
                resultMsg = UploadTradingOrderHT(sr);
            }
            return resultMsg;
        }

        public string UploadTradingOrderTDX(StreamReader sr, int operatorUserId)
        {
            string resultMsg = string.Empty;
            List<TradingStock> tStockList = _context.TradingStock.ToList();
            List<User> userList = _context.User.ToList();
            string tradingLine = string.Empty; //读文件中的一行
            while ((tradingLine = sr.ReadLine()) != null)
            {
                string[] tradingFields = Regex.Split(tradingLine, @"\s\s+");
                if (IsDateStr(tradingFields[0], DateStringFormats.yyyyMMdd))
                {
                    string bsTypeDescription = tradingFields[1];
                    if (IsValidTradingOrderBSType(bsTypeDescription))
                    {
                        TradingStock tStock = new TradingStock()
                        {
                            StockCode = tradingFields[2],
                            StockName = tradingFields[3],
                            StockHolderCode = tradingFields[17],
                            CashAccountNo = tradingFields[18]
                        };

                        if (tStockList.Any(x => x.StockCode.Equals(tStock.StockCode) && x.StockHolderCode.Equals(tStock.StockHolderCode)))
                        {
                            TradingOrder tOrder = new TradingOrder()
                            {
                                TradingDate = tradingFields[0],
                                BuySellType = bsTypeDescription.Equals("证券买入") ? TradingOrderBuySellType.Buy : TradingOrderBuySellType.Sell,
                                TradingPrice = decimal.Parse(tradingFields[4]),
                                TradingNumber = Int32.Parse(tradingFields[5]),
                                TradingAmount = decimal.Parse(tradingFields[7]),
                                SettleAmount = decimal.Parse(tradingFields[8]),
                                TradingCode = tradingFields[16],
                                OrderType = TradingOrderType.Normal,
                                ResourceType = UploadResourceType.TDX,
                                StockOperatorUserID = operatorUserId
                            };

                            tOrder.TradingStockID = tStockList.FirstOrDefault(x => x.StockCode.Equals(tStock.StockCode) && x.StockHolderCode.Equals(tStock.StockHolderCode)).ID;

                            if (!IsTradingOrderExist(tOrder))
                            {
                                _context.TradingOrder.Add(tOrder);
                                _context.SaveChanges();
                                resultMsg = "success";
                            }
                        }
                    }
                }
            }
            return resultMsg;
        }

        public string UploadTradingOrderTHS(StreamReader sr, int operatorUserId)
        {
            string resultMsg = string.Empty;
            List<TradingStock> tStockList = _context.TradingStock.ToList();
            List<User> userList = _context.User.ToList();
            string tradingLine = string.Empty; //读文件中的一行
            while ((tradingLine = sr.ReadLine()) != null)
            {
                string[] tradingFields = Regex.Split(tradingLine, @"\t");
                if (IsDateStr(tradingFields[0], DateStringFormats.yyyyMMdd))
                {
                    string bsTypeDescription = tradingFields[4];
                    if (IsValidTradingOrderBSType(bsTypeDescription))
                    {
                        TradingStock tStock = new TradingStock()
                        {
                            StockCode = tradingFields[2],
                            StockName = tradingFields[3],
                            StockHolderCode = tradingFields[16]
                        };

                        if (tStockList.Any(x => x.StockCode.Equals(tStock.StockCode) && x.StockHolderCode.Equals(tStock.StockHolderCode)))
                        {
                            TradingOrder tOrder = new TradingOrder()
                            {
                                TradingDate = tradingFields[0],
                                BuySellType = bsTypeDescription.Equals("证券买入") ? TradingOrderBuySellType.Buy : TradingOrderBuySellType.Sell,
                                TradingPrice = decimal.Parse(tradingFields[6]),
                                TradingNumber = Int32.Parse(tradingFields[5]),
                                TradingAmount = decimal.Parse(tradingFields[7]),
                                SettleAmount = decimal.Parse(tradingFields[9]),
                                TradingCode = tradingFields[14],
                                OrderType = TradingOrderType.Normal,
                                ResourceType = UploadResourceType.THS,
                                StockOperatorUserID = operatorUserId
                            };

                            tOrder.TradingStockID = tStockList.FirstOrDefault(x => x.StockCode.Equals(tStock.StockCode) && x.StockHolderCode.Equals(tStock.StockHolderCode)).ID;

                            if (!IsTradingOrderExist(tOrder))
                            {
                                _context.TradingOrder.Add(tOrder);
                                _context.SaveChanges();
                                resultMsg = "success";
                            }
                        }
                    }
                }
            }
            return resultMsg;
        }

        public string UploadTradingOrderHT(StreamReader sr)
        {
            string resultMsg = string.Empty;
            List<TradingStock> tStockList = _context.TradingStock.ToList();
            List<User> userList = _context.User.ToList();
            string tradingLine = string.Empty; //读文件中的一行



            return resultMsg;
        }

        private bool IsTradingOrderExist(TradingOrder tOrder)
        {
            return _context.TradingOrder.Any(x => x.TradingStockID == tOrder.TradingStockID && 
                x.TradingDate.Equals(tOrder.TradingDate) &&
                x.BuySellType == tOrder.BuySellType &&
                x.TradingCode.Equals(tOrder.TradingCode));
        }

        private bool IsValidTradingOrderBSType(string bsTypeDes)
        {
            return (bsTypeDes.Equals("证券买入") || bsTypeDes.Equals("证券卖出"));
        }

        private bool IsDateStr(string dateStr, string dateFormat)
        {
            DateTime currentDateTime;
            return DateTime.TryParseExact(dateStr, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out currentDateTime);
        }
    }
}
