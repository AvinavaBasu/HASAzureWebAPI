using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Domain = Raet.UM.HAS.Core.Domain;
using Models = Raet.UM.HAS.Infrastructure.Storage.Models;

namespace Raet.UM.HAS.Infrastructure.Storage.CosmosDB
{
    public class EffectiveAuthorizationGrantedEventProfile : Profile
    {
        public EffectiveAuthorizationGrantedEventProfile()
        {
            CreateMap<Models.ReadEffectiveAuthorizationEvent, Domain.EffectiveAuthorizationGrantedEvent>()
                .ForMember(dest => dest.From, opts => opts.MapFrom(src => src.From))
                .AfterMap((src, dest) => Mapper.Map(src.EffectiveAuthorization, dest.EffectiveAuthorization));

            CreateMap<Domain.EffectiveAuthorizationGrantedEvent, Models.WriteEffectiveAuthorizationGrantedEvent>()
                .ForMember(dest => dest.From, opts => opts.MapFrom(src => src.From))
                .ForMember(dest => dest.Action, opt => opt.UseValue<string>("granted"))
                .AfterMap((src, dest) => Mapper.Map(dest.EffectiveAuthorization, src.EffectiveAuthorization));
        }

    }
}
