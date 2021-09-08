namespace YapartMarket.Core.DateStructures
{
    public enum OrderStatus
    {
        /// <summary>
        /// Ожидание оплаты покупателем
        /// </summary>
        PLACE_ORDER_SUCCESS,
        /// <summary>
        /// Отмена заказа покупателем
        /// </summary>
        IN_CANCEL,
        /// <summary>
        /// В ожидании вашего груза
        /// </summary>
        WAIT_SELLER_SEND_GOODS,
        /// <summary>
        /// Частичная поставка
        /// </summary>
        SELLER_PART_SEND_GOODS,
        /// <summary>
        /// Ожидание получения товара покупателем
        /// </summary>
        WAIT_BUYER_ACCEPT_GOODS,
        /// <summary>
        /// Покупатель одобрил, обработка средств
        /// </summary>
        FUND_PROCESSING,
        /// <summary>
        /// Заказ в споре
        /// </summary>
        IN_ISSUE,
        /// <summary>
        /// Заказ в замороженном состоянии
        /// </summary>
        IN_FROZEN,
        /// <summary>
        /// Ожиданме подтверждение оплаты заказа
        /// </summary>
        WAIT_SELLER_EXAMINE_MONEY,
        /// <summary>
        /// Заказы проходят контроль рисков в течении 24 часов
        /// </summary>
        RISK_CONTROL,
        /// <summary>
        /// Выполнение заказа нужно запрашивать отдельно
        /// </summary>
        FINISH
    }
}
