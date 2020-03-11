using System.ComponentModel.DataAnnotations;

namespace Raet.UM.HAS.DTOs
{
    public class Permission
    {
        [Required(AllowEmptyStrings = false)]
        public string Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Application { get; set; }

        public string Description { get; set; }

        public Permission()
        {

        }

        public Permission(string id, string application, string description = "")
        {
            Id = id;
            Application = application;
            Description = description;
        }

        public override bool Equals(object obj)
        {
            var permission = obj as Permission;
            return permission != null && Equals(permission);
        }

        protected bool Equals(Permission other)
        {
            return string.Equals(Application, other.Application) && string.Equals(Id, other.Id);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Application != null ? Application.GetHashCode() : 0) * 397) ^ (Id != null ? Id.GetHashCode() : 0);
            }
        }
    }
}
