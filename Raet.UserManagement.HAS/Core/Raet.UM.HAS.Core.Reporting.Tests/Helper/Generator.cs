using Raet.UM.HAS.Core.Domain;
using System;
using System.Collections.Generic;

namespace Raet.UM.HAS.Core.Reporting.Tests.Helper
{
    public static class Generator
    {
        private static readonly Random random = new Random();

        public static IList<EffectiveAuthorizationInterval> GenerateEffectiveAuthorizationIntervals()
        {
            IList<EffectiveAuthorizationInterval> effectiveAuthorizationIntervalsList = new List<EffectiveAuthorizationInterval>()
            {
                new EffectiveAuthorizationInterval(new Interval(DateTime.Now, DateTime.Now),new Person(new ExternalId(), new PersonalInfo()),new Person(new ExternalId(), new PersonalInfo()),new Permission(), ""   )
                {
                    Permission = {Application = "App", Description = "Log Permission", Id = "P007"},
                    TargetPerson =
                    {
                        Key = {Id = "R007", Context = "Youforce.Users"},
                        PersonalInfo =
                            {BirthDate = DateTime.Now, Initials = "I", LastNameAtBirth = "LastName", LastNameAtBirthPrefix = "Last"}
                    },
                    User =
                    {
                        Key = {Id = "R005", Context = "Youforce.Users"},
                        PersonalInfo =
                            {BirthDate = DateTime.Now, Initials = "I", LastNameAtBirth = "LastName", LastNameAtBirthPrefix = "Last"}
                    }

                }
            };

            return effectiveAuthorizationIntervalsList;
        }

        public static IEnumerable<Person> GeneratePersonsData(IEnumerable<ExternalId> ids)
        {
            var result = new List<Person>();
            foreach (var externalId in ids)
            {
                result.Add(new Person(externalId, new PersonalInfo()
                {
                    BirthDate = DateTime.Now,
                    Initials = "I",
                    LastNameAtBirth = "SIHNF",
                    LastNameAtBirthPrefix = "GSXUA"
                }));
            }

            return result;
        }

        public static List<EffectiveAuthorizationEvent> GenerateReadRawEventData(int grantedCount, int revokeCount)
        {
            var res = new List<EffectiveAuthorizationEvent>();
            var currentDate = DateTime.Now;

            for (int i = grantedCount; i > 0; i--)
            {
                res.Add(GetEffectiveAuthorizationGrantedEvent(currentDate.StartOfMonth(i)));
            }

            for (int i = revokeCount; i > 0; i--)
            {
                grantedCount = grantedCount == 0 ? revokeCount : grantedCount;
                res.Add(GetEffectiveAuthorizationRevokedEvent(currentDate.EndOfMonth(grantedCount--)));
            }

            return res;
        }

        private static EffectiveAuthorizationGrantedEvent GetEffectiveAuthorizationGrantedEvent(DateTime eventDate)
        {
            return new EffectiveAuthorizationGrantedEvent()
            {
                From = eventDate,
                EffectiveAuthorization = GetEffectiveAuthorization()
            };
        }

        private static EffectiveAuthorizationRevokedEvent GetEffectiveAuthorizationRevokedEvent(DateTime eventDate)
        {
            return new EffectiveAuthorizationRevokedEvent()
            {
                Until = eventDate,
                EffectiveAuthorization = GetEffectiveAuthorization()
            };
        }

        private static EffectiveAuthorization GetEffectiveAuthorization()
        {
            return new EffectiveAuthorization()
            {
                Target = new ExternalId()
                {
                    Context = "Youforce.Users",
                    Id = "R007"
                },
                User = new ExternalId()
                {
                    Context = "Youforce.Users",
                    Id = "R005",
                },
                Permission = new Permission()
                {
                    Id = "P007",
                    Application = "App",
                    Description = "new log permission"
                },
                TenantId = "TheBestPosibleTenant"
            };
        }

        private static DateTime StartOfMonth(this DateTime current, int index)
        {
            int earlierMonth = DateTime.Now.AddMonths(-(index)).Month;
            return new DateTime(DateTime.Now.Year, earlierMonth, 2);
        }

        private static DateTime EndOfMonth(this DateTime current, int index)
        {
            int earlierMonth = DateTime.Now.AddMonths(-(index)).Month;
            return new DateTime(DateTime.Now.Year, earlierMonth, 2).AddMonths(1).AddDays(-1);
        }
    }
}
