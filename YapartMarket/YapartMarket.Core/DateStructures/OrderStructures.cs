namespace YapartMarket.Core.DateStructures
{
    public enum OrderStatus : short
    {
        Unknown = 0, //Неизвестный статус),
        PlaceOrderSuccess = 1, //Ожидает оплаты),
        PaymentPending = 2, //Обработка платежа),
        WaitExamineMoney = 3, //В ожидании подтверждения платежа),
        WaitGroup = 4, //Выполняется группова покупка),
        WaitSendGoods = 5, //Ожидается отправка),
        PartialSendGoods = 6, //Частично отправлено),
        WaitAcceptGoods = 7, //Ожидается получение),
        InCancel = 8, //Заявка покупателя на отмену),
        Complete = 9, //Выполнен),
        Close = 10, //Завершен),
        Finish = 11, //Завершен),
        InFrozen = 12, //Заморожено),
        InIssue = 13//В состоянии спора).
    }
   public enum PaymentStatus : short
    {
        Unknown = 0,
        NotPaid = 1,
        Hold = 2,
        Paid = 3,
        Cancelled = 4,
        Failed = 5
    }

    public enum LogisticsStatus
    {
        WAIT_SELLER_SEND_GOODS,
        SELLER_SEND_PART_GOODS,
        SELLER_SEND_GOODS,
        BUYER_ACCEPT_GOODS,
        NO_LOGISTICS,
        UNKNOWN
    }

    public enum BizType
    {
        AE_COMMON,
        AE_TRIAL,
        AE_RECHARGE,
        UNKNOWN
    }
}
