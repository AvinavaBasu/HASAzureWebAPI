using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Domain = Raet.UM.HAS.Core.Domain;
using Models = Raet.UM.HAS.Infrastructure.Storage.Models;

namespace Raet.UM.HAS.Infrastructure.Storage.CosmosDB
{
    public class EffectiveAuthorizationProfile : Profile
    {
        public EffectiveAuthorizationProfile()
        {
            CreateMap<Models.EffectiveAuthorization, Domain.EffectiveAuthorization>()
            .ForMember(dest => dest.TenantId, opts => opts.MapFrom(src => src.TenantId))
            .AfterMap((src, dest) => Mapper.Map(src.User, dest.User))
            .AfterMap((src, dest) => Mapper.Map(src.Permission, dest.Permission))
            .AfterMap((src, dest) => Mapper.Map(src.Target, dest.Target));
        }
    }
}
