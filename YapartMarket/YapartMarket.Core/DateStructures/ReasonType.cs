namespace YapartMarket.Core.DateStructures
{
    public enum ReasonType
    {
        /// <summary>
        /// Нет товара в наличии
        /// </summary>
        OUT_OF_STOCK = 0,
        /// <summary>
        /// Некорректная цена товара
        /// </summary>
        INCORRECT_PRICE = 1,
        /// <summary>
        /// Некорректная привязка товара к Офферу
        /// </summary>
        INCORRECT_PRODUCT = 2,
        /// <summary>
        /// Некорректные характеристики Товара на СберМегаМаркет
        /// </summary>        
        INCORRECT_SPEC = 3,
        /// <summary>
        /// Два Заказа на один и тот же товар, дубль заказа
        /// </summary>
        TWICE_ORDER = 4,
        /// <summary>
        /// Недостаточно времени на отгрузку
        /// </summary>
        NOT_TIME_FOR_SHIPPING = 5,
        /// <summary>
        /// Фродовый заказ
        /// </summary>
        FRAUD_ORDER = 6
    }
}
