using System;

namespace Raet.UM.HAS.Core.Domain
{
    public class PersonalInfo
    {
        public string Initials { get; set; }
        public string LastNameAtBirth { get; set; }
        public string LastNameAtBirthPrefix { get; set; }
        public DateTime BirthDate { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as PersonalInfo;
            return other != null
                && ((Initials != null)? Initials.Equals(other.Initials) : (other.Initials == null))
                && ((LastNameAtBirth != null)? LastNameAtBirth.Equals(other.LastNameAtBirth) : (other.LastNameAtBirth == null))
                && ((LastNameAtBirthPrefix != null)? LastNameAtBirthPrefix.Equals(other.LastNameAtBirthPrefix) : (other.LastNameAtBirthPrefix == null))
                && BirthDate.Equals(other.BirthDate);
        }

        public override int GetHashCode()
        {
           return $"{Initials??""}{LastNameAtBirth??""}{LastNameAtBirthPrefix??""}{BirthDate.ToString()??""}".GetHashCode();
        }

    }
}