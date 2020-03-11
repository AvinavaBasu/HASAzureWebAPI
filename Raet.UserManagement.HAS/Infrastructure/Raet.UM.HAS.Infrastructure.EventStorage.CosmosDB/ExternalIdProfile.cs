using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Domain = Raet.UM.HAS.Core.Domain;
using Models = Raet.UM.HAS.Infrastructure.Storage.Models;

namespace Raet.UM.HAS.Infrastructure.Storage.CosmosDB
{
    public class ExternalIdProfile : Profile
    {
        public ExternalIdProfile()
        {
            CreateMap<Models.ExternalId, Domain.ExternalId>()
            .ForMember(dest => dest.Context, opts => opts.MapFrom(src => src.Context))
            .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id));
        }
    }
}
