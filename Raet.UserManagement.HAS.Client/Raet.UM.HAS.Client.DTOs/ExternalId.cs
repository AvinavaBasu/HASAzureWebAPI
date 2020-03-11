using System;
using System.ComponentModel.DataAnnotations;

namespace Raet.UM.HAS.Client.DTOs
{
    public class ExternalId
    {
        [Required(AllowEmptyStrings=false)]
        public string Context { get; private set; }

        [Required(AllowEmptyStrings = false)]
        public string Id { get; private set; }

        public ExternalId(string context, string id)
        {
            Context = context;
            Id = id;
        }
    }
}
