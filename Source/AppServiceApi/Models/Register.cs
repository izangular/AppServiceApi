using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppServiceApi.Models
{
    public class Register
    {     
        [SwaggerDefaultValue("Sachin")]
        public string firstName { get; set; }
       
        [SwaggerDefaultValue("Naik")]
        public string lastName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [SwaggerDefaultValue("xyz@gmail.com")]
        public string email { get; set; }
      
        [SwaggerDefaultValue("+919999999999")]
        public string phone { get; set; }

        [Required]
        [SwaggerDefaultValue("123XGH67")]
        public string deviceId { get; set; }
    }
}