namespace Raet.UM.HAS.Core.Domain
{
    public class Person
    {
        public Person(ExternalId key, PersonalInfo personalInfo)
        {
            Key = key;
            PersonalInfo = personalInfo;
        }

        public ExternalId Key { get; set; }

        public PersonalInfo PersonalInfo { get; set; }

        public override bool Equals(object obj)
        {
            var person = obj as Person;
            return person != null && Key.Equals(person.Key) && PersonalInfo.Equals(person.PersonalInfo);
        }

        public override int GetHashCode()
        {
            return Key != null ? Key.GetHashCode() : 0;
        }
    }
}