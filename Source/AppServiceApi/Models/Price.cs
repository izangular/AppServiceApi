using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppServiceApi.Models
{
    public class PriceInput
    {
        public PriceInput()
        {
            zip = "8447";
            town = "Dachsen";
            street = "Dorfstrasse 13";
            externalKey = "CH_EFH";
            surfaceLiving = 800;
            buildYear = 1980;
            roomNb = 3.5;
            bathNb = 1;
            qualityMicro = 3;
        }
        
        public string zip { get; set; }
        public string town { get; set; }
        public string street { get; set; }
        public string externalKey { get; set; }
        public int surfaceLiving { get; set; }
        public int buildYear { get; set; }
        public double roomNb { get; set; }
        public int bathNb { get; set; }
        public double qualityMicro { get; set; }

    }
    public class PriceOutput
    {

    }
   
}