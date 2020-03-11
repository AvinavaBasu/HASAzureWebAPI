using System;
using System.ComponentModel.DataAnnotations;

namespace Raet.UM.HAS.Client.DTOs
{
    public class Permission
    {
        [Required(AllowEmptyStrings = false)]
        public string Identifier { get; private set; }

        [Required(AllowEmptyStrings = false)]
        public string Application { get; private set; }

        public string Description { get; private set; }

        public Permission(string identifier, string application, string description = "")
        {
            Identifier = identifier;
            Application = application;
            Description = description;
        }
    }
}
