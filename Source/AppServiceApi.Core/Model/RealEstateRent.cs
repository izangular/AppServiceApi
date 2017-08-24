using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppServiceApi.Core.Model
{
    public class RealEstateRent
    {

        public RealEstateRent()
        {
            RealEstateId = null;
            AppraisalValue = null;
            MinAppraisalValue = null;
            MaxAppraisalValue = null;
        }

        /// <summary>
        /// RealEstateId
        /// </summary>
        public long? RealEstateId { get; set; }

        /// <summary>
        /// Appraised Value
        /// </summary>
        public long? AppraisalValue { get; set; }

        /// <summary>
        /// Min Appraised value
        /// </summary>
        public long? MinAppraisalValue { get; set; }

        /// <summary>
        /// MaxAppraisedValue
        /// </summary>
        public long? MaxAppraisalValue { get; set; }
    }

}
