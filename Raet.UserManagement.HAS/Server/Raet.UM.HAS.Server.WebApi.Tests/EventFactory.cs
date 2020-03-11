using System;
using System.Collections.Generic;
using System.Text;
using Raet.UM.HAS.DTOs;

namespace Raet.UM.HAS.Server.WebApi.Tests
{
    public class EventFactory
    {
        public static EffectiveAuthorizationEvent BuildEventByType(string type)
        {
            switch (type)
            {
                case "granted":
                    return new EffectiveAuthorizationGrantedEvent()
                    {
                        FromDateTime = DateTime.Now,
                        EffectiveAuthorization = new EffectiveAuthorization("TestTenantHAS", new ExternalId("1", "Youforce.Users"), new Permission("1", "Permission", "To do something"), new ExternalId("3", "Youforce.Users"))
                    };
                case "revoked":
                    return new EffectiveAuthorizationRevokedEvent()
                    {
                        UntilDateTime = DateTime.Now,
                        EffectiveAuthorization = new EffectiveAuthorization("TestTenantHAS", new ExternalId("1", "Youforce.Users"), new Permission("1", "Permission", "To do something"), new ExternalId("3", "Youforce.Users"))

                    };
                default: return null;
            }


        }
    }
}
