using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppServiceApi.Models
{
    public class GoogleVisionApiInput
    {
        public List<Request> requests { get; set; }
        
        public GoogleVisionApiInput()
        {

            Feature feature = new Feature
            {
                type = "LABEL_DETECTION",
                maxResults = 10
            };

            Feature faceFeature = new Feature
            {
                type = "FACE_DETECTION",
                maxResults = 1
            };           

            Image image = new Image
            {
                content = ""
            };
            requests = new List<Request>();
            Request request = new Request();
            request.features = new List<Feature>();
            request.features.Add(feature);
            request.features.Add(faceFeature);
            request.image = image;
            requests.Add(request);          
                
        } 
        
    }
       
    public class Request
    {
        public Image image { get; set; }
        public List<Feature> features { get; set; }
    }

    public class Feature
    {
        public string type { get; set; }
        public int maxResults { get; set; }
    }

    public class Image
    {
        public string content { get; set; }

        
    }



}