using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppServiceApi.Core.Model
{
    public class RealEstateAppraise
    {

        public RealEstateAppraise()
        {
            RealEstateId = null;
            AppraisalValue = 0;
            MinAppraisalValue = 0;
            MaxAppraisalValue = 0;
        }

        /// <summary>
        /// RealEstateId
        /// </summary>
        public Guid? RealEstateId { get; set; }

        /// <summary>
        /// Appraised Value
        /// </summary>
        public long AppraisalValue { get; set; }

        /// <summary>
        /// Min Appraised value
        /// </summary>
        public long MinAppraisalValue { get; set; }

        /// <summary>
        /// MaxAppraisedValue
        /// </summary>
        public long MaxAppraisalValue { get; set; }
    }
}
