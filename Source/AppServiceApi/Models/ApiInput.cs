using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppServiceApi.Models
{
    public class ApiInput
    {
        public string imageBase64 { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }

    }

    public class DetailInput
    {       
        public int    surfaceLiving     { get; set; }
        public int    landSurface       { get; set; }
        public double roomNb            { get; set; }
        public int    bathNb            { get; set; }
        public int    buildYear         { get; set; }
        public double microRating       { get; set; }
        public int    catCode           { get; set; }
        public string zip               { get; set; }
        public string town              { get; set; }
        public string street            { get; set; }
        public string country           { get; set; }  

    }





}