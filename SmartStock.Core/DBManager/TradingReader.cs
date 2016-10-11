using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using System.Net;
using System.IO;
using System.Web;
using System.Diagnostics;

using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace SmartStock.Core.DBManager
{
    public class TradingReader
    {
        /// <summary>
        /// 取得交割单信息，导入数据库
        /// </summary>
        /// <param name="filePath">交割单文件的路径</param>
        /// <param name="stockOperator">导入交割单的交易员，默认值 : Admin</param>
        /// <returns></returns>
        public string GetTradingOrder(string filePath, string stockOperator)
        {
            string returnMessage = "";

            try
            {
                if (!File.Exists(filePath))
                {
                    returnMessage = "文件不存在，请选择需要导入的文件。"; 
                    
                    return returnMessage;
                }

                String tradingLine = "";                                                               //读文件中的一行

                StreamReader sr = new StreamReader(filePath, Encoding.GetEncoding("gb2312"));          //Encoding.Default);

                while ((tradingLine = sr.ReadLine()) != null)
                {

                    string[] tradingField = System.Text.RegularExpressions.Regex.Split(tradingLine, @"\s\s+"); //tradingLine.Split(' ');

                    string tradingDate = tradingField[0].ToString();

                    if (tradingDate.IndexOf("20") > -1)
                    {
                        string buySellType = tradingField[1].ToString();
                        string stockCode = tradingField[2].ToString();
                        string stockName = tradingField[3].ToString();
                        float tradingPrice = Convert.ToSingle(tradingField[4]);
                        int tradingNumber = Convert.ToInt16(tradingField[5]);
                        float tradingAmount = Convert.ToSingle(tradingField[7]);
                        float settleAmount = Convert.ToSingle(tradingField[8]);
                        string tradingCode = tradingField[16].ToString();
                        string stockHolderCode = tradingField[17].ToString();
                        string cashAccountNo = tradingField[18].ToString();

                        if (buySellType == "证券买入" || buySellType == "证券卖出")
                        {
                            WriteTradingOrder(tradingDate, buySellType, stockCode, stockName, tradingPrice, tradingNumber, tradingAmount, settleAmount, tradingCode, stockHolderCode, cashAccountNo, stockOperator);

                            returnMessage = "success";
                        }
                    }
                }

                //更新 奇数 的交易记录，某天的交易记录，如果产生了奇数条交易记录，就把IsOddOrder字段更新成 1.
                //UpdateOddOrder();
            }
            catch (Exception ex)
            {
                returnMessage = "交割单数据导入失败。";

                Utility.writeLog(returnMessage + ex.Message);                
            }

            return returnMessage;
        }

        public string UploadTradingOrder(StreamReader sr, string stockOperator)
        {
            string returnMessage = "";

            try
            {
                string tradingLine = "";                                                               //读文件中的一行

                while ((tradingLine = sr.ReadLine()) != null)
                {

                    string[] tradingField = System.Text.RegularExpressions.Regex.Split(tradingLine, @"\s\s+"); //tradingLine.Split(' ');

                    string tradingDate = tradingField[0].ToString();

                    if (tradingDate.IndexOf("20") > -1)
                    {
                        string buySellType = tradingField[1].ToString();
                        string stockCode = tradingField[2].ToString();
                        string stockName = tradingField[3].ToString();
                        float tradingPrice = Convert.ToSingle(tradingField[4]);
                        int tradingNumber = Convert.ToInt16(tradingField[5]);
                        float tradingAmount = Convert.ToSingle(tradingField[7]);
                        float settleAmount = Convert.ToSingle(tradingField[8]);
                        string tradingCode = tradingField[16].ToString();
                        string stockHolderCode = tradingField[17].ToString();
                        string cashAccountNo = tradingField[18].ToString();

                        if (buySellType == "证券买入" || buySellType == "证券卖出")
                        {
                            WriteTradingOrder(tradingDate, buySellType, stockCode, stockName, tradingPrice, tradingNumber, tradingAmount, settleAmount, tradingCode, stockHolderCode, cashAccountNo, stockOperator);

                            returnMessage = "success";
                        }
                    }
                }

                //更新 奇数 的交易记录，某天的交易记录，如果产生了奇数条交易记录，就把IsOddOrder字段更新成 1.
                //UpdateOddOrder();
            }
            catch (Exception ex)
            {
                returnMessage = "交割单数据导入失败。";

                Utility.writeLog(returnMessage + ex.Message);
            }

            return returnMessage;
        }

        /// <summary>
        /// 交割单信息写入数据库
        /// </summary>
        /// <param name="tradingDate"></param>
        /// <param name="buySellType"></param>
        /// <param name="stockCode"></param>
        /// <param name="stockName"></param>
        /// <param name="tradingPrice"></param>
        /// <param name="tradingNumber"></param>
        /// <param name="tradingAmount"></param>
        /// <param name="settleAmount"></param>
        /// <param name="tradingCode"></param>
        /// <param name="stockHolderCode"></param>
        /// <param name="cashAccountNo"></param>
        /// <param name="stockOperator"></param>
        private void WriteTradingOrder(string tradingDate, string buySellType, string stockCode, string stockName,float tradingPrice,int tradingNumber, float tradingAmount,float settleAmount,string tradingCode,string stockHolderCode, string cashAccountNo, string stockOperator )
        {
            if (!IsOrderExist(tradingDate, stockCode, buySellType, tradingCode, stockHolderCode))
            {

                string sql = "INSERT INTO TradingOrder ( " +
                             "TradingDate, BuySellType, StockCode, StockName, TradingPrice, TradingNumber, TradingAmount, SettleAmount, TradingCode, StockHolderCode, CashAccountNo, StockOperator, IsOddOrder) " +
                             "  VALUES " +
                             "(@tradingDate, @buySellType, @stockCode, @stockName, @tradingPrice, @tradingNumber, @tradingAmount, @settleAmount, @tradingCode, @stockHolderCode,  @cashAccountNo,@stockOperator, 0)";


                SqlParameter[] parms = {				   
                        new SqlParameter("@tradingDate", SqlDbType.NVarChar, 10),                            
                        new SqlParameter("@buySellType", SqlDbType.NVarChar, 10),                            
                        new SqlParameter("@stockCode", SqlDbType.NVarChar, 10), 
                        new SqlParameter("@stockName", SqlDbType.NVarChar, 10), 
                        new SqlParameter("@tradingPrice", SqlDbType.Float), 
                        new SqlParameter("@tradingNumber", SqlDbType.Int),         
                        new SqlParameter("@tradingAmount", SqlDbType.Float), 
                        new SqlParameter("@settleAmount", SqlDbType.Float),
                        new SqlParameter("@tradingCode", SqlDbType.NVarChar,50),
                        new SqlParameter("@stockHolderCode", SqlDbType.NVarChar,50),
                        new SqlParameter("@cashAccountNo", SqlDbType.NVarChar,50),
                        new SqlParameter("@stockOperator", SqlDbType.NVarChar,50)
                                   };

                parms[0].Value = tradingDate;
                parms[1].Value = buySellType;
                parms[2].Value = stockCode;
                parms[3].Value = stockName;
                parms[4].Value = tradingPrice;
                parms[5].Value = tradingNumber;
                parms[6].Value = tradingAmount;
                parms[7].Value = settleAmount;
                parms[8].Value = tradingCode;
                parms[9].Value = stockHolderCode;
                parms[10].Value = cashAccountNo;
                parms[11].Value = stockOperator;

                int i = Convert.ToInt32(SqlHelper.ExcuteNonQuery(CommandType.Text, sql, parms));
            }
        }

        /// <summary>
        /// 判断是否是已经导入了交割单数据
        /// </summary>
        /// <param name="tradingkDate"></param>
        /// <param name="stockCode"></param>
        /// <param name="buySellType"></param>
        /// <param name="tradingCode"></param>
        /// <param name="stockHolderCode"></param>
        /// <returns></returns>
        private bool IsOrderExist(string tradingkDate, string stockCode, string buySellType, string tradingCode, string stockHolderCode)
        {
            bool orderExist = false;

            string sql = "SELECT COUNT(*) FROM TradingOrder WHERE tradingDate = @tradingDate AND StockCode = @stockCode AND BuySellType = @buySellType AND TradingCode = @tradingCode AND StockHolderCode = @stockHolderCode";

            SqlParameter[] parms = {				   
				new SqlParameter("@tradingDate", SqlDbType.NVarChar,10),
                new SqlParameter("@stockCode", SqlDbType.NVarChar,10),
                new SqlParameter("@buySellType",SqlDbType.NVarChar,10),
                new SqlParameter("@tradingCode", SqlDbType.NVarChar,50),
                new SqlParameter("@stockHolderCode", SqlDbType.NVarChar,50)
                                   };

            parms[0].Value = tradingkDate;
            parms[1].Value = stockCode;
            parms[2].Value = buySellType;
            parms[3].Value = tradingCode;
            parms[4].Value = stockHolderCode;
            

            int i = Convert.ToInt32(SqlHelper.ExcuteScalar(CommandType.Text, sql, parms));

            if (i >= 1)
            {               
                orderExist = true;
            }

            return orderExist;
        }

        ///更新 奇数 的交易记录，某天的交易记录，如果产生了奇数条交易记录，就把IsOddOrder字段更新成 1 .
        /// <summary>
        /// 更新奇数交易记录为1
        /// </summary>
        private void UpdateOddOrder()
        {
            int i = 0;

            string stockCode = "";
            string tradingDate = "";
            string stockHolderCode = "";

            string sql = "SELECT TradingDate, StockHolderCode,  StockCode FROM TradingOrder WHERE IsOddOrder <> 1 " +
                " GROUP BY TradingDate, StockHolderCode,  StockCode" + //, BuySellType
                " ORDER BY TradingDate, StockHolderCode, StockCode"; //, BuySellType         找出交易的股票,"某日"交易的"某股东名下"的"某只股票"

            SqlDataReader reader = SqlHelper.ExcuteReader(CommandType.Text, sql, null);

            while (reader.Read())
            {
                i = i + 1;

                tradingDate = Convert.ToString(reader.GetValue(0));
                stockHolderCode = Convert.ToString(reader.GetValue(1));
                stockCode = Convert.ToString(reader.GetValue(2));

                UpdateOddOrder(tradingDate, stockHolderCode, stockCode);               
            }
        }

        /// <summary>
        /// 更新奇数交易记录为1
        /// </summary>
        /// <param name="tradingDate">交易日期</param>
        /// <param name="stockHolderCode">股东号码</param>
        /// <param name="stockCode">股票号码</param>
        private void UpdateOddOrder(string tradingDate, string stockHolderCode, string stockCode)
        {
            int i = 0;                                                                             // 交易次数
            int id = 0;                                                                            // 记录数

            string sql = "SELECT ID FROM TradingOrder WHERE tradingDate = @tradingDate AND StockHolderCode = @stockHolderCode AND StockCode = @stockCode ORDER BY ID";

            SqlParameter[] parms = {				   
				new SqlParameter("@tradingDate", SqlDbType.NVarChar,10),
                new SqlParameter("@stockHolderCode", SqlDbType.NVarChar,10),
                new SqlParameter("@stockCode",SqlDbType.NVarChar,10)
                                   };

            parms[0].Value = tradingDate;
            parms[1].Value = stockHolderCode;
            parms[2].Value = stockCode;

            try
            {
                SqlDataReader reader = SqlHelper.ExcuteReader(CommandType.Text, sql, parms);

                while (reader.Read())
                {
                    i = i + 1;

                    id = Convert.ToInt16(reader.GetValue(0));
                }
                
                if (Utility.IsOdd(i) == true)
                {
                    sql = "UPDATE TradingOrder SET IsOddOrder = 1 WHERE ID = " + id;

                    int j = Convert.ToInt32(SqlHelper.ExcuteNonQuery(CommandType.Text, sql, null));        //更新IsOddOrder字段为1
                }
            }
            catch(Exception ex)
            {
                Utility.writeLog(ex.Message);
            }
        }
     

        
    }
}






/* * ***************************************************************************************************************************
        /***** 读取的字段 *****/
        //public string FilePath = "";                                                               //文件位置
        //public string ReturnMessage = "";                                                          //返回信息
        //public string StockOperator = "";                                                          //交易员

        //private string tradingDate ="";                                                            //交割日期 *
        //private string StockCode ="";                                                              //股票代码 *
        //private string StockName ="";                                                              //股票名称 *
        //private string BuySellType ="";                                                            //业务名称 *
        //private string TradingCode = "";                                                           //成交编号 *
        //private float  TradingPrice = 0;                                                           //成交价格 *
        //private int    TradingNumber = 0;                                                          //成交数量 *
        //private float  TradingAmount = 0;                                                          //成交金额 *
        //private float SettleAmount = 0;                                                            //清算金额 *
        //private string StockHolderCode = "";                                                       //股东代码 *
        //private string CashAccountNo = "";                                                         //资金账号 *

        //private string Operator = "";  

/****************************************************************************************************************************/