using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppServiceApi.Models
{
    public class BaseAuth
    {
        public appService appService { get; set; }
    }

    public class appService
    {
        public bool modebase { get; set; }
    }
    
}