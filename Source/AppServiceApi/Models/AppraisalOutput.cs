using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppServiceApi.Models
{
    public class AppraisalOutput
    { 
       public string Zip    { get; set; }
       public string Town   { get; set; }
       public string Street { get; set; }
       public string Country { get; set; }
       public string Category { get; set; }
       public long AppraisalValue { get; set; }
       public double Rating { get; set; }
    }
}