using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppServiceApi.Models
{
    public class Rating
    {
    }

    public class RatingResponse
    {
        public string status { get; set; }
        public Results results { get; set; }

        public class Results
        {
            public int? microReferenceDate;
            public int? microQuality;
            public string microQualityText;
            public double? microRatingClass1To5;
            public double? microRatingUnscaledValue;
            public string microRatingClass1To5Text;
            public double? microRatingValue;
            public string microRatingType;
            public double? lat;
            public double? lon;
            public double? distance;
        }
    }
}