using AppServiceApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;


namespace AppServiceApi.Util.Helper
{
    public class APIManager 
    {
        public String token = "";
        public String Response;
        public RatingResponse ratingResponse;
        public ReverseGeoCodeResult reverseGeoCodeResult;
        public IAZIClientSync iaziClientsync ;
        JObject resultResponse = null;        
         

        public APIManager(string srctoken)
        {
            if (srctoken == null || srctoken.Length <= 0)
                token = AuthTokenHelper.GetAuthToken();
            else
                token = srctoken;

            iaziClientsync = new IAZIClientSync();
        }

        public AppraisalOutput processImageLatLon(string imageBase64 , double? latitude , double? longitude)
        {
            AppraisalOutput appraisalOutput = new AppraisalOutput();
            GoogleVisionApi googleVisionApi = new GoogleVisionApi();
            PriceInput priceInput = new PriceInput();
            int category;
            string country;
            string countryCode;
            double? lat;
            double? lng;

            try
            {
                category = googleVisionApi.fetchCategoryForImage(imageBase64);
            }
            catch(Exception)
            {
                imageBase64 = getImageAndConvertbase64();
                category = googleVisionApi.fetchCategoryForImage(imageBase64);
            }
           
            getAddressForLatLong(latitude??0.0, longitude??0.0);

            if (reverseGeoCodeResult.Country != "Switzerland")
            {
                country = "Switzerland";
                countryCode = "CH";

                priceInput.zip = appraisalOutput.zip = ConfigurationManager.AppSettings["DefaultZip"];
                priceInput.town = appraisalOutput.town = ConfigurationManager.AppSettings["DefaultTown"];
                priceInput.street = appraisalOutput.street = ConfigurationManager.AppSettings["DefaultStreet"];
                lat = Convert.ToDouble(ConfigurationManager.AppSettings["DefaultLatitude"]);
                lng = Convert.ToDouble(ConfigurationManager.AppSettings["DefaultLongitude"]);
            }
            else
            {
                country = reverseGeoCodeResult.Country;
                countryCode = "CH";

                priceInput.zip = appraisalOutput.zip = reverseGeoCodeResult.Zip;
                priceInput.town = appraisalOutput.town = reverseGeoCodeResult.Town;
                priceInput.street = appraisalOutput.street = reverseGeoCodeResult.Street;
                lat = (double)latitude;
                lng = (double)longitude;             
            }

            //country = (reverseGeoCodeResult.Country != "Switzerland") ? "Switzerland" : reverseGeoCodeResult.Country;
            //countryCode = (country != "Switzerland") ? "CH" : "CH";

            getMicroRating(category, lat ?? 0.0, lng ?? 0.0,countryCode);

            priceInput.qualityMicro = appraisalOutput.microRating = ratingResponse.results.microRatingClass1To5 ?? 3;
            //priceInput.zip = appraisalOutput.zip = reverseGeoCodeResult.Zip;
            //priceInput.town = appraisalOutput.town = reverseGeoCodeResult.Town;
            //priceInput.street = appraisalOutput.street = reverseGeoCodeResult.Street;
            //appraisalOutput.country = reverseGeoCodeResult.Country;
            appraisalOutput.country = country;
            appraisalOutput.CatCode = category;
            
            switch(category)
            {
                case 5 :
                     priceInput.surfaceLiving = Convert.ToInt16(ConfigurationManager.AppSettings["A2SurfaceLivingDefault"]) ;  //set default for A2 for Surface 
                     appraisalOutput.category = " Single family House";
                     break;
                case 6:
                     priceInput.surfaceLiving = Convert.ToInt16(ConfigurationManager.AppSettings["A3SurfaceLivingDefault"]) ;;
                     appraisalOutput.category = " Condominium";
                     break;
                default:
                     break;
            }


            CalculatePrice(priceInput, category, appraisalOutput);

            return appraisalOutput;

        }

