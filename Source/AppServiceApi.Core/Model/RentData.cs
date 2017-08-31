using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppServiceApi.Core.Model
{
    public class RentData
    {
        public RealEstateData realestateData;
        public RealEstateRent realestateRent;

        public RentData()
        {
            realestateData = new RealEstateData();
            realestateRent = new RealEstateRent();
        }
    }
}
