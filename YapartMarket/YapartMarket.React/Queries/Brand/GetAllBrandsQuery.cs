using System.Collections.Generic;
using MediatR;
using YapartMarket.React.ViewModels;

namespace YapartMarket.React.Queries.Brands
{
    public class GetAllBrandsQuery: IRequest<List<BrandViewModel>>
    {
    }
}
