using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpUtilities.Mvvm;

namespace MpcHcFrontend4.Model {
    public abstract class Pid : INotifyBase {
        private int index;
        /// <summary>
        /// streams.stream.0.index => 0
        /// </summary>
        public int Index {
            get { return index; }
            set {
                if (index == value) return;

                index = value;
                RaisePropertyChanged("Index");
            }
        }
        private string codecName;
        /// <summary>
        /// streams.stream.0.codec_name => mpeg4
        /// </summary>
        public string CodecName {
            get { return codecName; }
            set {
                if (codecName == value) return;

                codecName = value;
                RaisePropertyChanged("CodecName");
            }
        }

        private string codecNameLong;
        /// <summary>
        /// streams.stream.0.codec_long_name => MPEG-4 part 2
        /// </summary>
        public string CodecNameLong {
            get { return codecNameLong; }
            set {
                if (codecNameLong == value) return;

                codecNameLong = value;
                RaisePropertyChanged("CodecNameLong");
            }
        }

        protected Pid(Dictionary<string, string> pid) {
            foreach (var kv in pid) {
                string[] keyParts = kv.Key.Split('.');

                switch (keyParts[3]) {
                    case "index":
                        Index = Convert.ToInt32(kv.Value);
                        break;
                    case "codec_name":
                        CodecName = kv.Value;
                        break;
                    case "codec_long_name":
                        CodecNameLong = kv.Value;
                        break;
                    default:
                        break;
                }
            }
        }

        protected void UpdateWith(Pid pid) {
            Index = pid.Index;
            CodecName = pid.CodecName;
            CodecNameLong = pid.CodecNameLong;
        }
    }
}
