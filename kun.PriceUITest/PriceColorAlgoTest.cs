using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using kun.PriceUI;
using kun.PriceUI.Converter;
using kun.PriceUI.Resource;


namespace kun.PriceUITest
{
    [TestClass]
    public class PriceColorAlgoTest
    {
        [TestMethod]
        public void TestPriceGoHigher()
        {
            var price = new Price()
            {
                AskPx = 1,
                BidPx = 1
            };
            price.AskPx = 2;
            price.BidPx = 2;
            var b = new PriceCellBackgroundConverter().Convert(new []{ (object)price.AskPx, (object)price.AskPxOld}, null, null, null);
            Assert.AreEqual("#FF00B0F0", b.ToString());
        }

        [TestMethod]
        public void TestPriceGoLower()
        {
            var price = new Price()
            {
                AskPx = 2,
                BidPx = 2
            };
            price.AskPx = 1;
            price.BidPx = 1;
            var b = new PriceCellBackgroundConverter().Convert(new[] { (object)price.AskPx, (object)price.AskPxOld }, null, null, null);
            Assert.AreEqual("#FFFF0000", b.ToString());
        }

        [TestMethod]
        public void TestPriceGoEqual()
        {
            var price = new Price()
            {
                AskPx = 1,
                BidPx = 1
            };
            price.AskPx = 1;
            price.BidPx = 1;
            var b = new PriceCellBackgroundConverter().Convert(new[] { (object)price.AskPx, (object)price.AskPxOld }, null, null, null);
            Assert.AreEqual("#FFFFFFFF", b.ToString());
        }
    }
}
