using System;
using System.ComponentModel.DataAnnotations;

namespace Raet.UM.HAS.Core.Domain
{
    public class ExternalId
    {
        [Required(AllowEmptyStrings = false)]
        public string Context { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Id { get; set; }

        public override bool Equals(object obj)
        {
            var key = obj as ExternalId;
            return key != null && Equals(key);
        }

        protected bool Equals(ExternalId other)
        {
            return string.Equals(Context, other.Context) && string.Equals(Id, other.Id);
        }

        public static explicit operator ExternalId(DTOs.ExternalId v)
        {
            return new ExternalId
            {
                Context = v.Context ?? null,
                Id = v.Id ?? null
            };

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
