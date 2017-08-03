using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppServiceApi.Models
{
    public class PriceInput
    {
        public PriceInput()
        {
            zip = "8447";
            town = "Dachsen";
            street = "Dorfstrasse 13";
            externalKey = "CH_EFH";
            surfaceLiving = 800;
            buildYear = 1980;
            roomNb = 3.5;
            bathNb = 1;
            qualityMicro = 3;
        }
        
        public string zip { get; set; }
        public string town { get; set; }
        public string street { get; set; }
        public string externalKey { get; set; }
        public int surfaceLiving { get; set; }
        public int buildYear { get; set; }
        public double roomNb { get; set; }
        public int bathNb { get; set; }
        public double qualityMicro { get; set; }
        public int? surfaceGround { get; set; }

    }

    public class ExecutionInfo
    {
        public string id { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string version { get; set; }
    }

    public class Result
    {
        public int status { get; set; }
        public string text { get; set; }
        public int value { get; set; }
        public int modelId { get; set; }
        public int modelDate { get; set; }
        public string cultureOutput { get; set; }
        public double seop { get; set; }
        public int classEAA { get; set; }
        public int lowConfInt95 { get; set; }
        public int lowConfInt50 { get; set; }
        public int hiConfInt50 { get; set; }
        public int hiConfInt95 { get; set; }
        public int qualityCode { get; set; }
        public string qualityText { get; set; }
    }

    public class DataInfo
    {
        public string validationCode { get; set; }
        public string validationText { get; set; }
        public string replaceCode { get; set; }
        public string replaceText { get; set; }
        public string checkingCode { get; set; }
        public string checkingText { get; set; }
    }

    public class ParameterInfo
    {
        public string name { get; set; }
        public string type { get; set; }
        public string value { get; set; }
        public string replacedValue { get; set; }
        public string qualityCode { get; set; }
        public string qualityText { get; set; }
    }

    public class Datum
    {
        public string externalKey { get; set; }
        public ExecutionInfo executionInfo { get; set; }
        public Result result { get; set; }
        public DataInfo dataInfo { get; set; }
        public List<ParameterInfo> parameterInfo { get; set; }
    }

    public class PriceOutput
    {
        public string status { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public List<Datum> data { get; set; }
    }

   
   
}