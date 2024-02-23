using System.Threading;
using System.Threading.Tasks;

namespace YapartMarket.BL.Implementation
{
    public abstract class YmlServiceBase
    {
        public abstract Task ProcessCreateFileAsync(CancellationToken cancellationToken);
    }
}
