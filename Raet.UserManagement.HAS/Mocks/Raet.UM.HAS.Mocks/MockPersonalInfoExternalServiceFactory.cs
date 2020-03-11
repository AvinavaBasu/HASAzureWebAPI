using Raet.UM.HAS.Core.Reporting;
using System;
using System.Collections.Generic;
using System.Text;
using Raet.UM.HAS.Core.Reporting.Interface;

namespace Raet.UM.HAS.Mocks
{
    public class MockPersonalInfoExternalServiceFactory : IPersonalInfoExternalServiceFactory
    {
        public IPersonalInfoExternalService Resolve(string context)
        {
            return new MockPersonalInfoExternalService(context);
        }
    }
}
