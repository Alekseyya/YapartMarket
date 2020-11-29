namespace YapartMarket.Core.AccessModels
{
    public class AccessProduct
    {
        /// <summary>
        /// Ассортимент код
        /// </summary>
        public int AS_ID { get; set; }
        /// <summary>
        /// yapart внутренний код
        /// </summary>
        public string Tovar { get; set; }
        public string Sviat_Code { get; set; }
        /// <summary>
        /// Артикул
        /// </summary>
        public string Nomenkl_nomer { get; set; }
        /// <summary>
        /// Производитель или основной поставщик
        /// </summary>
        public int Proizvoditel { get; set; }
        public string Marka { get; set; }
        public string Public_Cross { get; set; }
        public int Tip { get; set; }
        public AccessProductType AccessProductType { get; set; }
        public string Opisanie { get; set; }
        public int Min_zapas { get; set; }
        public int Max_zapas { get; set; }
        public string MarkaP { get; set; }
        /// <summary>
        /// статус товара для сайта  1 - новинка;  2 - популярное
        /// </summary>
        public int ItemStatus { get; set; }
        /// <summary>
        /// поле поиска
        /// </summary>
        public string SearchField { get; set; }
    }
}
