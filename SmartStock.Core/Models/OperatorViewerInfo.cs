using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStock.Core.Models
{
    public class OperatorViewerSummary
    {
        public OperatorViewChart OperatorViewChartSummary { get; set; }
        public List<OperatorViewerTable> OperatorViewerTableSummary { get; set; }
    }

    /// <summary>
    /// 定义用于画 Operator 盈利曲线图的变量
    /// </summary>
    public class OperatorViewChart
    {
        public List<string> TradingDateList { get; set; }
        public List<decimal> DailyProfitList { get; set; }
    }

    /// <summary>
    /// 定义用于写 Operator 盈利表格的变量
    /// </summary>
    public class OperatorViewerTable
    {
        public string TradingDate { get; set; }
        public string StockCode { get; set; }
        public string StockName { get; set; }
        public decimal ProfitPerStock { get; set; }
    }
}
