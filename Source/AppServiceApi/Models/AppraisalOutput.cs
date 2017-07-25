using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppServiceApi.Models
{
    public class AppraisalOutput
    { 
       public string zip    { get; set; }
       public string town   { get; set; }
       public string street { get; set; }
       public string country { get; set; }
       public string category { get; set; }
       //public int    catCode { get; set; }
       public long   appraisalValue { get; set; }
       public double rating { get; set; }


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