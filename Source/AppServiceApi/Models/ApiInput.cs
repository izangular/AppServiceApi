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
}