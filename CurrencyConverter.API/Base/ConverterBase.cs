using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CurrencyConverter.API
{
    public class ConverterBase
    {
        HttpClient _client = null;

        public ConverterBase()
        {
            _client = new HttpClient();

        }

        public string Get(string url)
        {
            HttpResponseMessage response = _client.GetAsync(url).Result;
            response.EnsureSuccessStatusCode();
            using (HttpContent content = response.Content)
            {
                return response.Content.ReadAsStringAsync().Result;
            }
        }

        public string Currencies
        {
            get
            {
                return @"AUD,BGN,BRL,CAD,CHF,CNY,CZK,DKK,GBP,HKD,HRK,HUF,IDR,ILS,INR,JPY,KRW,MXN,MYR,NOK,NZD,PHP,PLN,RON,RUB,SEK,SGD,THB,TRY,ZAR,EUR";
            }
        }

        public BaseEntity GetEntity(string base_, string date, Dictionary<string, double>  rates)
        {
                return new BaseEntity()
                {
                    Base = base_,
                    Date = date,
                    Rates = rates
                };
        }
    }

    public class Rate
    {
        [JsonProperty(PropertyName = "name")]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "value")]
        public string Amount { get; set; }
    }

    public class BaseEntity : ICurrencyEntity
    {
        [JsonProperty(PropertyName = "base")]
        public string Base { get; set; }
        [JsonProperty(PropertyName = "date")]
        public string Date { get; set; }
        [JsonProperty(PropertyName = "rates")]
        public Dictionary<string, double> Rates { get; set; }

    }

}
