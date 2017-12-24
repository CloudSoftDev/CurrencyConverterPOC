using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CurrencyConverter.API;

namespace CurrencyConverter.Test
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestFixer()
        {
            ICurrencyConverter fixer = new API.Vendor.Fixer();
            var Test1 =fixer.GetLatest("USD");
        }

        
    }
}
