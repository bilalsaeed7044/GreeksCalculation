using GreeksCalculations.Models;


OptionParameters optionParams = new OptionParameters
{
    //UnderlyingPrice = 36.07,
    UnderlyingPrice = 35.00,
    StrikePrice = 35.00,
    TimeToExpiration = 0.0712,
    RiskFreeRate = 0.01,  
    DividendYield = 0,
    Volatility = 0.4825,  
   // Volatility = 0.5825,  
    OptionType = OptionType.Put
};

double delta = OptionCalculator.GetDelta(optionParams);
double gamma = OptionCalculator.GetGamma(optionParams);
double vega = OptionCalculator.GetVega(optionParams);
double theta = OptionCalculator.GetTheta(optionParams);

// Print results
Console.WriteLine($"Delta: {Math.Round(delta, 4)}");
Console.WriteLine($"Gamma: {Math.Round(gamma,4)}");
Console.WriteLine($"Vega: {Math.Round(vega, 4)}");
Console.WriteLine($"Theta: {Math.Round(theta, 4)}");