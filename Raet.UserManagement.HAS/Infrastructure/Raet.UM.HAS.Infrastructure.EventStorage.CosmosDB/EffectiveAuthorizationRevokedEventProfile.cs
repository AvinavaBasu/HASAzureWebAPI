using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Domain = Raet.UM.HAS.Core.Domain;
using Models = Raet.UM.HAS.Infrastructure.Storage.Models;

namespace Raet.UM.HAS.Infrastructure.Storage.CosmosDB
{
    public class EffectiveAuthorizationRevokedEventProfile : Profile
    {
        public EffectiveAuthorizationRevokedEventProfile()
        {
            CreateMap<Models.ReadEffectiveAuthorizationEvent, Domain.EffectiveAuthorizationRevokedEvent>()
            .ForMember(dest => dest.Until, opts => opts.MapFrom(src => src.Until))
            .AfterMap((src, dest) => Mapper.Map(src.EffectiveAuthorization, dest.EffectiveAuthorization));

            CreateMap<Domain.EffectiveAuthorizationRevokedEvent, Models.WriteEffectiveAuthorizationRevokedEvent>()
            .ForMember(dest => dest.Until, opts => opts.MapFrom(src => src.Until))
            .ForMember(dest => dest.Action, opt => opt.UseValue<string>("revoked"))
            .AfterMap((src, dest) => Mapper.Map(dest.EffectiveAuthorization, src.EffectiveAuthorization));
        }

    }
}
