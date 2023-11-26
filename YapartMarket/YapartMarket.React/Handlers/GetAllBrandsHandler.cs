using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using YapartMarket.Core.BL;
using YapartMarket.React.Queries.Brands;
using YapartMarket.React.ViewModels;

namespace YapartMarket.React.Handlers
{
    public class GetAllBrandsHandler : IRequestHandler<GetAllBrandsQuery, List<BrandViewModel>>
    {
        private readonly IBrandService _brandService;
        private readonly IMapper _mapper;

        public GetAllBrandsHandler(IBrandService brandService, IMapper mapper)
        {
            _brandService = brandService;
            _mapper = mapper;
        }
        public async Task<List<BrandViewModel>> Handle(GetAllBrandsQuery request, CancellationToken cancellationToken)
        {
            var brands = await _brandService.GetBrands();
            return _mapper.Map<List<BrandViewModel>>(brands);
        }
    }
}
