﻿using AppServiceApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using AppServiceApi.Core.Model;
using AppServiceApi.Core.Repository;



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

        public AppraisalOutput processImageLatLon(string imageBase64 , double? latitude , double? longitude, string deviceId)
        {
            AppraisalOutput appraisalOutput = new AppraisalOutput();
            GoogleVisionApi googleVisionApi = new GoogleVisionApi();
            PriceInput priceInput = new PriceInput();
            ImageCategory imageCategory;
            string country;
            string countryCode;
            double? lat;
            double? lng;

            try
            {
                imageCategory = googleVisionApi.fetchCategoryForImage(imageBase64);
            }
            catch(Exception)
            {
                imageBase64 = getImageAndConvertbase64();
                imageCategory = googleVisionApi.fetchCategoryForImage(imageBase64);
            }

            getAddressForLatLong(latitude??0.0, longitude??0.0);

            if (reverseGeoCodeResult.Country != "Switzerland")
            {
                country = "Switzerland";
                countryCode = "CH";

                priceInput.zip = appraisalOutput.zip = ConfigurationManager.AppSettings["DefaultZip"];
                priceInput.town = appraisalOutput.town = ConfigurationManager.AppSettings["DefaultTown"];
                //if country is not switzerland then attach time to street .
                priceInput.street = appraisalOutput.street = ConfigurationManager.AppSettings["DefaultStreet"] + DateTime.Now.Hour +  DateTime.Now.Minute + DateTime.Now.Second;

                appraisalOutput.minappraisalValue = Convert.ToInt64(latitude.ToString().Replace(".", String.Empty));
                appraisalOutput.maxappraisalValue = Convert.ToInt64(longitude.ToString().Replace(".", String.Empty));
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

            if (imageCategory.CategoryCode != -1) { 
                getMicroRating(imageCategory.CategoryCode, lat ?? 0.0, lng ?? 0.0, countryCode);
                priceInput.qualityMicro = appraisalOutput.microRating = ratingResponse.results.microRatingClass1To5 ?? 3;
            }

                     
            appraisalOutput.country = country;
            appraisalOutput.CatCode = imageCategory.CategoryCode;

            switch (imageCategory.CategoryCode)
            {
                case 5 :
                     priceInput.surfaceLiving = Convert.ToInt16(ConfigurationManager.AppSettings["A2SurfaceLivingDefault"]) ;  //set default for A2 for Surface 
                     appraisalOutput.category = imageCategory.CategoryText; //" Single family House";
                     break;
                case 6:
                     priceInput.surfaceLiving = Convert.ToInt16(ConfigurationManager.AppSettings["A3SurfaceLivingDefault"]) ;;
                     appraisalOutput.category = imageCategory.CategoryText; //" Condominium";
                     break;
                case -1:
                     appraisalOutput.category = imageCategory.CategoryText; //Set the first two labels that are returned by Google 
                     break;
                default:
                     break;
            }


             if (imageCategory.CategoryCode != -1)
                CalculatePrice(priceInput, imageCategory.CategoryCode, appraisalOutput);

            //Saving Property Details//
            try
            {
                RealEstateData realEsateData = new RealEstateData();
                realEsateData.Image = imageBase64;
                realEsateData.Latitude = (decimal)latitude;
                realEsateData.Longitude = (decimal)longitude;
                realEsateData.DeviceId = deviceId;

                using (var ms = new MemoryStream(Convert.FromBase64String(imageBase64)))
                {
                    System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                    realEsateData.ImageSize = ms.Length / 1024;
                    realEsateData.ImageWidth = img.Width;
                    realEsateData.ImageHeight = img.Height;
                }

                RealEstateAppraise realEstateAppraise = new RealEstateAppraise();
                //realEstateAppraise.RealEstateId =  new Guid();
                realEstateAppraise.AppraisalValue = appraisalOutput.appraisalValue;
                realEstateAppraise.MinAppraisalValue = appraisalOutput.minappraisalValue;
                realEstateAppraise.MaxAppraisalValue = appraisalOutput.maxappraisalValue;

                SavePricePropertyDetails(realEsateData, realEstateAppraise);

            }
            catch
            {
                return appraisalOutput;
            }
            

            return appraisalOutput;

        }

        public OfferedRentOutput processImageLatLonForOfferedRent(string imageBase64 , double? latitude , double? longitude, string deviceId)
        {
            OfferedRentOutput offeredRentOutput = new OfferedRentOutput();
            GoogleVisionApi googleVisionApi = new GoogleVisionApi();
            OfferedRentInput offeredRentInput = new OfferedRentInput();
            ImageCategory imageCategory;
            string country;
            string countryCode;

            try
            {
                imageCategory = googleVisionApi.fetchCategoryForImage(imageBase64);
            }
            catch (Exception)
            {
                imageBase64 = getImageAndConvertbase64();
                imageCategory = googleVisionApi.fetchCategoryForImage(imageBase64);
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
                    street = offeredRentOutput.street = ConfigurationManager.AppSettings["DefaultStreet"] + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second,
                    lat = Convert.ToDouble(ConfigurationManager.AppSettings["DefaultLatitude"]),
                    lng = Convert.ToDouble(ConfigurationManager.AppSettings["DefaultLongitude"]),
                    country = country
                };

                offeredRentOutput.minappraisalValue = Convert.ToInt64(latitude.ToString().Replace(".", String.Empty));
                offeredRentOutput.maxappraisalValue = Convert.ToInt64(longitude.ToString().Replace(".", String.Empty));
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

            if (imageCategory.CategoryCode != -1)
            {
                getMicroRating(imageCategory.CategoryCode, offeredRentInput.address.lat ?? 0.0, offeredRentInput.address.lng ?? 0.0, countryCode);
                offeredRentInput.qualityMicro = offeredRentOutput.qualityMicro = ratingResponse.results.microRatingClass1To5 ?? 3;
                offeredRentInput.ortId = getOrtId(countryCode, offeredRentInput.address.lat ?? 0.0, offeredRentInput.address.lng ?? 0.0, "en-US");
            }

           

            offeredRentOutput.country = country;
            offeredRentOutput.CategoryCode = imageCategory.CategoryCode;

            switch (imageCategory.CategoryCode)
            {
                case 5:
                    offeredRentInput.surfaceContract = Convert.ToInt16(ConfigurationManager.AppSettings["A2SurfaceLivingDefault"]);  //set default for A2 for Surface 
                    offeredRentInput.categoryCode = offeredRentOutput.CategoryCode;
                    offeredRentOutput.category = imageCategory.CategoryText; //" Single family House";
                    break;
                case 6:
                    offeredRentInput.surfaceContract = Convert.ToInt16(ConfigurationManager.AppSettings["A3SurfaceLivingDefault"]);
                    offeredRentInput.categoryCode = offeredRentOutput.CategoryCode;
                    offeredRentOutput.category = imageCategory.CategoryText; //" Condominium";
                    break;
                case -1:
                    offeredRentOutput.category = imageCategory.CategoryText;
                    break;
                default:
                    break;
            }

            if (imageCategory.CategoryCode != -1)
                CalculateRent(offeredRentInput, offeredRentOutput);

            //Saving Property Details//
            try
            {
                RealEstateData realEsateData = new RealEstateData();
                realEsateData.Image = imageBase64;
                realEsateData.Latitude = (decimal)latitude;
                realEsateData.Longitude = (decimal)longitude;
                realEsateData.DeviceId = deviceId;

                using (var ms = new MemoryStream(Convert.FromBase64String(imageBase64)))
                {
                    System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                    realEsateData.ImageSize = ms.Length / 1024;
                    realEsateData.ImageWidth = img.Width;
                    realEsateData.ImageHeight = img.Height;
                }

                RealEstateRent realEstateRent = new RealEstateRent();
                //realEstateAppraise.RealEstateId =  new Guid();
                realEstateRent.AppraisalValue = offeredRentOutput.appraisalValue;
                realEstateRent.MinAppraisalValue = offeredRentOutput.minappraisalValue;
                realEstateRent.MaxAppraisalValue = offeredRentOutput.maxappraisalValue;

                SaveRentPropertyDetails(realEsateData, realEstateRent);

            }
            catch
            {
                return offeredRentOutput;
            }

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

            ////Saving 
            try
            {
                RealEstateData realEstateData = new RealEstateData();
                realEstateData.SurfaceLiving = detailInput.surfaceLiving;
                realEstateData.LandSurface = detailInput.landSurface;
                realEstateData.RoomNb = (decimal)detailInput.roomNb;
                realEstateData.BathNb = detailInput.bathNb;
                realEstateData.BuildYear = detailInput.buildYear;
                realEstateData.MicroRating = (decimal)detailInput.microRating;
                realEstateData.CatCode = detailInput.catCode;
                realEstateData.AddressZip = detailInput.zip;
                realEstateData.AddressTown = detailInput.town;
                realEstateData.AddressStreet = detailInput.street;
                realEstateData.Country = detailInput.country;
                realEstateData.DeviceId = detailInput.deviceId;

                RealEstateAppraise realEstateAppraise = new RealEstateAppraise();
                //realEstateAppraise.RealEstateId =  new Guid();
                realEstateAppraise.AppraisalValue = appraisalOutput.appraisalValue;
                realEstateAppraise.MinAppraisalValue = appraisalOutput.minappraisalValue;
                realEstateAppraise.MaxAppraisalValue = appraisalOutput.maxappraisalValue;

                SavePricePropertyDetails(realEstateData, realEstateAppraise);

            }
            catch
            {
                return appraisalOutput;
            }

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

            ////Saving 
            try
            {
                RealEstateData realEstateData = new RealEstateData();
                realEstateData.SurfaceLiving = offeredRentInput.surfaceContract;
                //realEstateData.LandSurface = offeredRentInput.landSurface;
                realEstateData.RoomNb = (decimal)offeredRentInput.roomNb;
                //realEstateData.BathNb = offeredRentInput.bathNb;
                realEstateData.BuildYear = offeredRentInput.buildYear;
                realEstateData.Lift = offeredRentInput.lift;
                realEstateData.ObjectTypeCode = offeredRentInput.objectTypeCode;
                realEstateData.MicroRating = (decimal)offeredRentInput.qualityMicro;
                realEstateData.CatCode = offeredRentInput.categoryCode;
                realEstateData.AddressZip = offeredRentInput.address.zip;
                realEstateData.AddressTown = offeredRentInput.address.town;
                realEstateData.AddressStreet = offeredRentInput.address.street;
                realEstateData.Country = offeredRentInput.address.country;
                realEstateData.DeviceId = offeredRentInput.deviceId;

                RealEstateRent realEstateRent = new RealEstateRent();
                //realEstateAppraise.RealEstateId =  new Guid();
                realEstateRent.AppraisalValue = offeredRentOutput.appraisalValue;
                realEstateRent.MinAppraisalValue = offeredRentOutput.minappraisalValue;
                realEstateRent.MaxAppraisalValue = offeredRentOutput.maxappraisalValue;

                SaveRentPropertyDetails(realEstateData, realEstateRent);

            }
            catch
            {
                return offeredRentOutput;
            }


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

                if (reverseGeoCodeResult == null || reverseGeoCodeResult.Country == "Switzerland")                
                {
                    appraisalOutput.minappraisalValue = Convert.ToInt64(appraisalOutput.appraisalValue - (appraisalOutput.appraisalValue * randomLow));
                    appraisalOutput.maxappraisalValue = Convert.ToInt64(appraisalOutput.appraisalValue + (appraisalOutput.appraisalValue * randomHi));
                }

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

                Random rnd = new Random();
                double randomHi = 13 / 100.0; //rnd.Next(10, 15) / 100.0;
                double randomLow = 17 / 100.0; //rnd.Next(15, 20) / 100.0;

                if (reverseGeoCodeResult == null || reverseGeoCodeResult.Country == "Switzerland")
                {
                    offeredRentOutput.minappraisalValue = Convert.ToInt64(offeredRentOutput.appraisalValue - (offeredRentOutput.appraisalValue * randomLow));
                    offeredRentOutput.maxappraisalValue = Convert.ToInt64(offeredRentOutput.appraisalValue + (offeredRentOutput.appraisalValue * randomHi));
                }

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

        private void SavePricePropertyDetails(RealEstateData realEstateData, RealEstateAppraise realEsateAppraise)
        {
            RealEstateRepository realEstateRepository = new RealEstateRepository();
            realEstateRepository.savePricePropertyDetails(realEstateData, realEsateAppraise);
        }

        private void SaveRentPropertyDetails(RealEstateData realEstateData, RealEstateRent realEstateRent)
        {
            RealEstateRepository realEstateRepository = new RealEstateRepository();
            realEstateRepository.saveRentPropertyDetails(realEstateData, realEstateRent);
        }

        private void GetImageSize(string base64Image, ref Int64 imageSize, ref int imageWidth, ref int imageHeight)
        {
            byte[] image = Convert.FromBase64String(base64Image);
            using (var ms = new MemoryStream(image))
            {
                System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                imageSize = ms.Length / 1024;
                imageWidth = img.Width;
                imageHeight = img.Height;
                //var u = new Tuple<int, int>(img.Width, img.Height); // or some other data container
            }
        }

        #endregion

    }
}