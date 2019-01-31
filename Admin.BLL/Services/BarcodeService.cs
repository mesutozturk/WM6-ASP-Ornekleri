using System;
using System.Linq;
using HtmlAgilityPack;

namespace Admin.BLL.Services
{
    public class BarcodeService
    {
        public void Get(string barcode)
        {
            var url = $"http://www.barkodid.com/{barcode}";
            var web = new HtmlWeb();
            var doc = web.Load(url);
            var section = doc.DocumentNode.SelectNodes("//div[contains(@class,'product-details')]").FirstOrDefault();
            var bc = section.Element("h1").InnerText.Substring(4);
            var name  = section.Element("h4").InnerText.Trim();
            var priceT = section.SelectNodes("//div[contains(@class,'product-text')]").FirstOrDefault().Element("span").InnerText.Trim();
            var price = Convert.ToDecimal(priceT.Substring(0, priceT.Length - 3).Replace(".",","));
        }
    }
}
