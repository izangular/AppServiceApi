using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppServiceApi.Core.Model
{
    public class PriceData
    {
        public RealEstateData realestateData;
        public RealEstateAppraise realestateAppraise;

        public PriceData()
        {
            realestateData = new RealEstateData();
            realestateAppraise = new RealEstateAppraise();
        }

    }
}
