using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CurrencyConverter.API.Vendor
{
    public class Fixer : ConverterBase, ICurrencyConverter
    {
        StringBuilder _urlSB = new StringBuilder();
        static string _url = string.Empty;

        public Fixer()
        {
            _urlSB.Clear();

            if (string.IsNullOrEmpty(_url))
                _url = System.Configuration.ConfigurationManager.AppSettings["FixerURL"];

            BuildURL(_url);
            if (!_url.EndsWith("/"))
                BuildURL("/");
        }

        public string GetByData(string baseCurrency, DateTime date)
        {
            try
            {
                BuildURL("latest?");
                BuildURL("base=" + baseCurrency);
                BuildURL("&date=" + date.ToString("yyyy-MM-dd"));

                var data = RequestData();

                return JsonConvert.SerializeObject(data);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string GetByData(string fromCurrency, string toCurrency, DateTime date)
        {
            try
            {
                var match = new Regex(@"[A-z]\d", RegexOptions.IgnoreCase);

                double fValue = 0.0;
                double tValue = 0.0;

                if (match.Match(fromCurrency).Success)
                {
                    fValue = Convert.ToDouble(fromCurrency.Remove(0, 3));
                    fromCurrency = fromCurrency.Remove(3, fromCurrency.Length - 3);
                }

                if (match.Match(toCurrency).Success)
                {
                    tValue = Convert.ToDouble(toCurrency.Remove(0, 3));
                    toCurrency = toCurrency.Remove(3, toCurrency.Length - 3);
                }

                if (tValue > 0 && fValue > 0)
                    throw new Exception("Invalid request");



                BuildURL("latest?");
                BuildURL("symbols=" + fromCurrency + "," + toCurrency);
                BuildURL("&base=" + fromCurrency);
                BuildURL("&date=" + date.ToString("yyyy-MM-dd"));

                var data = RequestData();

                data.Rates.Add(fromCurrency, fValue==0?1:fValue);

                if (fValue > 0)
                    data.Rates[toCurrency] = Math.Round((double)(data.Rates[toCurrency] * fValue), 2);
               

                return JsonConvert.SerializeObject(data);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string GetLatest(string baseCurrency)
        {
            try
            {
                return GetByData(baseCurrency, DateTime.Now);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public string GetLatest(string fromCurrency, string toCurrency)
        {
            return GetByData(fromCurrency, toCurrency, DateTime.Now);
        }

        #region Helpre
        private void BuildURL(string value)
        {
            _urlSB.Append(value);
        }
        private ICurrencyEntity RequestData()
        {
            try
            {
                string result = Get(_urlSB.ToString());
                BaseEntity baseData = null;
                if (!string.IsNullOrEmpty(result))
                {
                    if (result.ToLower().Contains("error"))
                        throw new Exception("Unable to process the request");
                    FixerEntity data = JsonConvert.DeserializeObject<FixerEntity>(result);
                    baseData = GetEntity(data.Base, data.Date, data.Rates);
                }
                return baseData;
            }
            catch (Exception)
            {
                throw;
            }

        }

        #endregion
    }

    public class FixerEntity : ICurrencyEntity
    {
        public FixerEntity()
        {
            Rates = new Dictionary<string, double>();
        }
        [JsonProperty(PropertyName = "base")]
        public string Base { get; set; }
        [JsonProperty(PropertyName = "date")]
        public string Date { get; set; }
        [JsonProperty(PropertyName = "rates")]
        public Dictionary<string, double> Rates { get; set; }


    }
}
