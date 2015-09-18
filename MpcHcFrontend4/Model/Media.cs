using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using SharpUtilities.Mvvm;

namespace MpcHcFrontend4.Model {
    public class Media : INotifyBase {
        private int id;
        /// <summary>
        /// Primary key column
        /// </summary>
        public int Id {
            get { return id; }
            set {
                if (id == value) return;

                id = value;
                RaisePropertyChanged("Id");
            }
        }

        private DateTime lastUpdated;
        /// <summary>
        /// Last updated column
        /// </summary>
        public DateTime LastUpdated {
            get { return lastUpdated; }
            set {
                if (lastUpdated == value) return;

                lastUpdated = value;
                RaisePropertyChanged("LastUpdated");
            }
        }

        private DateTime created;
        /// <summary>
        /// Created column
        /// </summary>
        public DateTime Created {
            get { return created; }
            set {
                if (created == value) return;

                created = value;
                RaisePropertyChanged("Created");
            }
        }

        private string path;
        /// <summary>
        /// Column with path to file
        /// </summary>
        public string Path {
            get { return path; }
            set {
                if (path == value) return;

                path = value;
                RaisePropertyChanged("Path");
                RaisePropertyChanged("FileName");
            }
        }

        public string FileName {
            get { return Path.Substring(Path.LastIndexOf('\\') + 1); }
        }

        private int pidCount;
        /// <summary>
        /// Pid count column.
        /// </summary>
        public int PidCount {
            get { return pidCount; }
            set {
                if (pidCount == value) return;

                pidCount = value;
                RaisePropertyChanged("PidCount");
            }
        }

        private string formatName;
        /// <summary>
        /// Format name column.
        /// </summary>
        public string FormatName {
            get { return formatName; }
            set {
                if (formatName == value) return;

                formatName = value;
                RaisePropertyChanged("FormatName");
            }
        }

        private string formatNameLong;
        /// <summary>
        /// Long format name column.
        /// </summary>
        public string FormatNameLong {
            get { return formatNameLong; }
            set {
                if (formatNameLong == value) return;

                formatNameLong = value;
                RaisePropertyChanged("FormatNameLong");
            }
        }

        private int playtimeSeconds;
        /// <summary>
        /// Duration column.
        /// </summary>
        public int PlaytimeSeconds {
            get { return playtimeSeconds; }
            set {
                if (playtimeSeconds == value) return;

                playtimeSeconds = value;
                RaisePropertyChanged("PlaytimeSeconds");
                RaisePropertyChanged("PlaytimeMinutes");
            }
        }

        public int PlaytimeMinutes {
            get {
                var dbl = Convert.ToDouble(PlaytimeSeconds) / 60;
                return (int)dbl;
            }
        }

        private int fileSize;
        /// <summary>
        /// File size in bytes column.
        /// </summary>
        public int FileSize {
            get { return fileSize; }
            set {
                if (fileSize == value) return;

                fileSize = value;
                RaisePropertyChanged("FileSize");
                RaisePropertyChanged("HumanReadableSize");
            }
        }

        public double HumanReadableSize {
            get {
                var dbl = Convert.ToDouble(FileSize) / 1024 / 1024 / 1024;
                return Math.Round(dbl, 2);
            }
        }

        private int bitrate;
        /// <summary>
        /// Bitrate column.
        /// </summary>
        public int Bitrate {
            get { return bitrate; }
            set {
                if (bitrate == value) return;

                bitrate = value;
                RaisePropertyChanged("Bitrate");
            }
        }

        private VideoPid videoPid;
        /// <summary>
        /// Reference to the video pid.
        /// </summary>
        public VideoPid VideoPid {
            get { return videoPid; }
            set {
                videoPid = value;
                RaisePropertyChanged("VideoPid");
            }
        }

        private ObservableCollection<AudioPid> audioPids;
        /// <summary>
        /// Referenced audio pids.
        /// </summary>
        public ObservableCollection<AudioPid> AudioPids {
            get { return audioPids; }
            set {
                if (audioPids == value) return;

                audioPids = value;
                RaisePropertyChanged("AudioPids");
            }
        }

        public Media(string mediaPath) {
            Path = mediaPath;
        }

        public Media(FileInfo mediaFile) {
            Path = mediaFile.FullName;
        }

        public Media(Dictionary<string, string> mediaInfo) {
            AudioPids = new ObservableCollection<AudioPid>();

            var pids = new List<Dictionary<string, string>>();
            var videoPidIndex = -1;

            #region PreParse
            foreach (var kv in mediaInfo) {
                var keyArr = kv.Key.Split('.');

                App.Log(kv.Key + " = " + kv.Value);
                
                switch (keyArr[0]) {

                    #region Streams
                    case "streams":
                        var pidIndex = Convert.ToInt32(keyArr[2]);

                        if(pids.Count < (pidIndex + 1))
                            pids.Add(new Dictionary<string, string>());
                        
                        if (keyArr[3] == "codec_type" && kv.Value == "video") 
                            videoPidIndex = pidIndex;
                        
                        pids.Last().Add(kv.Key, kv.Value);
                        break;
                    #endregion

                    #region Media Format
                    case "format":
                        switch (keyArr[1]) {
                            case "filename":
                                Path = kv.Value;
                                break;
                            case "nb_streams":
                                PidCount = Convert.ToInt32(kv.Value);
                                break;
                            case "format_name":
                                FormatName = kv.Value;
                                break;
                            case "format_long_name":
                                FormatNameLong = kv.Value;
                                break;
                            case "duration":
                                var dbl = Convert.ToDouble(kv.Value.Replace('.', ','));
                                PlaytimeSeconds = Convert.ToInt32(dbl);
                                break;
                            case "size":
                                FileSize = Convert.ToInt32(kv.Value);
                                break;
                            case "bit_rate":
                                Bitrate = Convert.ToInt32(kv.Value);
                                break;
                        }
                        break;
                    #endregion

                    default:
                        continue;
                }
            }
            #endregion

            if (videoPidIndex > -1) {
                var videoPid = pids[videoPidIndex];
                pids.RemoveAt(videoPidIndex);
                VideoPid = new VideoPid(videoPid);
            }

            foreach (var pid in pids) {
                var audioPid = new AudioPid(pid);
                AudioPids.Add(audioPid);
            }
        }

        public void UpdateWith(Media m) {
            PidCount = m.PidCount;
            FormatName = m.FormatName;
            FormatNameLong = m.FormatNameLong;
            PlaytimeSeconds = m.PlaytimeSeconds;
            FileSize = m.FileSize;
            Bitrate = m.Bitrate;

            if (this.VideoPid == null && m.VideoPid == null) {
                
            } else {
                VideoPid.UpdateWith(m.VideoPid);
            }

            if (m.AudioPids != null && m.AudioPids.Count > 0) {
                AudioPids = m.AudioPids;
            }

            // @TODO: Any better way to update pids?
        }
    }
}
