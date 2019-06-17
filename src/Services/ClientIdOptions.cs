using System.ComponentModel.DataAnnotations;

namespace Firepuma.Api.Common.Services
{
    public class ClientIdOptions
    {
        [Required]
        public string ClaimKey { get; set; } = "azp"; // default
    }
}