using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppServiceApi.Models
{
    public class GoogleVisionApiOutput
    {
        public List<Respons> responses { get; set; }
    }  

    public class Vertex
    {
        public int x { get; set; }
        public int y { get; set; }
    }

    public class BoundingPoly
    {
        public List<Vertex> vertices { get; set; }
    }

    public class Vertex2
    {
        public int x { get; set; }
        public int y { get; set; }
    }

    public class FdBoundingPoly
    {
        public List<Vertex2> vertices { get; set; }
    }

    public class Position
    {
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
    }

    public class Landmark
    {
        public string type { get; set; }
        public Position position { get; set; }
    }

    public class FaceAnnotation
    {
        public BoundingPoly boundingPoly { get; set; }
        public FdBoundingPoly fdBoundingPoly { get; set; }
        public List<Landmark> landmarks { get; set; }
        public double rollAngle { get; set; }
        public double panAngle { get; set; }
        public double tiltAngle { get; set; }
        public double detectionConfidence { get; set; }
        public double landmarkingConfidence { get; set; }
        public string joyLikelihood { get; set; }
        public string sorrowLikelihood { get; set; }
        public string angerLikelihood { get; set; }
        public string surpriseLikelihood { get; set; }
        public string underExposedLikelihood { get; set; }
        public string blurredLikelihood { get; set; }
        public string headwearLikelihood { get; set; }
    }

    public class LabelAnnotation
    {
        public string mid { get; set; }
        public string description { get; set; }
        public double score { get; set; }
    }

    public class Respons
    {
        public List<FaceAnnotation> faceAnnotations { get; set; }
        public List<LabelAnnotation> labelAnnotations { get; set; }
    }
   
    public class ImageCategory
    {
        public int CategoryCode;
        public string CategoryText;
    }

}