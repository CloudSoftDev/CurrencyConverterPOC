using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CurrencyConverter.API.Vendor
{
    public class CurrencyLayer : ConverterBase, ICurrencyConverter
    {
        StringBuilder _urlSB = new StringBuilder();
        static string _url = string.Empty;
        static string _accessKey = "access_key={0}";

        public CurrencyLayer()
        {
            _urlSB.Clear();

            if (string.IsNullOrEmpty(_url))
            {
                _url = System.Configuration.ConfigurationManager.AppSettings["CurrencyLayerURL"];
                _accessKey = string.Format(_accessKey, System.Configuration.ConfigurationManager.AppSettings["CurrencyLayer_Access_Key"]);
            }

            BuildURL(_url);
            if (!_url.EndsWith("/"))
                BuildURL("/");
        }

        public string GetByData(string baseCurrency, DateTime date)
        {
            try
            {
                BuildURL("historical?");
                BuildURL(_accessKey);
                BuildURL("&currencies=" + Currencies);
                BuildURL("&source=" + baseCurrency);
                BuildURL("&date=" + date.ToString("yyyy-MM-dd"));
                BuildURL("&format=1");


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



                BuildURL("historical?");
                BuildURL(_accessKey);
                BuildURL("&currencies=" + fromCurrency);
                BuildURL("&source=" + toCurrency);
                BuildURL("&date=" + date.ToString("yyyy-MM-dd"));
                BuildURL("&format=1");


                var data = RequestData();
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
                BuildURL("live?");
                BuildURL(_accessKey);
                BuildURL("&currencies=" + Currencies);
                BuildURL("&source=" + baseCurrency);
                BuildURL("&format=1");


                var data = RequestData();
                return JsonConvert.SerializeObject(data);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string GetLatest(string fromCurrency, string toCurrency)
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



                BuildURL("live?");
                BuildURL(_accessKey);
                BuildURL("&currencies=" + fromCurrency);
                BuildURL("&source=" + toCurrency);
                BuildURL("&format=1");


                var data = RequestData();
                return JsonConvert.SerializeObject(data);
            }
            catch (Exception)
            {

                throw;
            }
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
                    if (!result.ToLower().Contains("\"success\":true"))
                        throw new Exception("Unable to process the request");
                    CurrencyLayerEntity data = JsonConvert.DeserializeObject<CurrencyLayerEntity>(result);

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

    public class CurrencyLayerEntity : ICurrencyEntity
    {
        public CurrencyLayerEntity()
        {
            Rates = new Dictionary<string, double>();
        }
        [JsonProperty(PropertyName = "source")]
        public string Base { get; set; }
        [JsonProperty(PropertyName = "date")]
        public string Date { get; set; }
        [JsonProperty(PropertyName = "quotes")]
        public Dictionary<string, double> Rates { get; set; }
    }
}
