using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using YapartMarket.React.ViewModels;

namespace YapartMarket.React.Queries.Brands
{
    public class GetAllBrandsQuery: IRequest<List<BrandViewModel>>
    {
    }
}
