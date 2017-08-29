using AppServiceApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace AppServiceApi.Util.Helper
{
    public class GoogleVisionApi
    {
        //const string apiKey = "AIzaSyDySexF3apM0gm6jCkcEb_fTIWINIryKkU";
        string apiKey = ConfigurationManager.AppSettings["GoogleVisionApiKey"];
        IAZIClientSync IAZIClientSync;
        GoogleVisionApiInput googleVisionApiInput;

        public GoogleVisionApi()
        {
            IAZIClientSync = new IAZIClientSync();
            googleVisionApiInput = new GoogleVisionApiInput();
        }
        
        public GoogleVisionApiOutput AnalyseImage(string imageBase64)
        {
            string url = String.Format("{0}?key={1}", ConfigurationManager.AppSettings["GoogleVisionApi"], apiKey); 
            googleVisionApiInput.requests[0].image.content = imageBase64;
            string postData = JsonConvert.SerializeObject(googleVisionApiInput, Formatting.Indented);

            try
            {
                string result = IAZIClientSync.postApiRequest(url, postData, null);
                return JsonConvert.DeserializeObject<GoogleVisionApiOutput>(result);
            }
            catch(Exception ex)
            {
                return null;
            }
            
        }

        public ImageCategory fetchCategoryForImage(string imageBase64)
        {
            int category = 0;
            double house = 0.0;
            double apartment = 0.0;
            string firsttwoLabels = String.Empty;
            ImageCategory imageCategory = new ImageCategory();
            GoogleVisionApiOutput googleVisionApiOutput = AnalyseImage(imageBase64);

            if (googleVisionApiOutput.responses != null && googleVisionApiOutput.responses.Count > 0 && googleVisionApiOutput.responses[0].labelAnnotations.Count > 0)
            {
                /* Mark the parameters */
                foreach (LabelAnnotation labelAnnotation in googleVisionApiOutput.responses[0].labelAnnotations)
                {
                    switch (labelAnnotation.description)
                    {                      
                        case "apartment":
                            apartment = labelAnnotation.score;
                            break;
                       
                        case "house" :
                            house = labelAnnotation.score;
                            break;
                    }

                }

                /* Statistically analysed condition from RPO */
                if (apartment > 0 || house > 0)
                {
                    if (apartment > 0.6561459)
                        imageCategory.CategoryCode = 6;
                    else
                    {
                        if (house > 0.6121141)
                            imageCategory.CategoryCode = 5;
                        else
                            imageCategory.CategoryCode = 6;
                    }
                }
                else                
                    imageCategory.CategoryCode = -1;// Not house or Apartment


                /* Set the Correct text */
                if (imageCategory.CategoryCode == -1)
                    imageCategory.CategoryText = googleVisionApiOutput.responses[0].labelAnnotations[0].description + " / " + googleVisionApiOutput.responses[0].labelAnnotations[1].description;
                else if (imageCategory.CategoryCode == 5)
                    imageCategory.CategoryText = "Single family House";
                else if (imageCategory.CategoryCode == 6)
                    imageCategory.CategoryText = "Condominum";

            }

            return imageCategory;

        }

        /// <summary>
        /// Old function to determine the type of input image (Not used kept for reference)
        /// </summary>
        /// <param name="imageBase64"></param>
        /// <returns></returns>

        public int fetchCategoryForImage_V1(string imageBase64)
        {
            int category = 0;
            int house = 0;
            GoogleVisionApiOutput  googleVisionApiOutput = AnalyseImage(imageBase64);

            if (googleVisionApiOutput.responses != null &&  googleVisionApiOutput.responses.Count > 0 && googleVisionApiOutput.responses[0].labelAnnotations.Count > 0)
            {               
                foreach (LabelAnnotation labelAnnotation in googleVisionApiOutput.responses[0].labelAnnotations)
                {  
                    switch (labelAnnotation.description)
                    {                       
                        case "condominium":
                        case "apartment":
                            category = 6; //condominium
                            break;
                        
                        case "house" :
                            house = 1;
                            break;
 
                        case "villa":
                        case "farmhouse" :
                        case "cottage" :
                        case "mansion":
                            category = 5; //single family
                            break;                        
                        default:
                            break;
                    }

                    if (category != 0)
                        break;
                }

                if (category == 0 && house == 1)
                    category = 5; /// if category = 0 & house was 1 then it is assumed to be single family.

                if(category == 0)
                    category = 6;  // Default is a condominium
            }

            return category;
        }


    }
}