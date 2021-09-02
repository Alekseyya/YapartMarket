using System;
using System.Threading.Tasks;
using Coravel.Invocable;
using Microsoft.Extensions.Logging;

namespace YapartMarket.React.Invocables
{
   public class DoSomethingInvocable : IInvocable
    {
        private readonly ILogger<DoSomethingInvocable> _logger;

        public DoSomethingInvocable(ILogger<DoSomethingInvocable> logger)
        {
            _logger = logger;
        }
        public async Task Invoke()
        {
            _logger.LogInformation("!!!!!!!!!!!");
            Console.Write("Doing expensive calculation for 5 sec...");
            await Task.Delay(5000);
            Console.Write("Expensive calculation done.");
        }
    }
}
