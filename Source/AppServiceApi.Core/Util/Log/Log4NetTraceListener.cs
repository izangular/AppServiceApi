using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppServiceApi.Core.Util.Log
{
    public class Log4NetTraceListener : TraceListener
    {
        private const string LevelFatal = "Fatal";
        private const string LevelError = "Error";
        private const string LevelWarning = "Warn";
        private const string LevelInfo = "Info";
        private const string LevelUndefined = "Undefined";

        private readonly ILog _logInstance;

        private static readonly Dictionary<TraceEventType, string> EventDic = new Dictionary<TraceEventType, string>
        {
            {TraceEventType.Critical, LevelFatal},
            {TraceEventType.Error, LevelError},
            {TraceEventType.Warning, LevelWarning},
            {TraceEventType.Information, LevelInfo}
        };

        public static void ConfigLog4Net()
        {
            XmlConfigurator.Configure();
        }

        public Log4NetTraceListener()
        {
            _logInstance = LogManager.GetLogger(typeof(Log4NetTraceListener));
            ConfigLog4Net();
        }

        public Log4NetTraceListener(ILog log)
        {
            _logInstance = log;
            ConfigLog4Net();
        }

        public Log4NetTraceListener(string repository)
        {
            _logInstance = LogManager.GetLogger(repository);
            ConfigLog4Net();
        }

        public override void Write(object o)
        {
            if (o == null)
                return;
            WriteMessage(o, LevelUndefined);
        }

        public override void Write(object o, string category)
        {
            if (o == null)
                return;

            WriteMessage(o, category);
        }

        public override void Write(string message)
        {
            WriteMessage(message, LevelUndefined);
        }

        public override void Write(string message, string category)
        {
            WriteMessage(message, category);
        }

        public override void WriteLine(object o)
        {
            if (o == null)
                return;
            WriteMessage(o, LevelUndefined);
        }

        public override void WriteLine(string message)
        {
            WriteMessage(message, LevelUndefined);
        }

        public override void WriteLine(object o, string category)
        {
            if (o == null)
                return;
            WriteMessage(o, category);
        }

        public override void WriteLine(string message, string category)
        {
            WriteMessage(message, category);
        }


        public override void Fail(string message, string detailMessage)
        {
            WriteMessage("NotImplemented Event!", LevelWarning);
            base.Fail(message, detailMessage);
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            WriteMessage(data, EventDic.ContainsKey(eventType) ? EventDic[eventType] : LevelUndefined);
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
        {
            WriteMessage("NotImplemented Event!", LevelWarning);
            base.TraceData(eventCache, source, eventType, id, data);
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id)
        {
            WriteMessage(String.Empty, EventDic.ContainsKey(eventType) ? EventDic[eventType] : LevelUndefined);
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            WriteMessage(message, EventDic.ContainsKey(eventType) ? EventDic[eventType] : LevelUndefined);
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format,
            params object[] args)
        {
            WriteMessage("NotImplemented Event!", LevelWarning);
            base.TraceEvent(eventCache, source, eventType, id, format, args);
        }

        public override void TraceTransfer(TraceEventCache eventCache, string source, int id, string message, Guid relatedActivityId)
        {
            WriteMessage("NotImplemented Event!", LevelWarning);
            base.TraceTransfer(eventCache, source, id, message, relatedActivityId);
        }

        public override void Fail(string message)
        {
            WriteMessage(message, LevelWarning);
        }

        private void WriteMessage(object message, string category)
        {
            if (_logInstance != null)
            {
                switch (category.ToUpperInvariant())
                {
                    case "INFO":
                        _logInstance.Info(message);
                        break;
                    case "WARN":
                        _logInstance.Warn(message);
                        break;
                    case "ERROR":
                        _logInstance.Error(message);
                        break;
                    case "FATAL":
                        _logInstance.Fatal(message);
                        break;
                    default:
                        _logInstance.Debug(message);
                        break;
                }
            }
        }
    }

}
