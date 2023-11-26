using Quartz;
using System;
using System.Threading.Tasks;
using YapartMarket.BL.Implementation;
using YapartMarket.Core.DTO;

namespace YapartMarket.WebApi.Job
{
    public sealed class CreateYMLFileJob : IJob
    {
        readonly YmlServiceBase ymlServiceBase;
        public CreateYMLFileJob(YmlServiceBase ymlServiceBase)
        {
            this.ymlServiceBase = ymlServiceBase ?? throw new ArgumentNullException(nameof(ymlServiceBase));
        }
        async Task IJob.Execute(IJobExecutionContext context)
        {
            await ymlServiceBase.ProcessCreateFileAsync(context.CancellationToken);
        }
    }
}
