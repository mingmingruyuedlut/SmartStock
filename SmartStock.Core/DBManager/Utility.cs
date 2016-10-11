using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStock.Core.DBManager
{
    public static class Utility
    {

        /////// <summary>
        /////// 取得某日的成交量
        /////// </summary>
        /////// <param name="stockcode">股票代码</param>
        /////// <param name="stockdate">股票日期</param>
        /////// <returns></returns>
        ////public static float getXDayVol(string stockcode, string stockdate)
        ////{
        ////    float vol = 0;            

        ////    string sql = "SELECT [TradingVolumn] FROM STOCKER.dbo.StockData WHERE StockCode = @StockCode AND StockDate = @StockDate";

        ////    try
        ////    {
        ////        SqlParameter[] parms = {				   
        ////        new SqlParameter("@StockCode", SqlDbType.NVarChar,6),
        ////        new SqlParameter("@StockDate", SqlDbType.NVarChar,10)
        ////                           };

        ////        parms[0].Value = stockcode;
        ////        parms[1].Value = stockdate;

        ////        SqlDataReader reader = SqlHelper.ExcuteReader(CommandType.Text, sql, parms);

        ////        while (reader.Read())
        ////        {
        ////            vol = Convert.ToSingle( reader.GetValue(0));
        ////        }
        ////        reader.Close();
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        writeLog(System.Reflection.MethodBase.GetCurrentMethod() + " : " + stockcode + " : " + ex.Message);
        ////    }

        ////    return vol;
        ////}


        /////// <summary>
        /////// 取得前一日成交量或者前多日的成交量均值， 不包括 stockdate 日
        /////// </summary>
        /////// <param name="stockcode">股票代码</param>
        /////// <param name="stockdate">股票日期</param>
        /////// <param name="m"> m = 1 时候，前一日的量，m > 1时，表示前 m 日的平均值</param>
        /////// <returns></returns>
        ////public static float getMDaysAverageVol(string stockcode, string stockdate, int m)
        ////{
        ////    float vol = 1;                                                                         //设置默认值为 1, 如果没有记录，返回值 1

        ////    string sql = "SELECT TOP (@M) TradingVolumn FROM STOCKER.dbo.StockData WHERE StockCode = @StockCode AND StockDate < @StockDate ORDER BY StockDate DESC";

        ////    try
        ////    {
        ////        SqlParameter[] parms = {	
        ////        new SqlParameter("@M", SqlDbType.Int),
        ////        new SqlParameter("@StockCode", SqlDbType.NVarChar,6),
        ////        new SqlParameter("@StockDate", SqlDbType.NVarChar,10)
        ////                           };

        ////        parms[0].Value = m;
        ////        parms[1].Value = stockcode;
        ////        parms[2].Value = stockdate;

        ////        SqlDataReader reader = SqlHelper.ExcuteReader(CommandType.Text, sql, parms);

        ////        int n = 0;
        ////        while (reader.Read())
        ////        {
        ////            n = n + 1;
        ////            vol = vol + Convert.ToSingle(reader.GetValue(0));
        ////        }
        ////        reader.Close();

        ////        if (n > 0)
        ////        {
        ////            if (n < m)
        ////            {
        ////                vol = vol / n;
        ////            }
        ////            else
        ////            {
        ////                vol = vol / m;
        ////            }
        ////        }
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        writeLog(System.Reflection.MethodBase.GetCurrentMethod() + " : " + stockcode + " : " + ex.Message);
        ////    }

        ////    return vol;
        ////}

        /////// <summary>
        /////// 取得放量日发生日期
        /////// </summary>
        /////// <param name="stockcode">股票代码</param>
        /////// <param name="buysellrule">股票日期</param>
        /////// <returns></returns>
        ////public static string getVolDay(string stockcode, string buysellrule)
        ////{
        ////    string voldate = "";

        ////    float xdayvol = 0;
        ////    float ndaysvol = 0;

        ////    string Last1stTradeDate = "";                                                            //当前日

        ////    int mdays = Utility.getMDaysBuySellRule(buysellrule);                                   // m 日内平均
        ////    float ntimes = Utility.getNTimesBuySellRule(buysellrule);                               // n 倍 量比
        ////    int ndays = Utility.getMDaysBuySellRule("VolDay");                                      // nday 日前的范围内找放量

        ////    for (int i = 1; i <= ndays; i++)
        ////    {
        ////        Last1stTradeDate = Utility.getTradeDay(stockcode, i);

        ////        xdayvol = getXDayVol(stockcode, Last1stTradeDate);
        ////        ndaysvol = getMDaysAverageVol(stockcode, Last1stTradeDate, mdays);

        ////        if (ndaysvol<=1)                                                                   //如果 n 日平均量 = 1， 后面没有数据了
        ////        {
        ////            break;                                                                         //跳出循环，不取平均值了。
        ////        }

        ////        if (xdayvol * ndaysvol != 0)
        ////        {
        ////            float nnn = xdayvol / ndaysvol;
        ////            if (xdayvol / ndaysvol > Convert.ToSingle(ntimes))
        ////            {
        ////                voldate = Last1stTradeDate;
        ////                break;
        ////            }
        ////        }
        ////    }

        ////    return voldate;
        ////}  

        /////// <summary>
        /////// 取得某交易日的开盘价
        /////// 开盘价 = [TodayOpenPrice]
        /////// </summary>
        /////// <param name="stockcode">股票代码</param>
        /////// <param name="stockdate">股票日期</param>
        /////// <returns></returns>
        ////public static float getOpenPrice(string stockcode, string stockdate)
        ////{
        ////    float price = 0;

        ////    string sql = "SELECT [TodayOpenPrice] FROM STOCKER.dbo.StockData WHERE StockCode = @StockCode AND StockDate = @StockDate";

        ////    try
        ////    {
        ////        SqlParameter[] parms = {	
        ////        new SqlParameter("@StockCode", SqlDbType.NVarChar,6),
        ////        new SqlParameter("@StockDate", SqlDbType.NVarChar,10)
        ////                           };

        ////        parms[0].Value = stockcode;
        ////        parms[1].Value = stockdate;

        ////        SqlDataReader reader = SqlHelper.ExcuteReader(CommandType.Text, sql, parms);

        ////        while (reader.Read())
        ////        {
        ////            price = Convert.ToSingle(reader.GetValue(0));
        ////        }
        ////        reader.Close();
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        writeLog(System.Reflection.MethodBase.GetCurrentMethod() + " : " + stockcode + " : " + ex.Message);
        ////    }

        ////    return price;
        ////}       

        /////// <summary>
        /////// 取某交易日的收盘价
        /////// </summary>
        /////// <param name="stockcode">股票代码</param>
        /////// <param name="stockdate">股票日期</param>
        /////// <returns></returns>
        ////public static float getClosePrice(string stockcode, string stockdate)
        ////{
        ////    float price = 0;

        ////    string sql = "SELECT [TodayClosePrice] FROM STOCKER.dbo.StockData WHERE StockCode = @StockCode AND StockDate = @StockDate";

        ////    try
        ////    {
        ////        SqlParameter[] parms = {	
        ////        new SqlParameter("@StockCode", SqlDbType.NVarChar,6),
        ////        new SqlParameter("@StockDate", SqlDbType.NVarChar,10)
        ////                               };

        ////        parms[0].Value = stockcode;
        ////        parms[1].Value = stockdate;

        ////        SqlDataReader reader = SqlHelper.ExcuteReader(CommandType.Text, sql, parms);

        ////        while (reader.Read())
        ////        {
        ////            price = Convert.ToSingle(reader.GetValue(0));
        ////        }
        ////        reader.Close();
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        writeLog(System.Reflection.MethodBase.GetCurrentMethod() + " : " + stockcode + " : " + ex.Message);
        ////    }

        ////    return price;
        ////}

        /////// <summary>
        /////// 取得某交易日的最高价
        /////// </summary>
        /////// <param name="stockcode">股票代码</param>
        /////// <param name="stockdate">股票日期</param>
        /////// <returns></returns>
        ////public static float getHighPrice(string stockcode, string stockdate)
        ////{
        ////    float price = 0;

        ////    string sql = "SELECT [TodayHighPrice] FROM STOCKER.dbo.StockData WHERE StockCode = @StockCode AND StockDate = @StockDate";

        ////    try
        ////    {
        ////        SqlParameter[] parms = {	
        ////        new SqlParameter("@StockCode", SqlDbType.NVarChar,6),
        ////        new SqlParameter("@StockDate", SqlDbType.NVarChar,10)
        ////                           };

        ////        parms[0].Value = stockcode;
        ////        parms[1].Value = stockdate;

        ////        SqlDataReader reader = SqlHelper.ExcuteReader(CommandType.Text, sql, parms);

        ////        while (reader.Read())
        ////        {
        ////            price = Convert.ToSingle(reader.GetValue(0));
        ////        }
        ////        reader.Close();
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        writeLog(System.Reflection.MethodBase.GetCurrentMethod() + " : " + stockcode + " : " + ex.Message);
        ////    }

        ////    return price;
        ////}

        /////// <summary>
        /////// 取得某交易日的最低价
        /////// </summary>
        /////// <param name="stockcode">股票代码</param>
        /////// <param name="stockdate">股票日期</param>
        /////// <returns></returns>
        ////public static float getLowPrice(string stockcode, string stockdate)
        ////{
        ////    float price = 0;

        ////    string sql = "SELECT [TodayLowPrice] FROM STOCKER.dbo.StockData WHERE StockCode = @StockCode AND StockDate = @StockDate";

        ////    try
        ////    {
        ////        SqlParameter[] parms = {	
        ////        new SqlParameter("@StockCode", SqlDbType.NVarChar,6),
        ////        new SqlParameter("@StockDate", SqlDbType.NVarChar,10)
        ////                           };

        ////        parms[0].Value = stockcode;
        ////        parms[1].Value = stockdate;

        ////        SqlDataReader reader = SqlHelper.ExcuteReader(CommandType.Text, sql, parms);

        ////        while (reader.Read())
        ////        {
        ////            price = Convert.ToSingle(reader.GetValue(0));
        ////        }
        ////        reader.Close();
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        writeLog(System.Reflection.MethodBase.GetCurrentMethod() + " : " + stockcode + " : " + ex.Message);
        ////    }

        ////    return price;
        ////}     

        /////// <summary>
        /////// 取得 M 日均价
        /////// </summary>
        /////// <param name="stockcode">股票代码</param> 
        /////// <param name="stockdate">股票日期</param> 
        /////// <param name="m"> m 日内 </param>
        /////// <returns></returns>
        ////public static float getAveragePrice(string stockcode, string stockdate, int m)
        ////{
        ////    float price = 0;

        ////    string sql = "SELECT TOP (@M) [TodayClosePrice] FROM STOCKER.dbo.StockData WHERE StockCode = @StockCode AND StockDate <= @StockDate ORDER BY stockdate DESC";

        ////    try
        ////    {
        ////        SqlParameter[] parms = {	
        ////        new SqlParameter("@M", SqlDbType.Int),
        ////        new SqlParameter("@StockCode", SqlDbType.NVarChar,6),
        ////        new SqlParameter("@StockDate", SqlDbType.NVarChar,10)
        ////                           };
        ////        parms[0].Value = m;
        ////        parms[1].Value = stockcode;
        ////        parms[2].Value = stockdate;

        ////        SqlDataReader reader = SqlHelper.ExcuteReader(CommandType.Text, sql, parms);

        ////        while (reader.Read())
        ////        {
        ////            price = price + Convert.ToSingle(reader.GetValue(0));
        ////        }
        ////        reader.Close();

        ////    if (m != 0)
        ////    {
        ////        price = price / m;

        ////        }
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        writeLog(System.Reflection.MethodBase.GetCurrentMethod() + " : " + stockcode + " : " + ex.Message);
        ////    }

        ////    return price;
        ////}

        /////// <summary>
        /////// 取得某一交易日日期
        /////// n = 1,  当前日          排序的第 1 日
        /////// n = 2,  当前日的前一日   排序的第 2 日
        /////// n = 3,  当前日的前两日   排序的第 3 日
        /////// </summary>
        /////// <param name="stockcode">股票代码</param>
        /////// <returns>股票日期</returns>
        ////public static string getTradeDay(string stockcode, int n)
        ////{
        ////    string tradeday = "";

        ////    string sql = "select rownum, stockdate from ( " +
        ////        "SELECT ROW_NUMBER() OVER (ORDER BY StockDate DESC) AS ROWNUM, StockDate  FROM  STOCKER.DBO.StockData WHERE StockCode = @StockCode " +
        ////        ") as StockDataN where rownum = @RowNum ";

        ////    try
        ////    {
        ////        SqlParameter[] parms = { 
        ////                                   new SqlParameter("@StockCode", SqlDbType.NVarChar, 6),
        ////                                   new SqlParameter("@RowNum", SqlDbType.Int)
        ////                               };

        ////        parms[0].Value = stockcode;
        ////        parms[1].Value = n;

        ////        SqlDataReader reader = SqlHelper.ExcuteReader(CommandType.Text, sql, parms);

        ////        while (reader.Read())
        ////        {
        ////            tradeday = Convert.ToString(reader.GetValue(1));
        ////        }
        ////        reader.Close();
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        writeLog(System.Reflection.MethodBase.GetCurrentMethod() + " : " + stockcode + " : " + ex.Message);
        ////    }

        ////    return tradeday;
        ////}

        /////// <summary>
        /////// 取得配置的量的倍数 n, 从配置的设定中取得
        /////// </summary>
        /////// <returns> n </returns>
        ////public static float getNTimesBuySellRule(string buysellrule)
        ////{
        ////    float n = 1;

        ////    string sql = "SELECT NTimes FROM STOCKER.dbo.BuySellRule WHERE RuleCode = '" + buysellrule + "'";

        ////    try
        ////    {
        ////        SqlDataReader reader = SqlHelper.ExcuteReader(CommandType.Text, sql, null);

        ////        while (reader.Read())
        ////        {
        ////            n = Convert.ToSingle(reader.GetValue(0));
        ////        }
        ////        reader.Close();
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        writeLog(System.Reflection.MethodBase.GetCurrentMethod() + " : " + buysellrule + " : " + ex.Message);
        ////    }

        ////    return n;
        ////}

        /////// <summary>
        /////// 取得配置的M值，
        /////// M为前多少日的平均值,
        /////// M=1时为前一日的量
        /////// </summary>
        /////// <returns></returns>
        ////public static int getMDaysBuySellRule(string buysellrule)
        ////{
        ////    int m = 1;

        ////    string sql = "SELECT MDays FROM STOCKER.dbo.BuySellRule WHERE RuleCode = '" + buysellrule + "'";

        ////    try
        ////    {
        ////        SqlDataReader reader = SqlHelper.ExcuteReader(CommandType.Text, sql, null);

        ////        while (reader.Read())
        ////        {
        ////            m = Convert.ToInt16(reader.GetValue(0));
        ////        }
        ////        reader.Close();
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        writeLog(System.Reflection.MethodBase.GetCurrentMethod() + " : " + buysellrule + " : " + ex.Message);
        ////    }

        ////    return m;
        ////}

        /////// <summary>
        /////// 取得买卖的原因
        /////// </summary>
        /////// <param name="buysellrule">规则</param>
        /////// <returns>详细原因</returns>
        ////public static string getBuyReasonBuySellRule(string buysellrule)
        ////{
        ////    string buyreason = "";

        ////    string sql = "SELECT BuyReason FROM STOCKER.dbo.BuySellRule WHERE RuleCode = '" + buysellrule + "'";

        ////    try
        ////    {
        ////        SqlDataReader reader = SqlHelper.ExcuteReader(CommandType.Text, sql, null);

        ////        while (reader.Read())
        ////        {
        ////            buyreason = Convert.ToString(reader.GetValue(0));
        ////        }
        ////        reader.Close();
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        writeLog(System.Reflection.MethodBase.GetCurrentMethod() + " : " + buysellrule + " : " + ex.Message);
        ////    }

        ////    return buyreason;
        ////}

        /////// <summary>
        /////// 股价下跌一段时间
        /////// </summary>
        /////// <param name="stockcode">股票代码</param>
        /////// <param name="stockdate">股票日期</param>
        /////// <param name="buyrule">买入规则</param>
        /////// <returns></returns>
        ////public static bool priceWentDown(string stockcode, string buyrule)
        ////{
        ////    bool pricedown = false;

        ////    int m = Utility.getMDaysBuySellRule(buyrule); 
        ////    if (buyrule == "BuyRule3")
        ////    { 
        ////       if (m == 0) { m = 6; };                                                             //默认短线取6日均线；中线取18日均线
        ////    }
        ////    else if (buyrule == "BuyRule4")
        ////    {
        ////        if (m == 0) { m = 18; };                                                           //默认中线取18日均线
        ////    }

        ////    string Last1stTradeDate = Utility.getTradeDay(stockcode, 1);                           //当前日
        ////    string last2ndTradeDate = Utility.getTradeDay(stockcode, m);                           //当前日的前6日 where m = rownum
        ////    string last3rdTradeDate = Utility.getTradeDay(stockcode, m + m);                       //当前日的前12日  where m = rownum

        ////    float last1stAveragePrice = Utility.getAveragePrice(stockcode, Last1stTradeDate, m);   //m 日均价      top m
        ////    float last2ndAveragePrice = Utility.getAveragePrice(stockcode, last2ndTradeDate, m);   //前 m 日均价   top m
        ////    float last3rdAveragePrice = Utility.getAveragePrice(stockcode, last3rdTradeDate, m);   //前 2m 日均价  top m

        ////    if (last1stAveragePrice < last2ndAveragePrice)
        ////    {
        ////        if (last2ndAveragePrice < last3rdAveragePrice)
        ////        {
        ////            pricedown = true;
        ////        }
        ////    }            

        ////    return pricedown;
        ////}

        /////// <summary>
        /////// 取得向下跳空发生的日期。
        /////// 如果没有跳空发生，返回空值。
        /////// </summary>
        /////// <param name="stockcode"></param>
        /////// <returns></returns>
        ////public static string getDownGapDay(string stockcode, string buysellrule)
        ////{
        ////    string gapdate = "";
        ////    string Last1stTradeDate = "";                                                          //当前日
        ////    string last2ndTradeDate = "";                                                          //当前日的前一日

        ////    float todayhight = 0;
        ////    float yesterdaylow = 0;

        ////    int ndays = Utility.getMDaysBuySellRule("GapDay");                                     // nday 日前的范围内找放量

        ////    for (int i = 1; i <= ndays; i++)
        ////    {
        ////        Last1stTradeDate = Utility.getTradeDay(stockcode, i);
        ////        last2ndTradeDate = Utility.getTradeDay(stockcode, i + 1);

        ////        todayhight = Utility.getHighPrice(stockcode, Last1stTradeDate);
        ////        yesterdaylow = Utility.getLowPrice(stockcode, last2ndTradeDate);

        ////        if (todayhight * yesterdaylow != 0)
        ////        {
        ////            if (todayhight < yesterdaylow)
        ////            {
        ////                gapdate = Last1stTradeDate;
        ////                break;
        ////            }
        ////        }
        ////    }

        ////    return gapdate;
        ////}

        /////// <summary>
        /////// 取得向上跳空发生的日期。
        /////// 如果没有跳空发生，返回空值。
        /////// </summary>
        /////// <param name="stockcode"></param>
        /////// <returns></returns>
        ////public static string getUpGapDay(string stockcode, string buysellrule)
        ////{
        ////    string gapdate = "";
        ////    string Last1stTradeDate = "";       //当前日
        ////    string last2ndTradeDate = "";    //当前日的前一日

        ////    float todaylow = 0;
        ////    float yesterdayhigh = 0;

        ////    int ndays = Utility.getMDaysBuySellRule("GapDay");

        ////    for (int i = 1; i <= ndays; i++)
        ////    {
        ////        Last1stTradeDate = Utility.getTradeDay(stockcode, i);
        ////        last2ndTradeDate = Utility.getTradeDay(stockcode, i + 1);

        ////        todaylow = Utility.getLowPrice (stockcode, Last1stTradeDate);
        ////        yesterdayhigh = Utility.getHighPrice (stockcode, last2ndTradeDate);

        ////        if (todaylow * yesterdayhigh != 0)
        ////        {
        ////            if (todaylow > yesterdayhigh)
        ////            {
        ////                gapdate = Last1stTradeDate;
        ////                break;
        ////            }
        ////        }
        ////    }

        ////    return gapdate;
        ////}

        /////// <summary>
        /////// 取得实体底部
        /////// </summary>
        /////// <param name="openprice">开盘价</param>
        /////// <param name="closeprice">收盘价</param>
        /////// <returns></returns>
        ////public static float getBodyLowPrice(float openprice, float closeprice)
        ////{
        ////    float price = 0;

        ////    if(openprice < closeprice)                                                             //阳线：开盘价 < 收盘价
        ////    {
        ////        price = openprice;                                                                 //实体底部 = 开盘价
        ////    }
        ////    else                                                                                   //阴线：开盘价 >= 收盘价
        ////    {
        ////        price = closeprice;                                                                //实体底部 = 收盘价
        ////    }

        ////    return price;
        ////}

        /////// <summary>
        /////// 取得实体顶部
        /////// </summary>
        /////// <param name="openprice">开盘价</param>
        /////// <param name="closeprice">收盘价</param>
        /////// <returns></returns>
        ////public static float getBodyHighPrice(float openprice, float closeprice)
        ////{
        ////    float price = 0;

        ////    if (openprice < closeprice)                                                            //阳线：开盘价 < 收盘价
        ////    {
        ////        price = closeprice;                                                                //实体顶部 = 收盘价
        ////    }
        ////    else                                                                                   //阴线：开盘价 >= 收盘价
        ////    {
        ////        price = openprice;                                                                 //实体顶部 = 开盘价
        ////    }

        ////    return price;
        ////}

        /////// <summary>
        /////// 判断股票池中是否存在该股票
        /////// </summary>
        /////// <param name="stockcode">股票代码</param>
        /////// <param name="stockdate">股票日期</param>
        /////// <param name="StatusComment">股票评价</param>
        /////// <returns></returns>
        ////public static bool isInStockPool(string stockcode, string stockdate, string StatusComment)
        ////{
        ////    bool isinstockpool = false;

        ////    string sql = "SELECT COUNT(*) FROM STOCKER.dbo.StockPool WHERE StockCode = @StockCode AND StockDate = @StockDate AND StatusComment = @StatusComment";

        ////    SqlParameter[] parms = {				   
        ////        new SqlParameter("@StockCode", SqlDbType.NVarChar,6),     
        ////        new SqlParameter("@StockDate", SqlDbType.NVarChar,10),
        ////        new SqlParameter("@StatusComment", SqlDbType.NVarChar,1000)
        ////                           };

        ////    parms[0].Value = stockcode;
        ////    parms[1].Value = stockdate;
        ////    parms[2].Value = StatusComment;

        ////    int i = Convert.ToInt32(SqlHelper.ExcuteScalar(CommandType.Text, sql, parms));
        ////    if (i > 0)
        ////    {
        ////        isinstockpool = true;
        ////    }
        ////    else
        ////    {
        ////        isinstockpool = false;
        ////    }

        ////    return isinstockpool;
        ////}

        /////// <summary>
        /////// 更新股票池，标注买入卖出状态
        /////// </summary>
        /////// <param name="stockCode">股票代码</param>
        /////// <param name="stockDate">股票日期</param>
        /////// <param name="reason">买卖原因</param>
        ////public static void UpdateStockPool(string stockCode, string stockDate, string status, string reason)
        ////{
        ////    string exmessage = "";

        ////    try
        ////    {
        ////        string sql = "INSERT INTO STOCKER.dbo.StockPool (StockCode, StockDate,StockStatus,StatusComment) VALUES ( @StockCode, @StockDate,@StockStatus,@StatusComment)";

        ////        SqlParameter[] parms = {				   
        ////                    new SqlParameter("@StockCode", SqlDbType.NVarChar,6),
        ////                    new SqlParameter("@StockDate", SqlDbType.NVarChar,10),
        ////                    new SqlParameter("@StockStatus", SqlDbType.NVarChar,100),
        ////                    new SqlParameter("@StatusComment", SqlDbType.NVarChar,1000)
        ////                };

        ////        parms[0].Value = stockCode;
        ////        parms[1].Value = stockDate;
        ////        parms[2].Value = status;
        ////        parms[3].Value = reason;

        ////        int k = Convert.ToInt32(SqlHelper.ExcuteNonQuery(CommandType.Text, sql, parms));

        ////        if (k <= 0)
        ////        {
        ////            exmessage = "INSERT STOCK DATA FAILED: " + stockCode;
        ////        }

        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        exmessage = System.Reflection.MethodBase.GetCurrentMethod() + " : " + stockCode + " : " + ex.Message;
        ////        Utility.writeLog(exmessage);
        ////    }      
        ////}

        /////// <summary>
        /////// 判断是否是新股
        /////// </summary>
        /////// <param name="stockcode"></param>
        /////// <returns></returns>
        ////public static bool isNewStock(string stockcode)
        ////{
        ////    bool newstock = false;

        ////    int mDays = getMDaysBuySellRule("NewStock");
        ////    float nTimes = getNTimesBuySellRule("NewStock");

        ////    for (int i = 1; i <= mDays; i++)
        ////    {
        ////        string Last1stTradeDay = Utility.getTradeDay(stockcode, i);                        //当前日           ROW NUM
        ////        float todayopen = Utility.getOpenPrice (stockcode, Last1stTradeDay);               //上市当天最高值
        ////        float todayclose = Utility.getClosePrice (stockcode, Last1stTradeDay);             //上市当天最低值

        ////        if (todayopen * todayclose != 0)
        ////        {
        ////            float rate = (todayclose - todayopen) / todayopen;
        ////            if (rate > Convert.ToSingle(nTimes))
        ////            {
        ////                newstock = true;
        ////                break;
        ////            }
        ////        }
        ////    }

        ////    return newstock;
        ////}

        /////// <summary>
        /////// 判断是否是除权股
        /////// </summary>
        /////// <param name="stockcode">股票代码</param>
        /////// <returns></returns>
        ////public static bool isExitRight(string stockcode)
        ////{
        ////    bool isexistright = false;

        ////    int mDays = getMDaysBuySellRule("ExtRight");
        ////    float nTimes = getNTimesBuySellRule("ExtRight");


        ////    for (int i = 1; i <= mDays; i++)
        ////    {
        ////        string Last1stTradeDay = Utility.getTradeDay(stockcode, i);                        //当前日          ROW NUM
        ////        string last2ndTradeDay = Utility.getTradeDay(stockcode, i + 1);                    //前一日          ROW NUM    

        ////        float last1stPrice = Utility.getClosePrice(stockcode, Last1stTradeDay);             //取得当日的收盘价
        ////        float last2ndPrice = Utility.getClosePrice(stockcode, last2ndTradeDay);            //取得前一日的收盘价

        ////        //除权
        ////        if (last1stPrice * last2ndPrice != 0 )
        ////        {                 
        ////            //价格降低 10% 以上，认为是除权股票
        ////            float rate = (last2ndPrice - last1stPrice) / last2ndPrice;

        ////            if (rate > Convert.ToSingle(nTimes))
        ////            {
        ////                isexistright = true;
        ////                break;
        ////            }

        ////        }
        ////    }

        ////    return isexistright;
        ////}


        /////// <summary>
        /////// 取得除权股
        /////// </summary>
        /////// <param name="stockcode">股票代码</param>
        /////// <returns></returns>
        ////public static string getExitRightDay(string stockcode)
        ////{

        ////    string exitdate = "";

        ////    int mDays = getMDaysBuySellRule("ExtRight");
        ////    float nTimes = getNTimesBuySellRule("ExtRight");


        ////    for (int i = 1; i <= mDays; i++)
        ////    {
        ////        string Last1stTradeDay = Utility.getTradeDay(stockcode, i);                        //当前日          ROW NUM
        ////        string last2ndTradeDay = Utility.getTradeDay(stockcode, i + 1);                    //前一日          ROW NUM    

        ////        float last1stPrice = Utility.getClosePrice(stockcode, Last1stTradeDay);             //取得当日的收盘价
        ////        float last2ndPrice = Utility.getClosePrice(stockcode, last2ndTradeDay);            //取得前一日的收盘价

        ////        //除权
        ////        if (last1stPrice * last2ndPrice != 0)
        ////        {
        ////            //价格降低 10% 以上，认为是除权股票
        ////            float rate = (last2ndPrice - last1stPrice) / last2ndPrice;

        ////            if (rate > Convert.ToSingle(nTimes))
        ////            {
        ////                exitdate = Last1stTradeDay;

        ////                break;
        ////            }

        ////        }
        ////    }

        ////    return exitdate;
        ////}

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="log"></param>
        public static void writeLog(string errorNote)
        {
            string errorTime = System.DateTime.Now.ToString();

            try
            {

                string sql = "INSERT INTO dbo.ErrorLog ( ErrorTime, ErrorNote ) VALUES ( @ErrorTime, @ErrorNote )";

                SqlParameter[] parms = {
                new SqlParameter("@ErrorTime", SqlDbType.NVarChar,50),
                new SqlParameter("@ErrorNote", SqlDbType.NVarChar, 1000)
                };

                parms[0].Value = errorTime;
                parms[1].Value = errorNote;

                int i = Convert.ToInt32(SqlHelper.ExcuteNonQuery(CommandType.Text, sql, parms));
            }
            catch
            {
                return;
            }
        }



        /// <summary>
        /// 判断奇数偶数
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static bool IsOdd(int n)
        {
            return (n % 2 == 1) ? true : false;
        }

    }
}
