using System.ComponentModel.DataAnnotations;

namespace Firepuma.Api.Common.Configure
{
    public class MetricsInfluxConfig
    {
        [Required]
        public string Url { get; set; }

        [Required]
        public string Database { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}