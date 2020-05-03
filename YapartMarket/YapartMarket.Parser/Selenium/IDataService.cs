using System.Collections.Generic;
using YapartMarket.Parser.Data.Models;

namespace YapartMarket.Parser.Selenium
{
    public interface IDataService
    {
        void SetPage(string url);
        void SingUp();
        void SearchArticulInSearchInput(string article);
        //int DetermineNumberGoodsSold();
        bool ChooseAnalogs(string brand);
        ArticleDTO GetInfoAboutArticle();
        IList<ShortInfo> GetTopThreeTrader();
        void BrowserQuit();
    }
}
