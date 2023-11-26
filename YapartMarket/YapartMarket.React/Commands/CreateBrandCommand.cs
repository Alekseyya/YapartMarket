using MediatR;
using YapartMarket.React.ViewModels;

namespace YapartMarket.React.Commands
{
    public class CreateBrandCommand : IRequest<BrandViewModel>
    {
        public string Name { get; set; }
    }
}
