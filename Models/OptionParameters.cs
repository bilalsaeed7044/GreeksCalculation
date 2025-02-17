using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreeksCalculations.Models
{
    public class OptionParameters
    {
        public double UnderlyingPrice { get; set; }
        public double StrikePrice { get; set; }
        public double TimeToExpiration { get; set; }
        public double RiskFreeRate { get; set; }  
        public double DividendYield { get; set; }
        public double Volatility { get; set; }   
        public OptionType OptionType { get; set; }
    }
}
