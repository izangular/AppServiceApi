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

        public void savePricePropertyDetails(RealEstateData realEstateData, RealEstateAppraise realEstateAppraise)
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[PRICE].[SavePropertyDetails]";

                command.Parameters.Add("@ImageBase64", SqlDbType.Text).Value = realEstateData.Image;
                command.Parameters.Add("@Latitude", SqlDbType.Decimal).Value = realEstateData.Latitude;
                command.Parameters.Add("@Longitude", SqlDbType.Decimal, 100).Value = realEstateData.Longitude;
                command.Parameters.Add("@SurfaceLiving", SqlDbType.Int, 100).Value = realEstateData.SurfaceLiving;
                command.Parameters.Add("@LandSurface", SqlDbType.Int, 100).Value = realEstateData.LandSurface;
                command.Parameters.Add("@RoomNb", SqlDbType.Decimal).Value = realEstateData.RoomNb;
                command.Parameters.Add("@BathNb", SqlDbType.Int).Value = realEstateData.BathNb;
                command.Parameters.Add("@BuildYear", SqlDbType.VarChar).Value = realEstateData.BuildYear;
                command.Parameters.Add("@MicroRating", SqlDbType.Decimal).Value = realEstateData.MicroRating;
                command.Parameters.Add("@CatCode", SqlDbType.Int).Value = realEstateData.CatCode;
                command.Parameters.Add("@AddressZip", SqlDbType.VarChar, 4).Value = realEstateData.AddressZip;
                command.Parameters.Add("@AddressTown", SqlDbType.VarChar, 100).Value = realEstateData.AddressTown;
                command.Parameters.Add("@AddressStreet", SqlDbType.VarChar, 100).Value = realEstateData.AddressStreet;
                command.Parameters.Add("@Country", SqlDbType.VarChar,100).Value = realEstateData.Country;
                command.Parameters.Add("@DeviceId", SqlDbType.VarChar, 100).Value = realEstateData.DeviceId;
                command.Parameters.Add("@UserId", SqlDbType.Int).Value = realEstateData.UserId;
                command.Parameters.Add("@ImageSize", SqlDbType.Int).Value = realEstateData.ImageSize;
                command.Parameters.Add("@ImageWidth", SqlDbType.Int).Value = realEstateData.ImageWidth;
                command.Parameters.Add("@ImageHeight", SqlDbType.Int).Value = realEstateData.ImageHeight;

                command.Parameters.Add("@RealEstateGuid", SqlDbType.UniqueIdentifier).Value = realEstateAppraise.RealEstateId;
                command.Parameters.Add("@AppraisalValue", SqlDbType.Int).Value = realEstateAppraise.AppraisalValue;
                command.Parameters.Add("@MinAppraisalValue", SqlDbType.Int).Value = realEstateAppraise.MinAppraisalValue;
                command.Parameters.Add("@MaxAppraisalValue", SqlDbType.Int).Value = realEstateAppraise.MaxAppraisalValue;

                DataLibrary.ExecuteNonQuery(command);
                
            }
        }

        public void saveRentPropertyDetails(RealEstateData realEstateData, RealEstateRent realEstateRent)
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[Rent].[SavePropertyDetails]";

                command.Parameters.Add("@ImageBase64", SqlDbType.Text).Value = realEstateData.Image;
                command.Parameters.Add("@Latitude", SqlDbType.Decimal).Value = realEstateData.Latitude;
                command.Parameters.Add("@Longitude", SqlDbType.Decimal, 100).Value = realEstateData.Longitude;
                command.Parameters.Add("@SurfaceLiving", SqlDbType.Int, 100).Value = realEstateData.SurfaceLiving;
                //command.Parameters.Add("@LandSurface", SqlDbType.Int, 100).Value = realEstateData.LandSurface;
                command.Parameters.Add("@RoomNb", SqlDbType.Decimal).Value = realEstateData.RoomNb;
                //command.Parameters.Add("@BathNb", SqlDbType.Int).Value = realEstateData.BathNb;
                command.Parameters.Add("@BuildYear", SqlDbType.VarChar).Value = realEstateData.BuildYear;
                command.Parameters.Add("@Lift", SqlDbType.VarChar).Value = realEstateData.Lift;
                command.Parameters.Add("@ObjectTypeCode", SqlDbType.VarChar).Value = realEstateData.ObjectTypeCode;

                command.Parameters.Add("@MicroRating", SqlDbType.Decimal).Value = realEstateData.MicroRating;
                command.Parameters.Add("@CatCode", SqlDbType.Int).Value = realEstateData.CatCode;
                command.Parameters.Add("@AddressZip", SqlDbType.VarChar, 4).Value = realEstateData.AddressZip;
                command.Parameters.Add("@AddressTown", SqlDbType.VarChar, 100).Value = realEstateData.AddressTown;
                command.Parameters.Add("@AddressStreet", SqlDbType.VarChar, 100).Value = realEstateData.AddressStreet;
                command.Parameters.Add("@Country", SqlDbType.VarChar, 100).Value = realEstateData.Country;
                command.Parameters.Add("@DeviceId", SqlDbType.VarChar, 100).Value = realEstateData.DeviceId;
                command.Parameters.Add("@UserId", SqlDbType.Int).Value = realEstateData.UserId;
                command.Parameters.Add("@ImageSize", SqlDbType.Int).Value = realEstateData.ImageSize;
                command.Parameters.Add("@ImageWidth", SqlDbType.Int).Value = realEstateData.ImageWidth;
                command.Parameters.Add("@ImageHeight", SqlDbType.Int).Value = realEstateData.ImageHeight;

                command.Parameters.Add("@RealEstateGuid", SqlDbType.UniqueIdentifier).Value = realEstateRent.RealEstateId;
                command.Parameters.Add("@AppraisalValue", SqlDbType.Int).Value = realEstateRent.AppraisalValue;
                command.Parameters.Add("@MinAppraisalValue", SqlDbType.Int).Value = realEstateRent.MinAppraisalValue;
                command.Parameters.Add("@MaxAppraisalValue", SqlDbType.Int).Value = realEstateRent.MaxAppraisalValue;

                DataLibrary.ExecuteNonQuery(command);

            }
        }

        #endregion

        #region Private Methods

        #endregion
    }
}
