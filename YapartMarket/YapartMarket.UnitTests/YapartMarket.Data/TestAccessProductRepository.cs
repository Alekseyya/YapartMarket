using System.IO;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;
using YapartMarket.Data;
using YapartMarket.Data.Implementation.Access;

namespace YapartMarket.UnitTests.YapartMarket.Data
{
    public class TestAccessProductRepository
    {
        private readonly ITestOutputHelper _output;
        private readonly AppSettings _appSettings;
        public TestAccessProductRepository(ITestOutputHelper output)
        {
            using (var r = new StreamReader("C:\\MyOwn\\YapartStore\\YapartMarket\\YapartMarket.Parser\\appsettings.json"))
            {
                var json = r.ReadToEnd();
                _appSettings = JsonConvert.DeserializeObject<AppSettings>(json);
                _output = output;
            }
        }

        [Fact]
        public void Test_GenerateInsertQuery()
        {
            var accessProductRepository = new AccessProductRepository(_appSettings);
            var generateInsertQuery = accessProductRepository.GenerateInsertQuery();
            Assert.NotEmpty(generateInsertQuery);
            _output.WriteLine(generateInsertQuery);
        }

        [Fact]
        public void Test_GenerateUpdateQuery()
        {
            var accessProductRepository = new AccessProductRepository(_appSettings);
            var generateUpdateQuery = accessProductRepository.GenerateUpdateQuery();
            Assert.NotEmpty(generateUpdateQuery);
            _output.WriteLine(generateUpdateQuery);
        }

    }
}
