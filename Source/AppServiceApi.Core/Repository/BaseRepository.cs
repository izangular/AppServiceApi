using IAZI.Web.SQL;
using System.Configuration;

namespace AppServiceApi.Core.Repository
{
    public class BaseRepository
    {
        protected readonly DataLibrary DataLibrary;

        public BaseRepository()
        {
            DataLibrary = new DataLibrary { ApplicationName = ConfigurationManager.AppSettings["ApplicationName"] };
            DataLibrary.SetConnection(ConfigurationManager.ConnectionStrings["MobileDBConnection"].ConnectionString);
        }
    }
}
