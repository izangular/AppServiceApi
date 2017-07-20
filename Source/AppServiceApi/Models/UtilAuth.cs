using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppServiceApi.Models
{
    public class BaseAuth
    {
        public UtilAuth Util { get; set; }
    }

    public class UtilAuth
    {
        public bool ModelBase { get; set; }
        public bool Bionic { get; set; }
    }
    
}