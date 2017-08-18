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

        public int fetchCategoryForImage(string imageBase64)
        {
            int category = 6; // Default is a condominium
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
                        case "villa" :
                        case "house" :
                        case "farmhouse" :
                        case "cottage" :
                            category = 5; //single family
                            break;                        
                        default:
                            break;
                    }

                    if (category != 0)
                        break;
                }

            }

            return category;
        }


    }
}