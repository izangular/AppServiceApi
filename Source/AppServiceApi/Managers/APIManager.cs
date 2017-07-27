﻿using AppServiceApi.Models;
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
                     appraisalOutput.category = " Single family House";
                     break;
                case 6: 
                     appraisalOutput.category = " Condominium";
                     break;
                default:
                     break;
            }


            //appraisalOutput.appraisalValue = CalculatePrice(priceInput, category);

            return appraisalOutput;

        }

        public AppraisalOutput processDetailInput(DetailInput detailInput)
        {
            PriceInput priceInput = MapDetailInputToPriceInput(detailInput);
            AppraisalOutput appraisalOutput = new AppraisalOutput();
            appraisalOutput.appraisalValue = CalculatePrice(priceInput, detailInput.catCode??0);
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

        private long CalculatePrice(PriceInput priceInput , int cat) 
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
            dynamic jsonPriceResult = Newtonsoft.Json.JsonConvert.DeserializeObject(result);

            if (jsonPriceResult.status.Value == "OK" && jsonPriceResult.data[0].result.status.Value == 0)
                return jsonPriceResult.data[0].result.value.Value;
            else
                return 0;

        }

        #endregion

    }
}