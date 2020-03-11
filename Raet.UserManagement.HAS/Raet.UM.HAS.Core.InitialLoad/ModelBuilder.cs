using ProtoBuf.Meta;

namespace Raet.UM.HAS.Core.InitialLoad
{
    public static class ModelBuilder
    {
        private static RuntimeTypeModel _model;

        public static RuntimeTypeModel GetModel()
        {
            if (_model == null)
            {
                _model = RuntimeTypeModel.Default;
                _model.Add(typeof(DTOs.ExternalId), true).Add("Context", "Id");
                _model.Add(typeof(DTOs.Permission), true).Add("Id", "Application", "Description");
                _model.Add(typeof(DTOs.EffectiveAuthorization), true).Add("TenantId", "User", "Permission", "Target");
                _model.Add(typeof(DTOs.EffectiveAuthorizationGrantedEvent), true).Add("FromDateTime", "EffectiveAuthorization");
            }
            return _model;
        }
    }
}
