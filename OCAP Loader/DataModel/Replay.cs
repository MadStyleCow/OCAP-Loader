using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCAP_Loader.DataModel
{
    [Serializable]
    public class Replay
    {
        #region Properties
        /// <summary>
        /// Name of the file
        /// </summary>
        [JsonIgnore]
        public string FileName { get; set; }

        /// <summary>
        /// Full path to the file on disk
        /// </summary>
        [JsonIgnore]
        public string FullPath { get; set; }

        /// <summary>
        /// Name of the world (island)
        /// </summary>
        [JsonProperty("worldName")]
        public string WorldName { get; set; }

        /// <summary>
        /// Name of the mission
        /// </summary>
        [JsonProperty("missionName")]
        public string MissionName { get; set; }

        /// <summary>
        /// Author
        /// </summary>
        [JsonProperty("missionAuthor")]
        public string MissionAuthor { get; set; }

        /// <summary>
        /// Capture delay
        /// </summary>
        [JsonProperty("captureDelay")]
        public double CaptureDelay { get; set; }

        /// <summary>
        /// Total duration in frames
        /// </summary>
        [JsonProperty("endFrame")]
        public long EndFrame { get; set; }

        /// <summary>
        /// Start date
        /// </summary>
        [JsonProperty("dateTimeStart")]
        public DateTime DateTimeStart { get; set; }

        /// <summary>
        /// End date
        /// </summary>
        [JsonProperty("dateTimeEnd")]
        public DateTime DateTimeEnd { get; set; }

        /*
        /// <summary>
        /// An array of entities
        /// </summary>
        [JsonProperty("entities")]
        public object[] Entities { get; set; }

        /// <summary>
        /// An array of events
        /// </summary>
        [JsonProperty("events")]
        public object[][] Events { get; set; }

        /// <summary>
        /// An array of system entries
        /// </summary>
        [JsonProperty("system")]
        public double[][] System { get; set; }

        /// <summary>
        /// An array of markers
        /// </summary>
        [JsonProperty("markers")]
        public object[] Markers { get; set; }
        */
        #endregion

        #region Constructors
        public Replay() { }
        #endregion
    }
}