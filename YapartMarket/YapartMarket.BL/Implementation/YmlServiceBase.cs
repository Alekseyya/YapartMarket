using System;
using System.Threading;
using System.Threading.Tasks;
using YapartMarket.Core.Config;

namespace YapartMarket.BL.Implementation
{
    public abstract class YmlServiceBase
    {
        public abstract Task ProcessCreateFileAsync(CancellationToken cancellationToken);
    }
}
