using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
using OCAP_Loader.DataModel;
using OCAP_Loader.Properties;


namespace OCAP_Loader.Model
{
    public sealed class Database
    {
        #region Variables
        /// <summary>
        /// Instance of class
        /// </summary>
        private static readonly Database _instance = new Database();
        #endregion

        #region Properties
        /// <summary>
        /// Instance property
        /// </summary>
        public static Database Instance
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
        static Database() {}

        /// <summary>
        /// Private constructor
        /// </summary>
        private Database() {}
        #endregion

        #region Public methods
        /// <summary>
        /// Method to add replays into the database.
        /// </summary>
        /// <param name="pReplays">A list of replays to be processed</param>
        public void AddReplayList (List<Replay> pReplays)
        {
            // Get a database connection which we will use
            using (SQLiteConnection _connection = (new SQLiteConnection(String.Format("Data Source={0};Version=3;", Settings.Default.DatabasePath))).OpenAndReturn())
            {
                // For each of the replays
                foreach (Replay _replay in pReplays)
                {
                    // Execute an add replay command
                    AddReplay(_connection, _replay);
                }
            }

            // Log into the event log
            Logger.Instance.Log(false, String.Format("{0} entries successfully added to database", pReplays.Count));
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Adds an entry to the database
        /// </summary>
        /// <param name="pConnection">Connection to be used when adding</param>
        /// <param name="pReplay">Data of the file to be added</param>
        private void AddReplay(SQLiteConnection pConnection, Replay pReplay)
        {
            // Create a command to be executed
            SQLiteCommand _addCommand = new SQLiteCommand(
                String.Format("INSERT INTO operations " +
                    "(world_name, mission_name, mission_duration, filename, date, type) " +
                    "values ('{0}', '{1}', {2}, '{3}', '{4}', 'wog3')",
                    pReplay.WorldName, pReplay.MissionName, (int)Math.Round(pReplay.EndFrame * pReplay.CaptureDelay), pReplay.FileName, pReplay.DateTimeEnd.ToString("yyyy-MM-dd")),
                pConnection);

            // Execute it
            _addCommand.ExecuteNonQuery();
        }
        #endregion
    }
}

