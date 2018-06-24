using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using OCAP_Loader.DataModel;
using OCAP_Loader.Properties;

namespace OCAP_Loader.Model
{
    public sealed class History
    {
        #region Variables
        /// <summary>
        /// Instance of class
        /// </summary>
        private static readonly History _instance = new History();

        /// <summary>
        /// List of files
        /// </summary>
        private List<string> _files = new List<string>();
        #endregion

        #region Properties
        /// <summary>
        /// Instance property
        /// </summary>
        public static History Instance
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
        static History() { }

        /// <summary>
        /// Private constructor
        /// </summary>
        private History()
        {
            // Once the constructor is run, parsed file data should be retrieved.
            if (File.Exists(Settings.Default.TaskHistoryFile))
            {
                // Read the file and convert it to a readable format
                this._files = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(Settings.Default.TaskHistoryFile));
            }
            else
            {
                Logger.Instance.Log(false, "No history file exists...");
            }
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Adds a file to the list of parsed files.
        /// </summary>
        /// <param name="pFileName">Name of the file to be added to the list.</param>
        public void Add(string pFileName)
        {
            if (!_files.Contains(pFileName))
            {
                _files.Add(pFileName);
            }
        }

        /// <summary>
        /// Checks the list to see, if such a file has been already parsed.
        /// </summary>
        /// <param name="pFileName">Name of the file to be checked.</param>
        /// <returns></returns>
        public bool Exists(string pFileName)
        {
            return _files.Contains(pFileName);
        }

        /// <summary>
        /// Writes the complete list to the file.
        /// </summary>
        public void Save()
        {
            // In order to write the changes into the file
            // If file does not exist, this should attempt to create it.
            File.WriteAllText(Settings.Default.TaskHistoryFile,
                JsonConvert.SerializeObject(_files, Formatting.Indented));
        }

        #endregion

    }
}
