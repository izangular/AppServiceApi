using AppServiceApi.Core.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IAZI.Web.SQL;

namespace AppServiceApi.Core.Repository
{
    public class RealEstateRepository : BaseRepository
    {
        public RealEstateRepository() : base() { }

        #region Public Methods

        public void savePricePropertyDetails(PriceData priceData)
        {
            //if (userAgent == null)
              String  userAgent = String.Empty;

            using (SqlCommand command = new SqlCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[PRICE].[SavePropertyDetails]";

                command.Parameters.Add("@ImageBase64", SqlDbType.VarChar,-1).Value = priceData.realestateData.Image;
                command.Parameters.Add("@Latitude", SqlDbType.Decimal).Value = priceData.realestateData.Latitude;
                command.Parameters.Add("@Longitude", SqlDbType.Decimal, 100).Value = priceData.realestateData.Longitude;
                command.Parameters.Add("@SurfaceLiving", SqlDbType.Int, 100).Value = priceData.realestateData.SurfaceLiving;
                command.Parameters.Add("@LandSurface", SqlDbType.Int, 100).Value = priceData.realestateData.LandSurface;
                command.Parameters.Add("@RoomNb", SqlDbType.Decimal).Value = priceData.realestateData.RoomNb;
                command.Parameters.Add("@BathNb", SqlDbType.Int).Value = priceData.realestateData.BathNb;
                command.Parameters.Add("@BuildYear", SqlDbType.VarChar).Value = priceData.realestateData.BuildYear;
                command.Parameters.Add("@MicroRating", SqlDbType.Decimal).Value = priceData.realestateData.MicroRating;
                command.Parameters.Add("@CatCode", SqlDbType.Int).Value = priceData.realestateData.CatCode;
                command.Parameters.Add("@AddressZip", SqlDbType.VarChar, 4).Value = priceData.realestateData.AddressZip;
                command.Parameters.Add("@AddressTown", SqlDbType.VarChar, 100).Value = priceData.realestateData.AddressTown;
                command.Parameters.Add("@AddressStreet", SqlDbType.VarChar, 100).Value = priceData.realestateData.AddressStreet;
                command.Parameters.Add("@Country", SqlDbType.VarChar, 100).Value = priceData.realestateData.Country;
                command.Parameters.Add("@DeviceId", SqlDbType.VarChar, 100).Value = priceData.realestateData.DeviceId;
                command.Parameters.Add("@UserId", SqlDbType.Int).Value = priceData.realestateData.UserId;
                command.Parameters.Add("@ImageSize", SqlDbType.Int).Value = priceData.realestateData.ImageSize;
                command.Parameters.Add("@ImageWidth", SqlDbType.Int).Value = priceData.realestateData.ImageWidth;
                command.Parameters.Add("@ImageHeight", SqlDbType.Int).Value = priceData.realestateData.ImageHeight;                

                command.Parameters.Add("@RealEstateGuid", SqlDbType.UniqueIdentifier).Value = priceData.realestateAppraise.RealEstateId;
                command.Parameters.Add("@AppraisalValue", SqlDbType.BigInt).Value = priceData.realestateAppraise.AppraisalValue;
                command.Parameters.Add("@MinAppraisalValue", SqlDbType.BigInt).Value = priceData.realestateAppraise.MinAppraisalValue;
                command.Parameters.Add("@MaxAppraisalValue", SqlDbType.BigInt).Value = priceData.realestateAppraise.MaxAppraisalValue;
                command.Parameters.Add("@UserAgent", SqlDbType.VarChar, -1).Value = userAgent;

                DataLibrary.ExecuteNonQuery(command);
                
            }
        }

