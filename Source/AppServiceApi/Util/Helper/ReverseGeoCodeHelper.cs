using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppServiceApi.Util.Helper
{
    public class ReverseGeoCodeHelper
    {
        ReverseGeoCodeResult reverseGeoCodeResult;
        public ReverseGeoCodeHelper()
        {
            reverseGeoCodeResult = new ReverseGeoCodeResult();
        }
        public ReverseGeoCodeResult processReverseGeoCode(string jsonString)
        {
            dynamic reverseGeoCode = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonString);

            if (reverseGeoCode.status == "OK")
            {
                if (reverseGeoCode.data != null && reverseGeoCode.data.results[0] != null)
                {

                    string city = "", houseNo = "";
                    double lat;
                    double lng;

                    for (int i = 0; i < reverseGeoCode.data.results[0].address_components.Count; i++)
                    {
                        dynamic addr = reverseGeoCode.data.results[0].address_components[i];
                        string addTypes = addr.types[0];
                        // check if this entry in address_components has a type of country
                        switch (addTypes)
                        {
                            case "country":
                                reverseGeoCodeResult.Country = addr.long_name;
                                break;
                            case "street_address":
                            case "establishment":
                            case "route":
                                reverseGeoCodeResult.Street = reverseGeoCodeResult.Street + addr.long_name;
                                break;
                            case "street_number":
                                houseNo = addr.long_name;
                                break;
                            case "postal_code":
                                reverseGeoCodeResult.Zip = addr.short_name;
                                break;
                            case "administrative_area_level_1":
                                reverseGeoCodeResult.Town = addr.long_name;
                                break;
                            case "locality":
                                city = addr.long_name;
                                break;
                            default:
                                break;
                        }

                    }

                    reverseGeoCodeResult.Street = reverseGeoCodeResult.Street + " " + houseNo;

                    if (reverseGeoCode.data.results[0].formatted_address != null)
                    {
                        reverseGeoCodeResult.FormattedAddress = reverseGeoCode.data.results[0].formatted_address;
                    }

                }
            }

            return reverseGeoCodeResult;
        }
    }

    public class ReverseGeoCodeResult
    {
        public ReverseGeoCodeResult()
        {
            Zip = "";
            Town = "";
            Street = "";
            Country = "";
            FormattedAddress = "";
        }

        public string Zip { get; set; }
        public string Town { get; set; }
        public string Street { get; set; }
        public string Country { get; set; }
        public string FormattedAddress { get; set; }
    }
}