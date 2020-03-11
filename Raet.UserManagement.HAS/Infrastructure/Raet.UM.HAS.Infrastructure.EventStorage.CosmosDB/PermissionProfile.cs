using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Domain = Raet.UM.HAS.Core.Domain;
using Models = Raet.UM.HAS.Infrastructure.Storage.Models;

namespace Raet.UM.HAS.Infrastructure.Storage.CosmosDB
{
    public class PermissionProfile : Profile
    {
        public PermissionProfile()
        {
            CreateMap<Models.Permission, Domain.Permission>()
            .ForMember(dest => dest.Application, opts => opts.MapFrom(src => src.Application))
            .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
            .ForMember(dest => dest.Description, opts => opts.MapFrom(src => src.Description));
        }
    }
}
