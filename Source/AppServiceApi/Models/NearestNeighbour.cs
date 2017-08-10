using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppServiceApi.Models
{
    public class RentFinancials
    {     
        [Required]
        [SwaggerDefaultValue("47.4122963")]
        public double latitude { get; set; }

        [Required]
        [SwaggerDefaultValue("8.55722609999998")]
        public double longitude { get; set; }

        [Required]
        [SwaggerDefaultValue("2016")]
        public int year { get; set; }

        [SwaggerDefaultValue("en-US")]
        public string culture { get; set; }

        [SwaggerDefaultValue("propertyType%253D0")]
        public string filter { get; set; }

        [SwaggerDefaultValue("5")]
        public int nbComparableProperties { get; set; }       
    }

    public class RentContracts
    {
        [Required]
        [SwaggerDefaultValue("47.4122963")]
        public double latitude { get; set; }

        [Required]
        [SwaggerDefaultValue("8.55722609999998")]
        public double longitude { get; set; }

        [SwaggerDefaultValue("en-US")]
        public string culture { get; set; }

        [SwaggerDefaultValue("propertyType%253D0")]
        public string filter { get; set; }

        [SwaggerDefaultValue("5")]
        public int nbComparableProperties { get; set; }
    }
}