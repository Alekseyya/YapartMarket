using MediatR;
using YapartMarket.React.ViewModels;

namespace YapartMarket.React.Queries.Brand
{
    public class GetBrandByIdQuery : IRequest<BrandViewModel>
    {
        public int BrandId { get; }

        public GetBrandByIdQuery(int brandId)
        {
            BrandId = brandId;
        }
    }
}
