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

        /// <summary>
        /// Токен доступа
        /// </summary>
        public string AccessToken { get; set; }
        /// <summary>
        /// Время истечения срока действия токена
        /// </summary>
        public int ExpireToken { get; set; }

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
    }
}