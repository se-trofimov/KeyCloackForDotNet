using System.ComponentModel.DataAnnotations;

namespace WebAPI
{
    public class KeycloakConfig
    {
        [Required(ErrorMessage = "RealmUrl is required")]
        public string RealmUrl { get; set; } = null!;

        [Required(ErrorMessage = "Audience is required")]
        public string Audience { get; set; } = null!;

        [Required(ErrorMessage = "ClientSecret is required")]
        public string ClientSecret { get; set; } = null!;

        [Required(ErrorMessage = "ClientId is required")]
        public string ClientId { get; set; } = null!;
    }
}
