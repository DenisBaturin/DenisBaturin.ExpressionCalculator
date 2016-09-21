using System.Collections.Generic;

namespace DenisBaturin.ExpressionCalculator
{
    public class ResultInfo
    {

        public ResultInfo()
        {
            TraceResult = new List<TraceResultItem>();
        }

        public decimal Answer { get; set; }

        public List<TraceResultItem> TraceResult { get; set; }
    }
}