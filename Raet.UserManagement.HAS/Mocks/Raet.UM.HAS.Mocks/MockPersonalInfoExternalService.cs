using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Raet.UM.HAS.Core.Domain;
using Raet.UM.HAS.Core.Reporting;
using Raet.UM.HAS.Core.Reporting.Interface;

namespace Raet.UM.HAS.Mocks
{
    public class MockPersonalInfoExternalService : IPersonalInfoExternalService
    {
        private readonly string context;

        public MockPersonalInfoExternalService(string context)
        {
            this.context = context;
        }

        private readonly Dictionary<string, Dictionary<string, PersonalInfo>> mockedResolver =
            new Dictionary<string, Dictionary<string, PersonalInfo>>
            {
                {"Youforce", new Dictionary<string, PersonalInfo> {
                    { "IC000001",
                        new PersonalInfo
                        { Initials = "J", LastNameAtBirth = "Doe", BirthDate = new DateTime(2000,1,1)}
                    },
                    { "IC000002",
                    new PersonalInfo
                        { Initials = "H", LastNameAtBirth = "None", BirthDate = new DateTime(1950,2,3)}}
                    }
                },
                {"TotalScheduling", new Dictionary<string, PersonalInfo>
                {
                    {"ts_id_1", new PersonalInfo
                        { Initials = "O", LastNameAtBirth = "Smith", BirthDate = new DateTime(1970,10,27)}
                    },
                    {"ts_id_2", new PersonalInfo
                        { Initials = "P", LastNameAtBirth = "Foo", BirthDate = new DateTime(1979,12,28)}
                    }
                }}
            };

        public Task<PersonalInfo> FindPersonalInfoAsync(string id)
        {
            // TODO: check whether return null or throw exception
            Dictionary<string, PersonalInfo> contextResolver;
            if (!mockedResolver.TryGetValue(context, out contextResolver))
            {
                return Task.FromResult<PersonalInfo>(null);
            }
            PersonalInfo personalInfo;
            if (!contextResolver.TryGetValue(id, out personalInfo))
            {
                return Task.FromResult<PersonalInfo>(null);
            }
            return Task.FromResult(personalInfo);
        }
    }
}