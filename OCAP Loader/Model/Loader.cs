using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Timers;
using Newtonsoft.Json;
using OCAP_Loader.Properties;
using OCAP_Loader.DataModel;

namespace OCAP_Loader.Model
{
    /// <summary>
    /// Loader service implementation
    /// </summary>
    public class Loader
    {
        #region Variables
        /// <summary>
        /// A flag, indicating a task is in progress already.
        /// </summary>
        private bool _inProgress = false;
        #endregion

        /// <summary>
        /// Default constructor
        /// </summary>
        public Loader() { }

        #region Public methods
        /// <summary>
        /// Primary method for the service. Called to start the main loop.
        /// </summary>
        public void Run()
        {
            // On run - create a timer, which will execute our required code
            Timer _timer = new Timer(Settings.Default.TaskDelay * 1000);

            // And assign an event handler to it.
            _timer.Elapsed += new ElapsedEventHandler(OnTimerElapsed);

            // Start the timer
            _timer.Start();
        }
        #endregion

        #region Private methods
        /// <summary>
        /// This method is called, to determine, whether there are any tasks awaiting.
        /// </summary>
        /// <returns>A boolean value, indicating whether there are tasks available</returns>
        private bool CheckTaskAvailability()
        {
            // Does the directory exist?
            if (Directory.Exists(Settings.Default.SourceDirectory))
            {
                // Get a list of all the JSON files in the directory
                foreach (string _file in Directory.EnumerateFiles(Settings.Default.SourceDirectory, "*.json"))
                {
                    // Was such a file previously parsed?
                    if (!History.Instance.Exists(Path.GetFileName(Path.GetFileName(_file))))
                    {
                        // Indicate that there is work to be done. No point in checking any other entries. 
                        return true;
                    }
                }

                // Return false if we are not already there.
                return false;
            }
            else
            {
                throw new FileNotFoundException("Specified target directory does not exist.");
            }
        }

        /// <summary>
        /// Checks the target directory for unparsed files.
        /// </summary>
        /// <returns>A list of files to be parsed.</returns>
        private List<string> GetTaskList()
        {
            // Create a list for the results
            List<string> _resultList = new List<string>();

            // Does the directory exist?
            if (Directory.Exists(Settings.Default.SourceDirectory))
            {
                // Get a list of all the JSON files in the directory
                foreach (string _file in Directory.EnumerateFiles(Settings.Default.SourceDirectory, "*.json"))
                {
                    // Was such a file previously parsed?
                    if (!History.Instance.Exists(Path.GetFileName(_file)))
                    {
                        // No, it was not. Add the file into the list
                        _resultList.Add(_file);
                    }
                }

                // Return the result list
                return _resultList;
            }
            else
            {
                throw new FileNotFoundException("Specified target directory does not exist.");
            }
        }

        /// <summary>
        /// Copies the specified replay to the specified target folder
        /// </summary>
        /// <param name="pReplay">Replay file to be copied</param>
        /// <param name="pDestination">Destination folder</param>
        private void CopyTo(Replay pReplay, string pDestination)
        {
            if (Directory.Exists(pDestination))
            {
                // Get the final path
                string _destinationPath = Path.Combine(pDestination, pReplay.FileName);

                // Does the specified file already exist?
                if (!File.Exists(_destinationPath))
                {
                    // Create a copy
                    File.Copy(pReplay.FullPath, _destinationPath);
                }
            }
            else
            {
                throw new DirectoryNotFoundException("Specified directory does not exist.");
            }
        }
        #endregion

        #region Event handlers
        /// <summary>
        /// An event handler, called once the timer elapses.
        /// </summary>
        /// <param name="pSource">Object invoking the event handler</param>
        /// <param name="pArgs">Invocation arguments</param>
        public void OnTimerElapsed(object pSource, ElapsedEventArgs pArgs)
        {
            // Do we have any jobs available?
            if (!_inProgress & CheckTaskAvailability())
            {
                // Set the in progress flag
                _inProgress = true;

                // Yes, we do. Get the list of tasks to be executed
                List<string> _files = GetTaskList();

                // Create a list of files to be processed
                List<Replay> _replays = new List<Replay>();

                // Read the file, parse it and create and place an order.
                foreach (string _file in _files)
                {
                    // Does the file exist?
                    if (File.Exists(_file))
                    {
                        // Yes it does. Read it and parse it.
                        Replay _replay = JsonConvert.DeserializeObject<Replay>(File.ReadAllText(_file),
                            new JsonSerializerSettings()
                            {
                                DateFormatString = "dd.MM.yyyy H:mm:ss"
                            });

                        // Set the filename & path
                        _replay.FullPath = _file;
                        _replay.FileName = Path.GetFileName(_file);

                        // Add it to the replay list
                        _replays.Add(_replay);
                    }
                    else
                    {
                        throw new FileNotFoundException("Specified file does not exist.");
                    }
                }

                // Now that all files have been parsed, orders for database loading should be issued
                Database.Instance.AddReplayList(_replays);

                // Afterwards, copy the files to the output directories
                foreach (Replay _replay in _replays)
                {
                    // Copy to the output and integration folders
                    CopyTo(_replay, Settings.Default.TargetDirectory);
                    CopyTo(_replay, Settings.Default.TargetIntegrationDirectory);

                    // Add the file to the list of processed items
                    History.Instance.Add(_replay.FileName);

                    Logger.Instance.Log(false, String.Format("{0} succesfully processed", _replay.FileName));
                }

                // Save the updated history file
                History.Instance.Save();

                // Indicate we are done
                _inProgress = false;
            }
        }
        #endregion
    }
}
