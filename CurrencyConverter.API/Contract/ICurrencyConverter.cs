using System;
using System.Collections.Generic;

namespace CurrencyConverter.API
{
    public interface ICurrencyConverter
    {
        string GetLatest(string baseCurrency);
        string GetLatest(string fromCurrency, string toCurrency);
        string GetByData(string baseCurrency, DateTime date);
        string GetByData(string fromCurrency, string toCurrency, DateTime date);
    }

 
    interface ICurrencyEntity
    {
        string Base
        {
            get;
            set;
        }

        string Date
        {
            get;
            set;
        }


        Dictionary<string, double> Rates
        {
            get;
            set;
        }
    }


}
