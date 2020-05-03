using System.Collections.Generic;
using System.Linq;
using Xunit;
using YapartMarket.Parser.Data;
using YapartMarket.Parser.Data.Models;
using YapartMarket.Parser.Selenium;

namespace YapartMarket.UnitTests.YapartMarket.Parser
{
    public class SeleniumTests
    {
        private readonly DataService _service;
        public SeleniumTests()
        {
            _service = new DataService();
        }
        //[TestMethod]
        //public void GetUrlTest()
        //{
        //    _service.SetPage("https://www.yandex.ru/");
        //}

        //[TestMethod]
        //public void SingUpTest()
        //{
        //    _service.SetPage("http://www.autopiter.ru/");
        //    _service.SingUp();
        //}

        //[TestMethod]
        //public void FindArticleTest()
        //{            
        //    _service.SetPage("http://www.autopiter.ru/");
        //    _service.SingUp();
        //    _service.FindArticle("AAAA");
        //}

        [Fact]
        public void SeleniumTest_ChooseAnalogTest()
        {
            _service.SetPage("http://www.autopiter.ru/");
            _service.SingUp();


            _service.SearchArticulInSearchInput("021483");
            _service.ChooseAnalogs("Sheriff");
            var result = _service.GetInfoAboutArticle();

        }

        [Fact]
        public void SeleniumTest_GetInfo()
        {

            AccessInputRepository repository = new AccessInputRepository();
            var data = repository.GetAll();
            _service.SetPage("http://www.autopiter.ru/");
            _service.SingUp();

            List<ArticleDTO> listResult = new List<ArticleDTO>();

            foreach (var product in data)
            {
                _service.SearchArticulInSearchInput(product.Article);
                _service.ChooseAnalogs(product.Brand);
                listResult.Add(_service.GetInfoAboutArticle());
                if (product.Id == 100)
                {
                    break;
                }
            }

            _service.BrowserQuit();
        }

        [Fact]
        public void SeleniumTest_ComplexCondition()
        {
            _service.SetPage("http://www.autopiter.ru/");
            _service.SingUp();


            _service.SearchArticulInSearchInput("021483");
            _service.ChooseAnalogs("Sheriff");
            var result1 = _service.GetInfoAboutArticle();

            _service.SearchArticulInSearchInput("022330");
            _service.ChooseAnalogs("Sheriff");
            var result2 = _service.GetInfoAboutArticle();

        }

        [Fact]
        public void SeleniumTest_ProductNotAvailable()
        {
            _service.SetPage("http://www.autopiter.ru/");
            _service.SingUp();


            _service.SearchArticulInSearchInput("LO-12-51");
            if (_service.ChooseAnalogs("Norplast"))
            {

                var result1 = _service.GetInfoAboutArticle();
            }
            _service.SearchArticulInSearchInput("LO-12-51");
            if (_service.ChooseAnalogs("baltex"))
            {
                var result2 = _service.GetInfoAboutArticle();
            }
            else
            {
                var bb = 0;
            }

        }

        public List<ArticleDTO> SeleniumTest_TaskGetData(List<Product> data, int skip, int take)
        {
            var _service = new DataService();
            _service.SetPage("http://www.autopiter.ru/");
            _service.SingUp();

            var listProductsTmp = new List<Product>();

            if (skip == 0)
                listProductsTmp = data.Take(take).ToList();
            else
                listProductsTmp = data.Skip(skip * take).Take(take).ToList();

            var resultList = new List<ArticleDTO>();
            foreach (var product in listProductsTmp)
            {
                _service.SearchArticulInSearchInput(product.Article);
                _service.ChooseAnalogs(product.Brand);
                resultList.Add(_service.GetInfoAboutArticle());
            }
            return resultList;
        }

