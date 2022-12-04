namespace YapartMarket.Core.DateStructures
{
    public enum OrderStatus : short
    {
        /// <summary>
        /// Ожидание оплаты покупателем
        /// </summary>
        PLACE_ORDER_SUCCESS = 0,
        /// <summary>
        /// Отмена заказа покупателем
        /// </summary>
        IN_CANCEL = 1,
        /// <summary>
        /// Ожидает отправки заказа
        /// </summary>
        WAIT_SELLER_SEND_GOODS = 2,
        /// <summary>
        /// Частичная поставка
        /// </summary>
        SELLER_PART_SEND_GOODS = 3,
        /// <summary>
        /// Ожидание получения товара покупателем
        /// </summary>
        WAIT_BUYER_ACCEPT_GOODS = 4,
        /// <summary>
        /// Покупатель одобрил, обработка средств
        /// </summary>
        FUND_PROCESSING = 5,
        /// <summary>
        /// Заказ в споре
        /// </summary>
        IN_ISSUE = 6,
        /// <summary>
        /// Заказ в замороженном состоянии
        /// </summary>
        IN_FROZEN = 7,
        /// <summary>
        /// Ожиданме подтверждение оплаты заказа
        /// </summary>
        WAIT_SELLER_EXAMINE_MONEY = 8,
        /// <summary>
        /// Заказы проходят контроль рисков в течении 24 часов
        /// </summary>
        RISK_CONTROL = 9,
        /// <summary>
        /// Выполнение заказа нужно запрашивать отдельно
        /// </summary>
        FINISH = 10,
        //Все возможные значения
        ANY = 11,
    }
    /// <summary>
    /// Статус заморозки товара
    /// </summary>
    public enum FrozenStatus
    {
        NO_FROZEN,
        IN_FROZEN
    }
    /// <summary>
    /// Статус проблемы заказа
    /// </summary>
    public enum IssueStatus
    {
        NO_ISSUE,
        IN_ISSUE,
        END_ISSUE
    }

    public enum ShipperType
    {
        /// <summary>
        /// Доставка от продавца
        /// </summary>
        SELLER_SEND_GOODS,
        /// <summary>
        /// Доставка со склада
        /// </summary>
        WAREHOUSE_SEND_GOODS
    }

    public enum FundStatus
    {
        NOT_PAY,
        PAY_SUCCESS,
        WAIT_SELLER_CHECK
    }
    
    public enum LogisticsStatus
    {
        WAIT_SELLER_SEND_GOODS,
        SELLER_SEND_PART_GOODS,
        SELLER_SEND_GOODS,
        BUYER_ACCEPT_GOODS,
        NO_LOGISTICS
    }

    public enum BizType
    {
        AE_COMMON,
        AE_TRIAL,
        AE_RECHARGE
    }
}
