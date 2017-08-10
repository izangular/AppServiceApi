using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppServiceApi.Models
{
    public class OfferedRentApiInput
    {
        [Required]
        [SwaggerDefaultValue(" Base64 Image text ")]
        public string imageBase64 { get; set; }

        [Required]
        [SwaggerDefaultValue("47.4091209")]
        public double? lat { get; set; }

        [Required]
        [SwaggerDefaultValue("8.5467016")]
        public double? lng { get; set; }

        [Required]
        [SwaggerDefaultValue("123XD&E")]
        public string deviceId { get; set; }

    }

    public class OfferedRentInput
    {
        public OfferedRentInput()
        {
            ortId = 35;
            externalKey = "ORCH_Residential_v2";
            categoryCode = 6;
            qualityMicro = 2;
            surfaceContract = 800;
            buildYear = 1980;
            roomNb = 3.5;
            //bathNb = 1;
            address = new OfferedRentAddress()
            {
                address = "Tramstrasse 10, 8050 Zürich, Switzerland",
                street = "Tramstrasse 10",
                zip = "8050",
                town = "Zürich",
                lat = 47.409195,
                lng = 8.547145
            };
        }

        [Required]
        [SwaggerDefaultValue("35")]
        public int ortId { get; set; }

        [Required]
        [SwaggerDefaultValue("35")]
        public string externalKey { get; set; }

        [Required]
        [SwaggerDefaultValue("6")]
        public int categoryCode { get; set; }

        [Required]
        [SwaggerDefaultValue("15")]
        public int objectTypeCode { get; set; }

        [Required]
        [SwaggerDefaultValue("3")]
        public double? qualityMicro { get; set; }

        [Required]
        [SwaggerDefaultValue("800")]
        public int surfaceContract { get; set; }

        [Required]
        [SwaggerDefaultValue("1990")]
        public int buildYear { get; set; }

        [Required]
        [SwaggerDefaultValue("3.5")]
        public double roomNb { get; set; }

        [Required]
        [SwaggerDefaultValue("0")]
        public int lift { get; set; }

        [Required]
        [SwaggerDefaultValue("123XD&E")]
        public string deviceId { get; set; }

        public OfferedRentAddress address { get; set; }
    }

    public class OfferedRentAddress
    {
        [Required]
        [SwaggerDefaultValue("Tramstrasse 10, 8050 Zürich, Switzerland")]
        public string address { get; set; }

        [Required]
        [SwaggerDefaultValue("Tramstrasse 10")]
        public string street { get; set; }

        [Required]
        [SwaggerDefaultValue("8050")]
        public string zip { get; set; }

        [Required]
        [SwaggerDefaultValue("Zürich")]
        public string town { get; set; }

        [Required]
        [SwaggerDefaultValue("Switzerland")]
        public string country { get; set; }

        [Required]
        [SwaggerDefaultValue("47.409195")]
        public double lat { get; set; }

        [Required]
        [SwaggerDefaultValue("8.547145")]
        public double lng { get; set; }
    }

    public class OfferedRentOutput
    {
        public int ortId { get; set; }
        public string zip { get; set; }
        public string town { get; set; }
        public string street { get; set; }
        public string country { get; set; }
        public string category { get; set; }
        public string objectType { get; set; }
        public long appraisalValue { get; set; }
        public double qualityMicro { get; set; }

        public int? surfaceContract { get; set; }
        //public int? landSurface { get; set; }
        public double? roomNb { get; set; }
        //public int? bathNb { get; set; }
        public int? buildYear { get; set; }
        public int? lift { get; set; }

        private int categoryCode;
        private int objectTypeCode;

        public int CategoryCode
        {
            get { return categoryCode; }
            set
            {
                categoryCode = value;
                switch (categoryCode)
                {
                    case 5:
                        category = "Single family House";
                        break;
                    case 6:
                        category = "Condominium";
                        break;
                    default:
                        break;
                }

            }
        }

        public int ObjectTypeCode
        {
            get { return objectTypeCode; }
            set
            {
                objectTypeCode = value;
                switch (objectTypeCode)
                {
                    case 2:
                        objectType = "Penthouse";
                        break;
                    case 5:
                        objectType = "Attic";
                        break;
                    case 13:
                        objectType = "Duplex";
                        break;
                    case 15:
                        objectType = "Furnished Apartment";
                        break;
                    case 16:
                        objectType = "Studio";
                        break;
                    case 21:
                        objectType = "Normal Flat";
                        break;
                    case 100:
                        objectType = "Loft";
                        break;
                    case 1099:
                        objectType = "Terrace Apartment";
                        break;
                    case 1199:
                        objectType = "House";
                        break;
                    default:
                        break;
                }
            }
        }
    }
}