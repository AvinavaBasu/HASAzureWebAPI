namespace Raet.UM.HAS.Core.Domain
{
    public class Permission
    {
        public string Id { get; set; }

        public string Application { get; set; }

        public string Description { get; set; }

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
