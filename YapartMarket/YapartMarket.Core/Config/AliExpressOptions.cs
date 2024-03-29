﻿namespace YapartMarket.Core.Config
{
    public class AliExpressOptions
    {
        public const string AliExpress = "AliExpress";

        /// <summary>
        /// Ключ приложения
        /// </summary>
        public string AppKey { get; set; }
        /// <summary>
        /// Секретный ключ приложения
        /// </summary>
        public string AppSecret { get; set; }

        public string RefreshToken { get; set; }
        /// <summary>
        /// Токен доступа
        /// </summary>
        public string AccessToken { get; set; }
        /// <summary>
        /// Время истечения срока действия токена
        /// </summary>
        public long ExpireToken { get; set; }
        public string SP { get; set; }

        /// <summary>
        /// Ник пользователя
        /// </summary>
        public string UserNik { get; set; }
        /// <summary>
        /// Id юзера
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Код авторизации
        /// </summary>
        public string AuthorizationCode { get; set; }
        /// <summary>
        /// Ссылка, куда придет код авторизации
        /// </summary>
        public string ReturnUrl { get; set; }
        /// <summary>
        /// Пароль доступа для получения AuthorizeCode
        /// </summary>
        public string PassAliExpressCode { get; set; }

        public string HttpsEndPoint { get; set; }
        public string Url { get; set; }
        public string GetOrderList { get; set; }
        public string CreateLogisticOrder { get; set; }
        public string MarketplaceCodeTest { get; set; }
        public string UpdateStocks { get; set; }
        public string GetProducts { get; set; }
        public string AuthToken { get; set; }
        public string HttpsBatchEndPoint { get; set; }
    }
}
