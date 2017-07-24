using AppServiceApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;


namespace AppServiceApi.Util.Helper
{
    public class APIManager 
    {
        String token = "";
        public String Response;
        public RatingResponse ratingResponse;
        public ReverseGeoCodeResult reverseGeoCodeResult;
        public IAZIClientSync iaziClientsync ;
        JObject resultResponse = null;        
         

        public APIManager()
        {
            token = AuthTokenHelper.GetAuthToken();
            iaziClientsync = new IAZIClientSync();
        }

        public AppraisalOutput processImageLatLon(string imageBase64 , double latitude , double longitude)
        {
            AppraisalOutput appraisalOutput = new AppraisalOutput();
            GoogleVisionApi googleVisionApi = new GoogleVisionApi();
            PriceInput priceInput = new PriceInput();

            int category = googleVisionApi.fetchCategoryForImage(imageBase64);
            getMicroRating(category, latitude, longitude);
            getAddressForLatLong(latitude, longitude);

            priceInput.qualityMicro = appraisalOutput.Rating = ratingResponse.results.microRatingClass1To5 ?? 3;
            priceInput.zip = appraisalOutput.Zip = reverseGeoCodeResult.Zip;
            priceInput.town = appraisalOutput.Town = reverseGeoCodeResult.Town;
            priceInput.street = appraisalOutput.Street = reverseGeoCodeResult.Street;
            appraisalOutput.Country = reverseGeoCodeResult.Country;
            
            switch(category)
            {
                case 5 : 
                     appraisalOutput.Category = " Single family House";
                     break;
                case 6: 
                     appraisalOutput.Category = " Condominium";
                     break;
                default:
                     break;
            }
          

            appraisalOutput.AppraisalValue = CalculatePrice(priceInput);

            return appraisalOutput;

        }


        #region Private 

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

        private long CalculatePrice(PriceInput priceInput) 
        {            
            string catText = "a3";
            int cat = 5;
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
            dynamic jsonPriceResult = Newtonsoft.Json.JsonConvert.DeserializeObject(result);

            if (jsonPriceResult.status.Value == "OK" && jsonPriceResult.data[0].result.status.Value == 0)
                return jsonPriceResult.data[0].result.value.Value;
            else
                return 0;

        }

        #endregion

    }
}