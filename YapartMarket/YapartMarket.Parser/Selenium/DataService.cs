using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using YapartMarket.Data;
using YapartMarket.Parser.Data.Models;

namespace YapartMarket.Parser.Selenium
{
    public class DataService : IDataService
    {
        private readonly IWebDriver _browser;
        private readonly string _login;
        private readonly string _pass;

        private ReadOnlyCollection<IWebElement> AllRowSellersWithoutAnalogs { get; set; }

        public DataService()
        {
            using (var r = new StreamReader("appsettings.json"))
            {
                var json = r.ReadToEnd();
                _login = JsonConvert.DeserializeObject<AppSettings>(json).Login;
                _pass = JsonConvert.DeserializeObject<AppSettings>(json).Password;
            }
            _browser = new ChromeDriver(@"C:\YapartStore\YapartMarket\YapartMarket.Parser\bin\Debug\netcoreapp3.1");
        }

        public void SetPage(string url)
        {
            _browser.Navigate().GoToUrl(url);
        }

        public void SingUp()
        {
            try
            {
                _browser.FindElement(By.CssSelector("input#UserName")).SendKeys(_login);
                _browser.FindElement(By.CssSelector("input#Password")).SendKeys(_pass);
                _browser.FindElement(By.CssSelector("input#LoginButton")).Click();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SearchArticulInSearchInput(string article)
        {
            try
            {
                _browser.FindElement(By.CssSelector("input#SearchNumber")).Clear();
                _browser.FindElement(By.CssSelector("input#SearchNumber")).SendKeys(article);
                Thread.Sleep(1000);
                _browser.FindElement(By.CssSelector(".search-line-group > input#btnSearchCatalog")).Click();

                ClickNotBot();
            }
            catch (NoSuchElementException ex)
            {

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ChooseAnalogs(string brand)
        {
            Thread.Sleep(2000);
            var analogsListLinks = _browser.FindElements(By.CssSelector(".w-tbl table tbody tr td:nth-child(2)"));
            foreach (var analog in analogsListLinks)
            {
                if (brand.ToLower() == analog.Text.ToLower())
                {
                    ClickNotBot();
                    analog.Click();
                    return true;
                }
            }
            return false;
        }

        public void ClickNotBot()
        {
            try
            {
                var cantBot = _browser.FindElement(By.CssSelector("button.g-recaptcha.btn"));
                cantBot.Click();

            }
            catch (NoSuchElementException ex)
            {

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ArticleDTO GetInfoAboutArticle()
        {
            Thread.Sleep(3000);
            var brand = string.Empty;
            var article = string.Empty;
            var descriptions = string.Empty;

            var articleDTO = new ArticleDTO();

            if (PartIsFound(3) == true)
            {
                try
                {
                    brand = _browser.FindElement(By.XPath("//table[@id='tGrid']//tbody/tr[2]/td[3]/div")).Text;
                    article = _browser.FindElement(By.XPath("//table[@id='tGrid']//tbody/tr[2]/td[4]")).Text;
                    descriptions = _browser.FindElement(By.XPath("(//table[@id='tGrid']//tbody/tr[@class='sel'])[1]/td[6]")).Text;
                }
                catch (NoSuchElementException ex)
                {

                }
                catch (Exception ex)
                {
                    throw ex;
                }

                articleDTO.Brand = brand;
                articleDTO.Article = article;
                articleDTO.Descriptions = descriptions;


                GetAllRowSellersWithoutAnalogs();
                var topThreeSellersInfo = GetTopThreeTrader();

                articleDTO.ListSellers = topThreeSellersInfo;
                articleDTO.Yapart = GetYapartCatalogs();
            }
            else
            {
                articleDTO.ListSellers = null;
                articleDTO.Yapart = null;
            }

            return articleDTO;
        }

        public IList<ShortInfo> GetYapartCatalogs()
        {
            var yapartCatalogShortInfo = new List<IWebElement>();
            foreach (var row in AllRowSellersWithoutAnalogs)
            {
                var classAttribute = row.GetAttribute("class");
                if (classAttribute == "sel")
                    yapartCatalogShortInfo.Add(row);
            }
            return GetShotrInfo(yapartCatalogShortInfo);
        }

        public bool PartIsFound(int repeat)
        {
            var notFoundString = _browser.FindElement(By.CssSelector("div.u-product.forHide > div")).Text;
            if (repeat == 0)
            {
                return false;
            }
            if (notFoundString == "Деталь не найдена")
            {
                _browser.Navigate().Refresh();
                Thread.Sleep(1000);
                return PartIsFound(repeat - 1);
            }
            else
            {
                return true;
            }
        }

        public void GetAllRowSellersWithoutAnalogs()
        {
            try
            {
                var IsAnalogs = _browser.FindElements(By.XPath("//table[@id='tGrid']/tbody/tr[@class='a-grouping-row']"));
                //not analogs
                if (IsAnalogs.Count == 1)
                {
                    var numberOfSellersTr = _browser.FindElements(By.XPath("//table[@id='tGrid']/tbody/tr[preceding-sibling::tr[@class='a-grouping-row']]"));
                    if (numberOfSellersTr != null)
                        AllRowSellersWithoutAnalogs = numberOfSellersTr;
                    else AllRowSellersWithoutAnalogs = null;
                }
                //true analogs
                else if (IsAnalogs.Count == 2)
                {
                    var numberOfSellersTr = _browser.FindElements(By.XPath("//table[@id='tGrid']/tbody/tr[preceding-sibling::tr[@class='a-grouping-row']][following-sibling::tr[@class='a-grouping-row']]"));
                    if (numberOfSellersTr != null)
                        AllRowSellersWithoutAnalogs = numberOfSellersTr;
                    else AllRowSellersWithoutAnalogs = null;
                }
                if (IsAnalogs.Count == 3)
                {
                    ComplexCondition();
                    //var numberOfSellersTr = _browser.FindElements(By.XPath("//table[@id='tGrid']/tbody/tr[preceding-sibling::tr[@class='a-grouping-row']][following-sibling::tr[@class='a-grouping-row']]"));                    
                    //var listElements = new List<IWebElement>();
                    //int indexStartProduct = 0;
                    //for (int i = 0; i < numberOfSellersTr.Count; i++)
                    //{
                    //    var classAttribute = numberOfSellersTr[i].GetAttribute("class");
                    //    if(classAttribute == "a-grouping-row")
                    //    {
                    //        indexStartProduct = i;
                    //    }
                    //}
                    //for (int i = indexStartProduct + 1; i < numberOfSellersTr.Count; i++)
                    //{
                    //    listElements.Add(numberOfSellersTr[i]);
                    //}
                    //AllRowSellersWithoutAnalogs = new ReadOnlyCollection<IWebElement>(listElements);
                }

            }
            catch (NoSuchElementException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void ComplexCondition()
        {
            try
            {
                var firstGroupRowText = _browser.FindElement(By.XPath("//table[@id='tGrid']/tbody/tr[@class='a-grouping-row'][1]/td")).Text;
                var resultText = firstGroupRowText.Substring(0, firstGroupRowText.IndexOf("\r\n"));

                if (resultText == "Запрошенный номер на нашем складе")
                {
                    var sellerTr = _browser.FindElements(By.XPath("//table[@id='tGrid']/tbody/tr[preceding-sibling::tr[@class='a-grouping-row'][2]][following-sibling::tr[@class='a-grouping-row']]"));
                    if (sellerTr == null)
                        AllRowSellersWithoutAnalogs = null;
                    AllRowSellersWithoutAnalogs = sellerTr;
                }
                if (resultText == "Запрошенный номер")
                {
                    var sellerTr = _browser.FindElements(By.XPath("//table[@id='tGrid']/tbody/tr[preceding-sibling::tr[@class='a-grouping-row'][1]][following-sibling::tr[@class='a-grouping-row'][2]]"));
                    if (sellerTr == null)
                        AllRowSellersWithoutAnalogs = null;
                    AllRowSellersWithoutAnalogs = sellerTr;
                }
            }
            catch (NoSuchElementException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public IList<ShortInfo> GetTopThreeTrader()
        {
            try
            {
                if (AllRowSellersWithoutAnalogs != null)
                {
                    if (AllRowSellersWithoutAnalogs.Count >= 3)
                    {
                        var firstThreeRows = AllRowSellersWithoutAnalogs.Take(3).ToList();
                        return GetShotrInfo(firstThreeRows);
                    }
                    if (AllRowSellersWithoutAnalogs.Count == 1 || AllRowSellersWithoutAnalogs.Count == 2)
                    {
                        var nRows = AllRowSellersWithoutAnalogs.Take(AllRowSellersWithoutAnalogs.Count).ToList();
                        return GetShotrInfo(nRows);
                    }
                }
                return null;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IList<ShortInfo> GetShotrInfo(IEnumerable<IWebElement> tradersTr)
        {
            var shortInfo = new List<ShortInfo>();
            Regex regex = new Regex("^[0-9]{1,}");
            try
            {
                foreach (var trader in tradersTr)
                {
                    var traderInfo = trader.FindElements(By.XPath("td[position() >6 and position() <= 9]"));
                    var inStock = 0;
                    if (!int.TryParse(traderInfo[0].Text, out inStock))
                        inStock = 0;
                    else
                        inStock = Convert.ToInt16(traderInfo[0].Text);

                    var info = new ShortInfo()
                    {
                        Price = Convert.ToDouble(regex.Match(traderInfo[2].Text).Value),
                        InStock = inStock,
                        DeliveryDays = Convert.ToInt16(traderInfo[1].Text)
                    };
                    shortInfo.Add(info);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return shortInfo;
        }

        public void BrowserQuit()
        {
            _browser.Quit();
        }

        //public int DetermineNumberGoodsSold()
        //{
        //    try
        //    {
        //        var IsAnalogs = _browser.FindElements(By.XPath("//table[@id='tGrid']/tbody/tr[@class='a-grouping-row']"));
        //        //not analogs
        //        if (IsAnalogs.Count == 1)
        //        {
        //            return _browser.FindElements(By.XPath("//table[@id='tGrid']/tbody/tr[@class='sel']")).Count;
        //        }
        //        else if (IsAnalogs.Count == 2)
        //        {
        //            var numberOfSellersTr = _browser.FindElements(By.XPath("//table[@id='tGrid']/tbody/tr[preceding-sibling::tr[@class='a-grouping-row']][following-sibling::tr[@class='a-grouping-row']]"));
        //            var repeatedGood = 0;
        //            foreach (var seller in numberOfSellersTr)
        //            {
        //                if (seller.GetAttribute("class") == "sel")
        //                {
        //                    repeatedGood++;
        //                }
        //            }
        //            return repeatedGood;
        //        }
        //    }
        //    catch (NoSuchElementException ex)
        //    {
        //        return 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return 0;
        //}
    }
}
