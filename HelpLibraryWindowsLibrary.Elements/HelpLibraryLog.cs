using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpLibrary.UWP.Elements
{
    /// <summary>
    /// Genera un nuevo registro "Log" y lo transmite por el modelo evento.
    /// </summary>
    public class HelpLibraryLog
    {
        /// <summary>
        /// 
        /// </summary>
        public static event EventHandler<LogEventArgs> LogEvent;

         
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnEventLog(LogEventArgs e)
        {
           
            LogEvent?.Invoke(this, e);
        }

        public HelpLibraryLog(string Message)
        {
            OnEventLog(new LogEventArgs() { Message = Message, Time = DateTime.Now });
        }

        public HelpLibraryLog(Exception exception, object sender) => OnEventLog(new LogEventArgs() { Exception = exception, Time = DateTime.Now, Sender = sender, Message = exception.Message });




    }

    public class LogEventArgs : EventArgs
    {
        public Exception Exception { get; set; }

        public object Sender { get; set; }

        public DateTime Time { get; set; }

        public string Message { get; set; }

    }
}