        public OfferedRentOutput processImageLatLonForOfferedRent(string imageBase64 , double? latitude , double? longitude)
        {
            OfferedRentOutput offeredRentOutput = new OfferedRentOutput();
            GoogleVisionApi googleVisionApi = new GoogleVisionApi();
            OfferedRentInput offeredRentInput = new OfferedRentInput();
            int category;
            string country;
            string countryCode;

            try
            {
                category = googleVisionApi.fetchCategoryForImage(imageBase64);
            }
            catch (Exception)
            {
                imageBase64 = getImageAndConvertbase64();
                category = googleVisionApi.fetchCategoryForImage(imageBase64);
            }
          
            getAddressForLatLong(latitude ?? 0.0, longitude ?? 0.0);
           
            if (reverseGeoCodeResult.Country != "Switzerland")
            {
                country = "Switzerland";
                countryCode = "CH";

                offeredRentInput.address = new OfferedRentAddress()
                {
                    address = ConfigurationManager.AppSettings["DefaultFormatedAddress"],
                    zip = offeredRentOutput.zip = ConfigurationManager.AppSettings["DefaultZip"],
                    town = offeredRentOutput.town = ConfigurationManager.AppSettings["DefaultTown"],
                    street = offeredRentOutput.street = ConfigurationManager.AppSettings["DefaultStreet"],
                    lat = Convert.ToDouble(ConfigurationManager.AppSettings["DefaultLatitude"]),
                    lng = Convert.ToDouble(ConfigurationManager.AppSettings["DefaultLongitude"]),
                    country = country
                };
            }
            else
            {
                country = reverseGeoCodeResult.Country;
                countryCode = "CH";

                offeredRentInput.address = new OfferedRentAddress()
                {
                    address = reverseGeoCodeResult.FormattedAddress,
                    zip = offeredRentOutput.zip = reverseGeoCodeResult.Zip,
                    town = offeredRentOutput.town = reverseGeoCodeResult.Town,
                    street = offeredRentOutput.street = reverseGeoCodeResult.Street,
                    lat = (double)latitude,
                    lng = (double)longitude,
                    country = country
                };
            }

            getMicroRating(category, offeredRentInput.address.lat ?? 0.0, offeredRentInput.address.lng ?? 0.0, countryCode);
            offeredRentInput.qualityMicro = offeredRentOutput.qualityMicro = ratingResponse.results.microRatingClass1To5 ?? 3;

            offeredRentInput.ortId = getOrtId(countryCode, offeredRentInput.address.lat ?? 0.0, offeredRentInput.address.lng ?? 0.0, "en-US");

            offeredRentOutput.country = country;
            offeredRentOutput.CategoryCode = category;

            switch (category)
            {
                case 5:
                    offeredRentInput.surfaceContract = Convert.ToInt16(ConfigurationManager.AppSettings["A2SurfaceLivingDefault"]);  //set default for A2 for Surface 
                    offeredRentInput.categoryCode = offeredRentOutput.CategoryCode;
                    offeredRentOutput.category = " Single family House";
                    break;
                case 6:
                    offeredRentInput.surfaceContract = Convert.ToInt16(ConfigurationManager.AppSettings["A3SurfaceLivingDefault"]);
                    offeredRentInput.categoryCode = offeredRentOutput.CategoryCode;
                    offeredRentOutput.category = " Condominium";
                    break;
                default:
                    break;
            }


            CalculateRent(offeredRentInput, offeredRentOutput);

            return offeredRentOutput;
        }


        public AppraisalOutput processDetailInput(DetailInput detailInput)
        {
            PriceInput priceInput = MapDetailInputToPriceInput(detailInput);
            AppraisalOutput appraisalOutput = new AppraisalOutput();
            CalculatePrice(priceInput, detailInput.catCode??0 , appraisalOutput);
            appraisalOutput.microRating = detailInput.microRating??0;
            appraisalOutput.zip = detailInput.zip;
            appraisalOutput.town = detailInput.town;
            appraisalOutput.street = detailInput.street;
            appraisalOutput.CatCode = detailInput.catCode??0;
            appraisalOutput.country = detailInput.country;

            return appraisalOutput;
        }

        public OfferedRentOutput processOfferedRentInput(OfferedRentInput offeredRentInput)
        {
            OfferedRentOutput offeredRentOutput = new OfferedRentOutput();
            CalculateRent(offeredRentInput, offeredRentOutput);
            offeredRentOutput.qualityMicro = offeredRentInput.qualityMicro ?? 0;
            offeredRentOutput.zip = offeredRentInput.address.zip;
            offeredRentOutput.town = offeredRentInput.address.town;
            offeredRentOutput.street = offeredRentInput.address.street;
            offeredRentOutput.CategoryCode = offeredRentInput.categoryCode;
            offeredRentOutput.country = offeredRentInput.address.country;

            return offeredRentOutput;
        }

        public object calculateRentFinancial(RentFinancials rentFinancial)
        {
            string url = String.Format("{0}/{1}?culture={2}&filter={3}&latitude={4}&longitude={5}&nbComparableProperties={6}&year={7}", 
                                        ConfigurationManager.AppSettings["Server"], ConfigurationManager.AppSettings["NearestNeighbourRentFinancial"],
                                        rentFinancial.culture, rentFinancial.filter, rentFinancial.latitude, rentFinancial.longitude, 
                                        rentFinancial.nbComparableProperties, rentFinancial.year);
            string result = iaziClientsync.getApiResponse(url, token);

            return JObject.Parse(result);
        }

