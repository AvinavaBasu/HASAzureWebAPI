using System.ComponentModel.DataAnnotations;

namespace Raet.UM.HAS.DTOs
{
    public class EffectiveAuthorizationEvent
    {
        private string _id;

        public string Id { get { return _id; } }

        public void SetId(string id)
        {
            _id = id;
        }

        [Required]
        public EffectiveAuthorization EffectiveAuthorization { get; set; }
    }
}
