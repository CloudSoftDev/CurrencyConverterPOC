using CurrencyConverter.API.Vendor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConverter.API.Factory
{
    internal class VendorDictionary
    {
        private static Dictionary<string, Type> _vendors;

        public static Type GetVendor(string vendorCode)
        {
            vendorCode = vendorCode.ToUpper();
            if (_vendors == null)
                BuildVendor();
            if (_vendors[vendorCode] != null)
                return _vendors[vendorCode];
            else
                throw new Exception("Invalid vendor code");

        }

        private static void BuildVendor()
        {
            _vendors = new Dictionary<string, Type>();

            //Add all vendor over here..
            _vendors.Add("FIX", typeof(Fixer));
            _vendors.Add("CUL", typeof(CurrencyLayer));

        }
    }
}