        public object calculateRentContracts(RentContracts rentContracts)
        {
            string url = String.Format("{0}/{1}?culture={2}&filter={3}&latitude={4}&longitude={5}&nbComparableProperties={6}",
                                        ConfigurationManager.AppSettings["Server"], ConfigurationManager.AppSettings["NearestNeighbourRentContract"],
                                        rentContracts.culture, rentContracts.filter, rentContracts.latitude, rentContracts.longitude,
                                        rentContracts.nbComparableProperties);
            string result = iaziClientsync.getApiResponse(url, token);

            return JObject.Parse(result);
        }

        #region Private 

        private string getImageAndConvertbase64()
        {
            //string path = @"D:\Workspaces\AppServiceApi\AppServiceApi\Source\AppServiceApi\Resources\Images\Commercial1.jpg";
            //string path = @"D:\Workspaces\AppServiceApi\AppServiceApi\Source\AppServiceApi\Resources\Images\Comercial 2.jpg";
            //string path = @"D:\Workspaces\AppServiceApi\AppServiceApi\Source\AppServiceApi\Resources\Images\imagereader4.jpg";

            string path = ConfigurationManager.AppSettings["Testimages"];
            using (System.Drawing.Image image = System.Drawing.Image.FromFile(path))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();

                    // Convert byte[] to Base64 String
                    string base64String = Convert.ToBase64String(imageBytes);
                   // System.IO.File.WriteAllText(@"D:\Workspaces\AppServiceApi\ImageBase64Sample.txt", base64String);
                    return base64String;
                }
            }
        }


        private PriceInput MapDetailInputToPriceInput(DetailInput detailInput)
        {
            PriceInput priceInput = new PriceInput();              
            priceInput.surfaceLiving =   detailInput.surfaceLiving??0  ;
            priceInput.surfaceGround =   detailInput.landSurface;   
            priceInput.roomNb        =   detailInput.roomNb??0;         
            priceInput.bathNb        =   detailInput.bathNb??0;         
            priceInput.buildYear     =   detailInput.buildYear??0 ; 
            priceInput.qualityMicro  =   detailInput.microRating??0; 
            priceInput.zip           =   detailInput.zip;            
            priceInput.town          =   detailInput.town;           
            priceInput.street        =   detailInput.street;

            return priceInput;
        }

        private void getMicroRating(double cat, double lat, double lon, string country)
        {
            string url = String.Format("{0}/{1}?cat={2}&countryCode={3}&lat={4}&lon={5}", ConfigurationManager.AppSettings["Server"], ConfigurationManager.AppSettings["MicroRating"], cat, country, lat, lon);
            string result = iaziClientsync.getApiResponse(url,token);
            ratingResponse = JsonConvert.DeserializeObject<RatingResponse>(result);                        
        }

        private int getOrtId(string country, double lat, double lon, string culture)
        {
            string url = String.Format("{0}/{1}?countryCode={2}&lat={3}&lon={4}&culture={5}", ConfigurationManager.AppSettings["Server"], ConfigurationManager.AppSettings["OfferedRentOrtIdService"], country, lat, lon, culture);
            string result = iaziClientsync.getApiResponse(url, token);
            return Convert.ToInt32(result);
        }

        private void getAddressForLatLong(double lat, double lon)
        {
            ReverseGeoCodeHelper reverseGeoCodeHelper = new ReverseGeoCodeHelper();

            /// Call Reverse Geo code            
            string reverseGeocodeUrl = String.Format("{0}/{1}?lat={2}&lon={3}",ConfigurationManager.AppSettings["Server"], ConfigurationManager.AppSettings["ReverseGeoCode"], lat, lon);
            string result = iaziClientsync.getApiResponse(reverseGeocodeUrl, token);
            reverseGeoCodeResult = reverseGeoCodeHelper.processReverseGeoCode(result);
        }

        private void CalculatePrice(PriceInput priceInput, int cat, AppraisalOutput appraisalOutput) 
        {            
            string catText = "a3";            
            /// Call Price Service 
            switch (cat)
            {
                case 1:
                    catText = "a1";
                    break;
                case 5:
                    catText = "a2";
                    break;
                case 6:
                    catText = "a3";
                    break;
                case 21:
                    catText = "a21";
                    break;
                default:
                    catText = "a2";
                    break;
            }

            string priceUrl = String.Format("{0}/{1}/{2}",ConfigurationManager.AppSettings["Server"], ConfigurationManager.AppSettings["PriceService"], catText);
            string postData = "[" + JsonConvert.SerializeObject(priceInput) + "]";  
            string result = iaziClientsync.postApiRequest(priceUrl, postData,token);

            //PriceOutput priceOutput = Newtonsoft.Json.JsonConvert.DeserializeObject<PriceOutput>(result);
            parsePriceModelRJson(result, appraisalOutput);

        }

        private void CalculateRent(OfferedRentInput offeredRentInput, OfferedRentOutput offeredRentOutput)
        {
            string priceUrl = String.Format("{0}/{1}", ConfigurationManager.AppSettings["Server"], ConfigurationManager.AppSettings["OfferedRentService"]);
            string postData = "[" + JsonConvert.SerializeObject(offeredRentInput) + "]";
            string result = iaziClientsync.postApiRequest(priceUrl, postData, token);

            parseOfferedRentModelRJson(result, offeredRentOutput);
        }

        private void parsePriceModelRJson(string resultJson , AppraisalOutput appraisalOutput)
        {

            dynamic jsonPriceResult = Newtonsoft.Json.JsonConvert.DeserializeObject(resultJson);

            if (jsonPriceResult.status.Value == "OK" && jsonPriceResult.data[0].result.status.Value == 0)
            {
                appraisalOutput.appraisalValue = jsonPriceResult.data[0].result.value.Value;

                Random rnd = new Random();
                double randomHi = 13 / 100.0; //rnd.Next(10, 15) / 100.0;
                double randomLow = 17 / 100.0; //rnd.Next(15, 20) / 100.0;

                appraisalOutput.minappraisalValue =Convert.ToInt64(appraisalOutput.appraisalValue - (appraisalOutput.appraisalValue * randomLow));
                appraisalOutput.maxappraisalValue =Convert.ToInt64(appraisalOutput.appraisalValue + (appraisalOutput.appraisalValue * randomHi));


                for (int i = 0; i < jsonPriceResult.data[0].parameterInfo.Count; i++)
                {
                    dynamic component = jsonPriceResult.data[0].parameterInfo[i];
                    string compName = component.name;
                    // check if this entry in address_components has a type of country
                    switch (compName)
                    {
                        case "roomNb":
                            appraisalOutput.roomNb = (component.value > 0) ? component.value : component.replacedValue;
                            break;
                        case "surfaceLiving":
                            appraisalOutput.surfaceLiving = (component.value > 0) ? component.value : component.replacedValue;
                            break;
                        case "surfaceGround":
                            appraisalOutput.landSurface = (component.value > 0) ? component.value : component.replacedValue;
                            break;
                        case "bathNb":
                            appraisalOutput.bathNb = (component.value > 0) ? component.value : component.replacedValue;
                            break;
                        case "buildYear":
                            appraisalOutput.buildYear = (component.value > 0) ? component.value : component.replacedValue;
                            break;
                        default:
                            break;
                    }

                }
            }

        }

        private void parseOfferedRentModelRJson(string resultJson, OfferedRentOutput offeredRentOutput)
        {
            dynamic jsonOfferedRentResult = Newtonsoft.Json.JsonConvert.DeserializeObject(resultJson);


            if (jsonOfferedRentResult.status.Value == "OK" && jsonOfferedRentResult.data[0].result.status.Value == 0)
            {
                offeredRentOutput.appraisalValue = (long)jsonOfferedRentResult.data[0].result.value.Value;


                for (int i = 0; i < jsonOfferedRentResult.data[0].parameterInfo.Count; i++)
                {
                    dynamic component = jsonOfferedRentResult.data[0].parameterInfo[i];
                    string compName = component.name;
                    // check if this entry in address_components has a type of country
                    switch (compName)
                    {
                        case "ortId":
                             offeredRentOutput.ortId = (component.value > 0) ? component.value : component.replacedValue;
                            break;
                        case "objectTypeCode":
                            offeredRentOutput.ObjectTypeCode = (component.value > 0) ? component.value : component.replacedValue;
                            break;
                        case "roomNb":
                            offeredRentOutput.roomNb = (component.value > 0) ? component.value : component.replacedValue;
                            break;
                        case "surfaceContract":
                            offeredRentOutput.surfaceContract = (component.value > 0) ? component.value : component.replacedValue;
                            break;
                        //case "surfaceGround":
                        //    offeredRentOutput.landSurface = (component.value > 0) ? component.value : component.replacedValue;
                        //    break;
                        //case "bathNb":
                        //    offeredRentOutput.bathNb = (component.value > 0) ? component.value : component.replacedValue;
                        //    break;
                        case "buildYear":
                            offeredRentOutput.buildYear = (component.value > 0) ? component.value : component.replacedValue;
                            break;
                        case "liftNb":
                            offeredRentOutput.lift = (component.value > 0) ? component.value : (component.replacedValue == null) ? 0 : component.replacedValue;
                            break;
                        //case "categoryCode":
                        //    offeredRentOutput.CategoryCode = (component.value > 0) ? component.value : (component.replacedValue == null) ? 0 : component.replacedValue;
                        //    break;
                        default:
                            break;
                    }

                }
            }
        }

        #endregion

    }
}