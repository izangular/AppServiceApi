using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppServiceApi.Models
{
    public class RatingInput
    {
        [Required]
        [SwaggerDefaultValue("5")]
        public double category { get; set; }

        [Required]
        [SwaggerDefaultValue("47.4091209")]
        public double latitude { get; set; }

        [Required]
        [SwaggerDefaultValue("8.5467016")]
        public double longitude { get; set; }

        [Required]
        [SwaggerDefaultValue("123XD&E")]
        public string deviceId { get; set; }
    }
}