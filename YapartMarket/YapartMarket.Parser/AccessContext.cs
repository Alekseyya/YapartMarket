using System;
using System.IO;
using Newtonsoft.Json;
using YapartMarket.Data;

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
                    _connectionString = JsonConvert.DeserializeObject<AppSettings>(json).ConnectionAccess;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
