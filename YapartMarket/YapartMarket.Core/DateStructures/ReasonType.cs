namespace YapartMarket.Core.DateStructures
{
    public enum ReasonType
    {
        Empty = 0,
        /// <summary>
        /// Нет товара в наличии
        /// </summary>
        OUT_OF_STOCK = 1,
        /// <summary>
        /// Некорректная цена товара
        /// </summary>
        INCORRECT_PRICE = 2,
        /// <summary>
        /// Некорректная привязка товара к Офферу
        /// </summary>
        INCORRECT_PRODUCT = 3,
        /// <summary>
        /// Некорректные характеристики Товара на СберМегаМаркет
        /// </summary>        
        INCORRECT_SPEC = 4,
        /// <summary>
        /// Два Заказа на один и тот же товар, дубль заказа
        /// </summary>
        TWICE_ORDER = 5,
        /// <summary>
        /// Недостаточно времени на отгрузку
        /// </summary>
        NOT_TIME_FOR_SHIPPING = 6,
        /// <summary>
        /// Фродовый заказ
        /// </summary>
        FRAUD_ORDER = 7
    }
}
