using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using YapartMarket.Core.BL;
using YapartMarket.Core.Models;
using YapartMarket.React.Commands;
using YapartMarket.React.ViewModels;

namespace YapartMarket.React.Handlers
{
    public class CreateBrandHandler :  IRequestHandler<CreateBrandCommand, BrandViewModel> 
    {
        private readonly IBrandService _brandService;
        private readonly IMapper _mapper;

        public CreateBrandHandler(IBrandService brandService, IMapper mapper)
        {
            _brandService = brandService;
            _mapper = mapper;
        }
        public async Task<BrandViewModel> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
        {
            var brand = await _brandService.CreateBrandAsync(_mapper.Map<Brand>(request));
            return _mapper.Map<BrandViewModel>(brand);
        }
    }
}
