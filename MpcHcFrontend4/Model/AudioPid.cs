using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MpcHcFrontend4.Model {
    public class AudioPid : Pid {
        //streams.stream.1.index => 1
        //streams.stream.1.codec_name => mp3
        //streams.stream.1.codec_long_name => MP3 (MPEG audio layer 3)

        private string sampleFormat;
        /// <summary>
        /// streams.stream.1.sample_fmt => s16p
        /// </summary>
        public string SampleFormat {
            get { return sampleFormat; }
            set {
                if (sampleFormat == value) return;

                sampleFormat = value;
                RaisePropertyChanged("SampleFormat");
            }
        }

        private int sampleRate;
        /// <summary>
        /// streams.stream.1.sample_rate => 48000
        /// </summary>
        public int SampleRate {
            get { return sampleRate; }
            set {
                if (sampleRate == value) return;

                sampleRate = value;
                RaisePropertyChanged("SampleRate");
            }
        }

        private int channelCount;
        /// <summary>
        /// streams.stream.1.channels => 2
        /// </summary>
        public int ChannelCount {
            get { return channelCount; }
            set {
                if (channelCount == value) return;

                channelCount = value;
                RaisePropertyChanged("ChannelCount");
            }
        }

        public AudioPid(Dictionary<string, string> pid) : base(pid) {
            foreach(var kv in pid) {
                string[] keyParts = kv.Key.Split('.');

                switch(keyParts[3]) {
                    case "sample_fmt":
                        SampleFormat = kv.Value;
                        break;
                    case "sample_rate":
                        SampleRate = Convert.ToInt32(kv.Value);
                        break;
                    case "channels":
                        ChannelCount = Convert.ToInt32(kv.Value);
                        break;
                    default:
                        break;
                }
            }
        }

        public void UpdateWith(AudioPid pid) {
            base.UpdateWith(pid);
            SampleFormat = pid.SampleFormat;
            SampleRate = pid.SampleRate;
            ChannelCount = pid.ChannelCount;
        }

    }
}
