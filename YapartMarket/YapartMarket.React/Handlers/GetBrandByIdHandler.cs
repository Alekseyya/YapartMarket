using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using YapartMarket.Core.BL;
using YapartMarket.React.Queries.Brand;
using YapartMarket.React.ViewModels;

namespace YapartMarket.React.Handlers
{
    public class GetBrandByIdHandler : IRequestHandler<GetBrandByIdQuery, BrandViewModel>
    {
        private readonly IBrandService _brandService;
        private readonly IMapper _mapper;

        public GetBrandByIdHandler(IBrandService brandService, IMapper mapper)
        {
            _brandService = brandService;
            _mapper = mapper;
        }
        public async Task<BrandViewModel> Handle(GetBrandByIdQuery request, CancellationToken cancellationToken)
        {
            var brand = await _brandService.GetBrandAsync(request.BrandId);
            return brand != null ? _mapper.Map<BrandViewModel>(brand) : null;
        }
    }
}
