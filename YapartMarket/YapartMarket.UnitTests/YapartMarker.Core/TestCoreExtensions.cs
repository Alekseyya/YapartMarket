using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Xunit;
using Xunit.Abstractions;
using YapartMarket.Core.AccessModels;
using YapartMarket.Core.Extensions;

namespace YapartMarket.UnitTests.YapartMarker.Core
{
    public class TestCoreExtensions
    {

        private readonly ITestOutputHelper _output;
        public TestCoreExtensions(ITestOutputHelper output)
        {
            _output = output;
        }
        [Fact]
        public void Test_Attribute()
        {
            foreach (var prop in typeof(AccessProduct).GetProperties())
            {
                if (prop.GetCustomAttribute(typeof(KeyAttribute), false) != null)
                    _output.WriteLine(prop.Name);
            }

            var result = AccessExtension.GetKeyProperty<AccessProduct>();
            Assert.NotNull(result);
            Assert.Equal("AS_ID", result);
        }

        [Fact]
        public void Test_GetProperties()
        {
            var list = AccessExtension.GetKeyProperty<AccessProduct>();
            Assert.Contains(list, "AS_ID");
        }
    }
}
