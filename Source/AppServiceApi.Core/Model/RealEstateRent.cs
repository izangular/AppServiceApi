using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppServiceApi.Core.Model
{
    public class RealEstateRent
    {
        
        RealEstateRent()
        {
            RealEstateId = 0;
            AppraisalValue = 0;
            MinAppraisalValue = 0;
            MaxAppraisalValue = 0;
        }

        /// <summary>
        /// RealEstateId
        /// </summary>
        public long RealEstateId { get; set; }

        /// <summary>
        /// Appraised Value
        /// </summary>
        public int AppraisalValue { get; set; }

        /// <summary>
        /// Min Appraised value
        /// </summary>
        public int MinAppraisalValue { get; set; }

        /// <summary>
        /// MaxAppraisedValue
        /// </summary>
        public int MaxAppraisalValue { get; set; }
    }

}
