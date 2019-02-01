using System;
using System.Linq;
using Admin.Models.Models;
using HtmlAgilityPack;

namespace Admin.BLL.Services
{
    public class BarcodeService
    {
        public BarcodeResult Get(string barcode)
        {
            var url = $"http://www.barkodid.com/{barcode}";
            try
            {
                var web = new HtmlWeb();
                var doc = web.Load(url);
                var section = doc.DocumentNode.SelectNodes("//div[contains(@class,'product-details')]").FirstOrDefault();
                var bc = section?.Element("h1").InnerText.Substring(4);
                var name = section?.Element("h4").InnerText.Trim();
                var priceT = section?.SelectNodes("//div[contains(@class,'product-text')]").FirstOrDefault()?.Element("span").InnerText.Trim();
                var price = Convert.ToDecimal(priceT?.Substring(0, priceT.Length - 3).Replace(".", ","));
                return new BarcodeResult()
                {
                    Barcode = barcode,
                    Name = name,
                    Price = price
                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
