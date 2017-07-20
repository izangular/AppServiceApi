using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace AppServiceApi.Util.Misc
{
    public static class ExceptionHelper
    {
        /// <summary>
        /// Gets the exception message.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="recursive">if set to <c>true</c> [recursive].</param>
        /// <param name="addStracktrace">if set to <c>true</c> [add stracktrace].</param>
        /// <returns></returns>
        public static string GetExceptionMessage(Exception ex, bool recursive = false, bool addStracktrace = false)
        {
            if (ex == null)
                return null;

            var sb = new StringBuilder();
            sb.Append(ex.Message);

            if (recursive)
            {
                while (ex.InnerException != null)
                {
                    sb.Append("\n / ");
                    sb.Append(ex.InnerException.Message);
                    ex = ex.InnerException;
                }
            }
            else if (ex.InnerException != null)
            {
                sb.Append("\nInner exception:");
                sb.Append(ex.InnerException.Message);
            }

            if (addStracktrace)
            {
                sb.Append("\nTrace:");
                sb.Append(ex.StackTrace);
            }

            return sb.ToString();
        }
    }
}