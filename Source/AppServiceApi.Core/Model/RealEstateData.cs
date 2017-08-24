using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppServiceApi.Core.Model
{
    public class RealEstateData
    {
        public RealEstateData()
        {
            Id = -1;
            Image = null;
            Latitude = 0;
            Longitude = 0;
            SurfaceLiving = null;
            LandSurface = null;
            RoomNb = null;
            BathNb = null;
            BuildYear = null;
            MicroRating = null;
            CatCode = null;
            AddressZip = null;
            AddressStreet = null;
            Lift = null;
            NNFilter = null;
            NbCompareProperties = null;
            Country = null;
            DeviceId = null;
            UserId = null;
            ImageSize = null;
            ImageWidth = null;
            ImageHeight = null;
        }


        /// <summary>
        /// Gets the Id of the realestate
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// Base64 image string
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// latitude
        /// </summary>
        public decimal? Latitude { get; set; }

        /// <summary>
        /// longitude
        /// </summary>
        public decimal? Longitude { get; set; }

        /// <summary>
        /// SurfaceLiving
        /// </summary>
        public decimal? SurfaceLiving { get; set; }

        /// <summary>
        /// LandSurface
        /// </summary>
        public decimal? LandSurface { get; set; }

        /// <summary>
        /// Number of Rooms
        /// </summary>
        public decimal? RoomNb { get; set; }

        /// <summary>
        /// Number of Bathrooms
        /// </summary>
        public int? BathNb { get; set; }

        /// <summary>
        /// BuildYear
        /// </summary>
        public int? BuildYear { get; set; }

        /// <summary>
        /// Lift
        /// </summary>
        public int? Lift { get; set; }

        /// <summary>
        /// ObjectTypeCode
        /// </summary>
        public int? ObjectTypeCode { get; set; }

        /// <summary>
        /// MicroQuality
        /// </summary>
        public decimal? MicroRating { get; set; }

        /// <summary>
        /// Category code 
        /// </summary>
        public int? CatCode { get; set; }
        
        /// <summary>
        /// AddressZip
        /// </summary>
        public string AddressZip { get; set; }

        /// <summary>
        /// AddressStreet
        /// </summary>
        public string AddressStreet { get; set; }

        /// <summary>
        /// AddressTown
        /// </summary>
        public string AddressTown { get; set; }

        /// <summary>
        /// Filter description for Rent
        /// </summary>
        public string NNFilter { get; set; }

        /// <summary>
        /// Rent Compare properties
        /// </summary>
        public int? NbCompareProperties { get; set; }

        /// <summary>
        /// Country code
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Device ID
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// Auth service UserID
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// Image Size
        /// </summary>
        public Int64? ImageSize { get; set; }

        /// <summary>
        /// Image Width
        /// </summary>
        public int? ImageWidth { get; set; }

        /// <summary>
        /// Image Height
        /// </summary>
        public int? ImageHeight { get; set; }


    }
}
