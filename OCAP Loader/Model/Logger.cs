using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCAP_Loader.Model
{
    public sealed class Logger
    {
        #region Variables
        /// <summary>
        /// Instance of class
        /// </summary>
        private static readonly Logger _instance = new Logger();

        /// <summary>
        /// An event log instance
        /// </summary>
        private static EventLog _eventLog = new EventLog()
        {
            Source = "OCAP Loader"
        };
        #endregion

        #region Properties
        /// <summary>
        /// Instance property
        /// </summary>
        public static Logger Instance
        {
            get
            {
                return _instance;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Explicit static constructor
        /// </summary>
        static Logger() { }

        /// <summary>
        /// Private constructor
        /// </summary>
        private Logger() {

        }
        #endregion

        #region Public methods
        /// <summary>
        /// Logs an error / informational message into the event log
        /// </summary>
        /// <param name="pIsError">A boolean, indicating whether the entry is an error or informational.</param>
        /// <param name="pMessage">Message to be logged (optional)</param>
        /// <param name="pEx">Exception to be logged (optional)</param>
        public void Log(bool pIsError, string pMessage = "", Exception pEx = null)
        {
            // Log an entry into the event log
            // 
            _eventLog.WriteEntry((pEx != null) ? pEx.ToString() : pMessage,
                pIsError ? EventLogEntryType.Error : EventLogEntryType.Information);  
        }
        #endregion
    }
}
