using System.ComponentModel.DataAnnotations;

namespace Raet.UM.HAS.DTOs
{
    public class ExternalId
    {
        public string Context { get; set; }
        
        public string Id { get; set; }

        public ExternalId()
        {

        }
        public ExternalId(string context, string id)
        {
            Context = context;
            Id = id;
        }

        public override bool Equals(object obj)
        {
            var key = obj as ExternalId;
            return key != null && Equals(key);
        }

        protected bool Equals(ExternalId other)
        {
            return string.Equals(Context, other.Context) && string.Equals(Id, other.Id);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Context != null ? Context.GetHashCode() : 0) * 397) ^ (Id != null ? Id.GetHashCode() : 0);
            }
        }
    }
}