        public void saveRentPropertyDetails(RentData rentData)
        {
            //if (userAgent == null)
            String userAgent = String.Empty;

            using (SqlCommand command = new SqlCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[Rent].[SavePropertyDetails]";

                command.Parameters.Add("@ImageBase64", SqlDbType.VarChar,-1).Value = rentData.realestateData.Image;
                command.Parameters.Add("@Latitude", SqlDbType.Decimal).Value = rentData.realestateData.Latitude;
                command.Parameters.Add("@Longitude", SqlDbType.Decimal, 100).Value = rentData.realestateData.Longitude;
                command.Parameters.Add("@SurfaceLiving", SqlDbType.Int, 100).Value = rentData.realestateData.SurfaceLiving;
                //command.Parameters.Add("@LandSurface", SqlDbType.Int, 100).Value = realEstateData.LandSurface;
                command.Parameters.Add("@RoomNb", SqlDbType.Decimal).Value = rentData.realestateData.RoomNb;
                //command.Parameters.Add("@BathNb", SqlDbType.Int).Value = realEstateData.BathNb;
                command.Parameters.Add("@BuildYear", SqlDbType.VarChar).Value = rentData.realestateData.BuildYear;
                command.Parameters.Add("@Lift", SqlDbType.VarChar).Value = rentData.realestateData.Lift;
                command.Parameters.Add("@ObjectTypeCode", SqlDbType.VarChar).Value = rentData.realestateData.ObjectTypeCode;

                command.Parameters.Add("@MicroRating", SqlDbType.Decimal).Value = rentData.realestateData.MicroRating;
                command.Parameters.Add("@CatCode", SqlDbType.Int).Value = rentData.realestateData.CatCode;
                command.Parameters.Add("@AddressZip", SqlDbType.VarChar, 4).Value = rentData.realestateData.AddressZip;
                command.Parameters.Add("@AddressTown", SqlDbType.VarChar, 100).Value = rentData.realestateData.AddressTown;
                command.Parameters.Add("@AddressStreet", SqlDbType.VarChar, 100).Value = rentData.realestateData.AddressStreet;
                command.Parameters.Add("@Country", SqlDbType.VarChar, 100).Value = rentData.realestateData.Country;
                command.Parameters.Add("@DeviceId", SqlDbType.VarChar, 100).Value = rentData.realestateData.DeviceId;
                command.Parameters.Add("@UserId", SqlDbType.Int).Value = rentData.realestateData.UserId;
                command.Parameters.Add("@ImageSize", SqlDbType.Int).Value = rentData.realestateData.ImageSize;
                command.Parameters.Add("@ImageWidth", SqlDbType.Int).Value = rentData.realestateData.ImageWidth;
                command.Parameters.Add("@ImageHeight", SqlDbType.Int).Value = rentData.realestateData.ImageHeight;

                command.Parameters.Add("@RealEstateGuid", SqlDbType.UniqueIdentifier).Value = rentData.realestateRent.RealEstateId;
                command.Parameters.Add("@AppraisalValue", SqlDbType.BigInt).Value = rentData.realestateRent.AppraisalValue;
                command.Parameters.Add("@MinAppraisalValue", SqlDbType.BigInt).Value = rentData.realestateRent.MinAppraisalValue;
                command.Parameters.Add("@MaxAppraisalValue", SqlDbType.BigInt).Value = rentData.realestateRent.MaxAppraisalValue;
                command.Parameters.Add("@UserAgent", SqlDbType.VarChar, -1).Value = userAgent;

                DataLibrary.ExecuteNonQuery(command);

            }
        }

        public void saveException(string exceptionText, string innerException, string stackTrace)
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[ERROR].[logException]";
                command.Parameters.Add("@ExpMessage", SqlDbType.VarChar, -1).Value = exceptionText;
                command.Parameters.Add("@InnerException", SqlDbType.VarChar, -1).Value = innerException;
                command.Parameters.Add("@StackTrace", SqlDbType.VarChar, -1).Value = stackTrace;

                DataLibrary.ExecuteNonQuery(command);
            }

           
        }
        
        #endregion

        #region Private Methods

        #endregion
    }
}
