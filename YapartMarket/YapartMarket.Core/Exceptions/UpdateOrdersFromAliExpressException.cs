using System;

namespace YapartMarket.Core.Exceptions
{
    [Serializable]
    public class UpdateOrdersFromAliExpressException : Exception
    {
        public UpdateOrdersFromAliExpressException()
        { }

        public UpdateOrdersFromAliExpressException(string message)
            : base(message)
        { }

        public UpdateOrdersFromAliExpressException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
