using MathNet.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreeksCalculations.Models
{
    public static class OptionCalculator
    {
        // Validation to prevent invalid inputs
        private static void ValidateInputs(OptionParameters parameters)
        {
            if (parameters.UnderlyingPrice <= 0)
                throw new ArgumentException("Underlying price must be greater than zero.");

            if (parameters.StrikePrice <= 0)
                throw new ArgumentException("Strike price must be greater than zero.");

            if (parameters.TimeToExpiration <= 0)
                throw new ArgumentException("Time to expiration must be greater than zero.");

            if (parameters.Volatility <= 0)
                throw new ArgumentException("Volatility must be greater than zero.");

            if (Math.Sqrt(parameters.TimeToExpiration) <= 0)
                throw new ArgumentException("Square root of time to expiration must be greater than zero.");
        }


        // Compute d1 and d2 for Black-Scholes formula
        private static (double d1, double d2) GetD1D2(OptionParameters parameters)
        {
            double sqrtT = Math.Sqrt(parameters.TimeToExpiration);
            double d1 = (Math.Log(parameters.UnderlyingPrice / parameters.StrikePrice) +
                         (parameters.RiskFreeRate - parameters.DividendYield + 0.5 * Math.Pow(parameters.Volatility, 2)) * parameters.TimeToExpiration)
                         / (parameters.Volatility * sqrtT);

            double d2 = d1 - parameters.Volatility * sqrtT;
            return (d1, d2);
        }

        // Delta Calculation
        public static double GetDelta(OptionParameters parameters)
        {
            ValidateInputs(parameters);
            var (d1, _) = GetD1D2(parameters);
            double Nd1 = NormalCDF(d1);

            double delta;
            if (parameters.OptionType == OptionType.Call)
            {
                delta = Math.Exp(-parameters.DividendYield * parameters.TimeToExpiration) * Nd1;
            }
            else
            {
                delta = Math.Exp(-parameters.DividendYield * parameters.TimeToExpiration) * (Nd1 - 1);
            }
            return delta;
        }

        // Gamma Calculation
        public static double GetGamma(OptionParameters parameters)
        {
            ValidateInputs(parameters);
            var (d1, _) = GetD1D2(parameters);

            double gama = (Math.Exp(-parameters.DividendYield * parameters.TimeToExpiration) * NormalPDF(d1)) /
                            (parameters.UnderlyingPrice * parameters.Volatility * Math.Sqrt(parameters.TimeToExpiration));
            return gama;
        }

        // Vega Calculation
        public static double GetVega(OptionParameters parameters)
        {
            ValidateInputs(parameters);
            var (d1, _) = GetD1D2(parameters);

            double vega = (parameters.UnderlyingPrice * Math.Exp(-parameters.DividendYield * parameters.TimeToExpiration) * NormalPDF(d1) * Math.Sqrt(parameters.TimeToExpiration)) / 100 ;
            return vega;
        }

        // Theta Calculation
        public static double GetTheta(OptionParameters parameters)
        {
            ValidateInputs(parameters);
            var (d1, d2) = GetD1D2(parameters);

            double term1 = (-parameters.UnderlyingPrice * NormalPDF(d1) * parameters.Volatility * Math.Exp(-parameters.DividendYield * parameters.TimeToExpiration)) /
                            (2 * Math.Sqrt(parameters.TimeToExpiration));

            double term2Call = parameters.RiskFreeRate * parameters.StrikePrice * Math.Exp(-parameters.RiskFreeRate * parameters.TimeToExpiration) * NormalCDF(d2);
            double term2Put = parameters.RiskFreeRate * parameters.StrikePrice * Math.Exp(-parameters.RiskFreeRate * parameters.TimeToExpiration) * NormalCDF(-d2);

            double theta;
            if (parameters.OptionType == OptionType.Call)
            {
                theta = (term1 - term2Call) / 365;
            }
            else
            {
                theta = (term1 + term2Put) / 365;
            }

            return theta;
        }

        // Normal Distribution CDF
        private static double NormalCDF(double x)
        {
            return 0.5 * (1.0 + SpecialFunctions.Erf(x / Math.Sqrt(2.0)));
        }
        // Normal Probability Density Function (PDF)
        private static double NormalPDF(double x)
        {
            return (1.0 / Math.Sqrt(2 * Math.PI)) * Math.Exp(-0.5 * x * x);
        }

    }
}
