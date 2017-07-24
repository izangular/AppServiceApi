using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppServiceApi.Models
{
    public class GoogleVisionApiOutput
    {
        public List<Respons> responses { get; set; }
    }

    public class LabelAnnotation
    {
        public string mid { get; set; }
        public string description { get; set; }
        public double score { get; set; }
    }

    public class Respons
    {
        public List<LabelAnnotation> labelAnnotations { get; set; }
    }
   

}