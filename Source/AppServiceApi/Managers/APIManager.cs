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

            try
            {
                category = googleVisionApi.fetchCategoryForImage(imageBase64);
            }
            catch(Exception)
            {
                imageBase64 = getImageAndConvertbase64();
                category = googleVisionApi.fetchCategoryForImage(imageBase64);
            }

            getMicroRating(category, latitude??0.0 , longitude??0.0);
            getAddressForLatLong(latitude??0.0, longitude??0.0);

            priceInput.qualityMicro = appraisalOutput.rating = ratingResponse.results.microRatingClass1To5 ?? 3;
            priceInput.zip = appraisalOutput.zip = reverseGeoCodeResult.Zip;
            priceInput.town = appraisalOutput.town = reverseGeoCodeResult.Town;
            priceInput.street = appraisalOutput.street = reverseGeoCodeResult.Street;
            appraisalOutput.country = reverseGeoCodeResult.Country;
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

        public AppraisalOutput processDetailInput(DetailInput detailInput)
        {
            PriceInput priceInput = MapDetailInputToPriceInput(detailInput);
            AppraisalOutput appraisalOutput = new AppraisalOutput();
            CalculatePrice(priceInput, detailInput.catCode??0 , appraisalOutput);
            appraisalOutput.rating = detailInput.microRating??0;
            appraisalOutput.zip = detailInput.zip;
            appraisalOutput.town = detailInput.town;
            appraisalOutput.street = detailInput.street;
            appraisalOutput.CatCode = detailInput.catCode??0;
            appraisalOutput.country = detailInput.country;

            return appraisalOutput;
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

        private void getMicroRating(double cat, double lat, double lon)
        {
            string url = String.Format("{0}/{1}?cat={2}&countryCode={3}&lat={4}&lon={5}",ConfigurationManager.AppSettings["Server"], ConfigurationManager.AppSettings["MicroRating"], cat, "CH", lat, lon);
            string result = iaziClientsync.getApiResponse(url,token);
            ratingResponse = JsonConvert.DeserializeObject<RatingResponse>(result);                        
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

        private void parsePriceModelRJson(string resultJson , AppraisalOutput appraisalOutput)
        {

            dynamic jsonPriceResult = Newtonsoft.Json.JsonConvert.DeserializeObject(resultJson);

            if (jsonPriceResult.status.Value == "OK" && jsonPriceResult.data[0].result.status.Value == 0)
            {
                appraisalOutput.appraisalValue = jsonPriceResult.data[0].result.value.Value;


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
                        case "landSurface":
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

        #endregion

    }
}