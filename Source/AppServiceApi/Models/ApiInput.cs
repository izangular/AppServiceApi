using AppServiceApi.Util.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppServiceApi.Models
{   
    
    public class ApiInput
    {
        [Required]
        [SwaggerDefaultValue(" Base64 Image text ")]
        public string imageBase64 { get; set; }

        [Required]
        [SwaggerDefaultValue("47.4091209")]        
        public double? latitude { get; set; }

        [Required]
        [SwaggerDefaultValue("8.5467016")]        
        public double? longitude { get; set; }

        [Required]
        [SwaggerDefaultValue("123XD&E")]
        public string deviceId { get; set; }

    }

    public class DetailInput
    {
        //[Required]
        //[Range(1, 300)]
        [SwaggerDefaultValue("250")]         
        public int?    surfaceLiving     { get; set; }
              
        //[Range(1, 1500)]
        [SwaggerDefaultValue("1000")]  
        //[RequireWhenCategoryAttribute]
        public int?    landSurface       { get; set; }

        //[Required]
        //[Range(1, 10)]
        [SwaggerDefaultValue("3.5")]  
        public double? roomNb            { get; set; }

        //[Required]
        //[Range(1, 5)]
        [SwaggerDefaultValue("1")]  
        public int?    bathNb            { get; set; }

        //[Required]       
        //[CustomDateRangeAttribute]
        [SwaggerDefaultValue("1990")]  
        public int?    buildYear         { get; set; }

        [Required]
        [Range(1, 5)]
        [SwaggerDefaultValue("2.9")]  
        public double? microRating       { get; set; }

        [Required]
        [SwaggerDefaultValue("5")]  
        public int?    catCode           { get; set; }

        [Required]
        [SwaggerDefaultValue("8050")]  
        public string zip               { get; set; }

        [Required]
        [SwaggerDefaultValue("Zürich")]
        public string town              { get; set; }

        [Required]
        [SwaggerDefaultValue("Tramstrasse 10")]
        public string street            { get; set; }

        [Required]
        [SwaggerDefaultValue("Switzerland")]
        public string country           { get; set; }

        [Required]
        [SwaggerDefaultValue("123XD&E")]
        public string deviceId { get; set; }

    }





}