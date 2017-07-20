using log4net;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;

namespace AppServiceApi.Util.Log
{
    public class Log4NetExceptionLogger : ExceptionLogger
    {
        private static readonly ILog LogInstance = LogManager.GetLogger(typeof(Log4NetExceptionLogger));
        public override void Log(ExceptionLoggerContext context)
        {
            LogInstance.Error( RequestToString(context));
        }

        private static string RequestToString(ExceptionLoggerContext Context)
        {
            var message = new StringBuilder();
            if (GetLocalIPAddress() != null)
                message.AppendFormat("IP: {0}", GetLocalIPAddress()).AppendLine();
            if (Context.Request.Method != null)
                message.AppendFormat("Error in Method: {0}", Context.Request.Method).AppendLine();
            if (Context.Request.RequestUri != null)
                message.AppendFormat("Requested Uri: {0}", Context.Request.RequestUri).AppendLine();



            message.Append("Exception Details: ");
            if (Context.Exception.Message != null)
            {
                message.Append("Message").AppendLine();
                message.Append("--------------------").AppendLine();
                message.AppendLine(Context.Exception.Message).AppendLine();
            }
            if (Context.Exception.Source != null)
            {
                message.Append("Source").AppendLine();
                message.Append("--------------------").AppendLine();
                message.AppendLine(Context.Exception.Source).AppendLine();
              
            }
            if (Context.Exception.InnerException.ToString() != null)
            {
                message.Append("Inner Exception").AppendLine();
                message.Append("--------------------").AppendLine();
                message.AppendLine(Context.Exception.InnerException.ToString()).AppendLine();
            }

            if (Context.Exception.StackTrace != null)
            {
                message.Append("Stack Trace").AppendLine();
                message.Append("--------------------").AppendLine();
                message.AppendLine(Context.Exception.StackTrace).AppendLine();
            }
            if (Context.Exception.TargetSite != null)
            {
                message.Append("Target Site").AppendLine();
                message.Append("--------------------").AppendLine();
                message.AppendLine(Context.Exception.TargetSite.ToString()).AppendLine();
            }

            message.AppendLine(AppendFooter());

            return message.ToString();
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return null;
        }

        public static string AppendFooter()
        {
            var footer = new StringBuilder();
            footer.AppendLine("Webmaster,").AppendLine();

            footer.AppendLine("IAZI AG – CIFI SA ");
            footer.AppendLine("Tramstrasse 10 ");
            footer.AppendLine("8050 Zürich ");
            footer.AppendLine("www.iazi.ch").AppendLine();

            footer.AppendLine("The information transmitted is intended only for the person or entity to which it is addressed. ");
            footer.AppendLine("If you received this in error, please contact the sender and delete the material from computer.");
            footer.AppendLine("Thank you.");
            return footer.ToString();
        }
    }
}
