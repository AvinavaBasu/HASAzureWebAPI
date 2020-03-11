using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using ProtoBuf.Meta;
using Raet.UM.HAS.DTOs;

namespace Raet.UM.HAS.Client.InitialLoad
{
    public static class ModelBuilder
    {
        private static RuntimeTypeModel _model;

        public static RuntimeTypeModel GetModel()
        {
            if (_model == null)
            {
                _model = RuntimeTypeModel.Default;
                _model.Add(typeof(ExternalId), true).Add("Context", "Id");
                _model.Add(typeof(Permission), true).Add("Id", "Application", "Description");
                _model.Add(typeof(EffectiveAuthorization), true).Add("TenantId", "User", "Permission", "Target");
                _model.Add(typeof(EffectiveAuthorizationGrantedEvent), true).Add("FromDateTime", "EffectiveAuthorization");
            }

            return _model;
        }

    }
}