        [Fact]
        public void SeleniumTest_GetInfoAndAddToTable()
        {

            AccessInputRepository repository = new AccessInputRepository();
            List<ArticleDTO> listProducts = new List<ArticleDTO>();
            var data = repository.GetAll();

            _service.SetPage("http://www.autopiter.ru/");
            _service.SingUp();

            foreach (var product in data)
            {
                _service.SearchArticulInSearchInput(product.Article);
                if (_service.ChooseAnalogs(product.Brand))
                {
                    listProducts.Add(_service.GetInfoAboutArticle());
                }
            }

            #region tasks
            //var numberRow = data.Count;
            //double rorTwo = (double)numberRow / 2;

            //var taskList = new List<Task<List<ArticleDTO>>>();

            //if (rorTwo % 1 == 0)
            //{
            //    taskList.Add(new Task<List<ArticleDTO>>(() => { return TaskGetData(data.ToList(), 0, (int)rorTwo); }));
            //    taskList.Add(new Task<List<ArticleDTO>>(() => { return TaskGetData(data.ToList(), 1, (int)rorTwo); }));
            //}
            //else
            //{
            //    var valueRow = Math.Floor(rorTwo);
            //    var numberForThreeTask = numberRow - (valueRow * 2);

            //    taskList.Add(new Task<List<ArticleDTO>>(() =>{return TaskGetData(data.ToList(), 0, (int)rorTwo);}));
            //    taskList.Add(new Task<List<ArticleDTO>>(() => { return TaskGetData(data.ToList(), 1, (int)rorTwo); }));                
            //    taskList.Add(new Task<List<ArticleDTO>>(() => { return TaskGetData(data.ToList(), 2, (int)numberForThreeTask); }));
            //}


            //foreach (var task in taskList)
            //{
            //    task.Start();
            //}

            //Task.WaitAll(taskList.ToArray());

            #endregion

            List<OutputInfo> listOutput = new List<OutputInfo>();
            foreach (var product in listProducts)
            {
                var tmpInfo = new OutputInfo()
                {
                    Brand = product.Brand,
                    Article = product.Article
                };

                if (product.ListSellers != null)
                {
                    var count = product.ListSellers.Count;
                    if (count == 1)
                    {
                        tmpInfo.FirstPrice = product.ListSellers[0].Price;
                        tmpInfo.FirstCount = product.ListSellers[0].InStock;
                        tmpInfo.FirstDays = product.ListSellers[0].DeliveryDays;
                    }
                    else if (count == 2)
                    {
                        tmpInfo.FirstPrice = product.ListSellers[0].Price;
                        tmpInfo.FirstCount = product.ListSellers[0].InStock;
                        tmpInfo.FirstDays = product.ListSellers[0].DeliveryDays;

                        tmpInfo.SecondPrice = product.ListSellers[1].Price;
                        tmpInfo.SecondCount = product.ListSellers[1].InStock;
                        tmpInfo.SecondDays = product.ListSellers[1].DeliveryDays;
                    }
                    else if (count == 3)
                    {
                        tmpInfo.FirstPrice = product.ListSellers[0].Price;
                        tmpInfo.FirstCount = product.ListSellers[0].InStock;
                        tmpInfo.FirstDays = product.ListSellers[0].DeliveryDays;

                        tmpInfo.SecondPrice = product.ListSellers[1].Price;
                        tmpInfo.SecondCount = product.ListSellers[1].InStock;
                        tmpInfo.SecondDays = product.ListSellers[1].DeliveryDays;

                        tmpInfo.ThirdPrice = product.ListSellers[2].Price;
                        tmpInfo.ThirdCount = product.ListSellers[2].InStock;
                        tmpInfo.ThirdDays = product.ListSellers[2].DeliveryDays;
                    }
                }
                if (product.Yapart != null)
                {
                    if (product.Yapart.Count >= 1)
                    {
                        tmpInfo.YourPrice = product.Yapart[0].Price;
                        tmpInfo.YourCount = product.Yapart[0].InStock;
                        tmpInfo.YourDays = product.Yapart[0].DeliveryDays;
                    }
                }
                listOutput.Add(tmpInfo);
            }

            var outputRepository = new AccessOutputRepository();
            outputRepository.AddListProducts(listOutput);

            _service.BrowserQuit();
        }
    }
}
