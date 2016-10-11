using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStock.Core.Models
{
    public class ClientViewerSummary
    {
        public ClientViewChart ClientViewChartSummary { get; set; }
        public List<ClientViewTable> ClientViewTableSummary { get; set; }
    }

    /// <summary>
    /// 定义用于画 Client 盈利曲线图的变量
    /// </summary>
    public class ClientViewChart
    {
        public List<string> TradingDateList { get; set; }
        public List<decimal> DailyProfitList { get; set; }
    }

    /// <summary>
    /// 定义用于写 Client 盈利表格的变量
    /// </summary>
    public class ClientViewTable
    {
        public string TradingDate { get; set; }
        public string StockCode { get; set; }
        public string StockName { get; set; }
        public decimal ProfitPerStock { get; set; }
    }
}
