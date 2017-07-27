using AppServiceApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppServiceApi.Util.Validation
{
    public class RequireWhenCategoryAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var detailInput = (DetailInput)validationContext.ObjectInstance;
            if (detailInput.catCode == 5)
            {
                return ValidationResult.Success;
            }
            var landSurface = value as int?;
            return landSurface == null ? new ValidationResult("The land surface field is required.") : ValidationResult.Success;
        }
    }
    public class CustomDateRangeAttribute : RangeAttribute
    {
        public CustomDateRangeAttribute()
            : base(typeof(Int32), "1900", DateTime.Today.AddYears(2).Year.ToString())
        { }
    }
}