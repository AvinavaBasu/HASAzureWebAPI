using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Domain = Raet.UM.HAS.Core.Domain;

namespace Raet.UM.HAS.Infrastructure.Storage.CosmosDB
{
    public class EffectiveIntervalCSVMapperProfile : Profile
    {
        public EffectiveIntervalCSVMapperProfile()
        {
            CreateMap<Domain.EffectiveAuthorizationInterval, Domain.EffectiveIntervalCSVMapper>()
                .ForMember(dest => dest.EffectiveIntervalStart, opts => opts.MapFrom(src => src.EffectiveInterval.Start))
                .ForMember(dest => dest.EffectiveIntervalEnd, opts => opts.MapFrom(src => src.EffectiveInterval.End))
                .ForMember(dest => dest.EffectiveIntervalIsClosed, opts => opts.MapFrom(src => src.EffectiveInterval.IsClosed))
                .ForMember(dest => dest.UserKeyContext, opts => opts.MapFrom(src => src.User.Key.Context))
                .ForMember(dest => dest.UserKeyId, opts => opts.MapFrom(src => src.User.Key.Id))
                .ForMember(dest => dest.UserPersonalInfoInitials, opts => opts.MapFrom(src => src.User.PersonalInfo.Initials))
                .ForMember(dest => dest.UserPersonalInfoLastNameAtBirth, opts => opts.MapFrom(src => src.User.PersonalInfo.LastNameAtBirth))
                .ForMember(dest => dest.UserPersonalInfoLastNameAtBirthPrefix, opts => opts.MapFrom(src => src.User.PersonalInfo.LastNameAtBirthPrefix))
                .ForMember(dest => dest.UserPersonalInfoBirthDate, opts => opts.MapFrom(src => src.User.PersonalInfo.BirthDate))
                .ForMember(dest => dest.TargetKeyContext, opts => opts.MapFrom(src => src.TargetPerson.Key.Context))
                .ForMember(dest => dest.TargetKeyId, opts => opts.MapFrom(src => src.TargetPerson.Key.Id))
                .ForMember(dest => dest.TargetPersonalInfoInitials, opts => opts.MapFrom(src => src.TargetPerson.PersonalInfo.Initials))
                .ForMember(dest => dest.TargetPersonalInfoLastNameAtBirth, opts => opts.MapFrom(src => src.TargetPerson.PersonalInfo.LastNameAtBirth))
                .ForMember(dest => dest.TargetPersonalInfoLastNameAtBirthPrefix, opts => opts.MapFrom(src => src.TargetPerson.PersonalInfo.LastNameAtBirthPrefix))
                .ForMember(dest => dest.TargetPersonalInfoBirthDate, opts => opts.MapFrom(src => src.TargetPerson.PersonalInfo.BirthDate))
                .ForMember(dest => dest.PermissionId, opts => opts.MapFrom(src => src.Permission.Id))
                .ForMember(dest => dest.PermissionApplication, opts => opts.MapFrom(src => src.Permission.Application))
                .ForMember(dest => dest.PermissionDescription, opts => opts.MapFrom(src => src.Permission.Description))
                .ForMember(dest => dest.TenantId, opts => opts.MapFrom(src => src.TenantId));
        }
    }
}
