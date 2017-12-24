using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConverter.API.Factory
{
    public class CurrencyFactory
    {
        private static object _lock = new object();

        public static ICurrencyConverter GetCurrencyConverter(string vendorCode)
        {
            lock (_lock)
            {
                try
                {
                    return (ICurrencyConverter)Activator.CreateInstance(VendorDictionary.GetVendor(vendorCode));
                }
                catch (Exception)
                {

                    throw;
                }
            }
           
        }
    }
}
