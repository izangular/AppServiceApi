using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppServiceApi.Models
{
    public class AppraisalOutput
    { 
       public string zip                { get; set; }
       public string town               { get; set; }
       public string street             { get; set; }
       public string country            { get; set; }
       public string category           { get; set; }       
       public long   appraisalValue     { get; set; }
       public long   minappraisalValue  { get; set; }
       public long   maxappraisalValue  { get; set; }
       public double microRating        { get; set; }

       public int?  surfaceLiving       { get; set; }
       public int?  landSurface         { get; set; }
       public double? roomNb            { get; set; }
       public int?  bathNb              { get; set; }
       public int?  buildYear           { get; set; }


       private int catCode;

       public int CatCode
       {
           get { return catCode; }
           set { 
                 catCode = value; 
                 switch(catCode)
                 {
                     case 5:  
                        category = "Single family House";
                         break;
                     case 6:  
                        category = "Condominium";
                         break;
                     default:
                         break;
                 }
           
           }
       }
        
    }
}