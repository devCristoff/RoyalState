﻿using AutoMapper;
using RoyalState.Core.Application.Interfaces.Repositories;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.ViewModels.SaleTypes;
using RoyalState.Core.Domain.Entities;

namespace RoyalState.Core.Application.Services
{
    public class SaleTypeService : GenericService<SaveSaleTypeViewModel, SaleTypeViewModel, SaleType>, ISaleTypeService
    {
        private readonly ISaleTypeRepository _saleTypeRepository;
        private readonly IMapper _mapper;

        public SaleTypeService(ISaleTypeRepository saleTypeRepository, IMapper mapper) : base(saleTypeRepository, mapper)
        {
            _mapper = mapper;
            _saleTypeRepository = saleTypeRepository;
        }
    }
}
