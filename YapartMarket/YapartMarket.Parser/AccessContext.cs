using System;
using System.IO;
using Newtonsoft.Json;

namespace YapartMarket.Parser
{
    public class AccessContext
    {
        public readonly string _connectionString;
        

        public AccessContext()
        {
            try
            {
                using (var r = new StreamReader("C:\\YapartStore\\YapartMarket\\YapartMarket.Parser\\appsettings.json"))
                {
                    var json = r.ReadToEnd();
                    _connectionString = JsonConvert.DeserializeObject<AppSettings>(json).connectionAccess;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
    public class AppSettings
    {
        public string connectionAccess { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
