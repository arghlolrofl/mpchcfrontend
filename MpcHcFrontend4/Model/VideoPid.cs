using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MpcHcFrontend4.Model {
    public class VideoPid : Pid {
        // streams.stream.0.index => 0
        // streams.stream.0.codec_name => mpeg4
        // streams.stream.0.codec_long_name => MPEG-4 part 2

        private double frameRate;
        /// <summary>
        /// streams.stream.0.r_frame_rate => 25/1
        /// </summary>
        public double FrameRate {
            get { return frameRate; }
            set {
                if (frameRate == value) return;

                frameRate = value;
                RaisePropertyChanged("FrameRate");
            }
        }

        private string codecTag;
        /// <summary>
        /// streams.stream.0.codec_tag_string => XVID
        /// </summary>
        public string CodecTag {
            get { return codecTag; }
            set {
                if (codecTag == value) return;

                codecTag = value;
                RaisePropertyChanged("CodecTag");
            }
        }

        private int width;
        /// <summary>
        /// streams.stream.0.width => 624
        /// </summary>
        public int Width {
            get { return width; }
            set {
                if (width == value) return;

                width = value;
                RaisePropertyChanged("VideoWidth");
            }
        }

        private int height;
        /// <summary>
        /// streams.stream.0.height => 352
        /// </summary>
        public int Height {
            get { return height; }
            set {
                if (height == value) return;

                height = value;
                RaisePropertyChanged("VideoHeight");
            }
        }

        private string aspectRatio;
        /// <summary>
        /// streams.stream.0.sample_aspect_ratio => 1:1
        /// </summary>
        public string AspectRatio {
            get { return aspectRatio; }
            set {
                if (aspectRatio == value) return;

                aspectRatio = value;
                RaisePropertyChanged("AspectRatio");
            }
        }

        private string pixelFormat;
        /// <summary>
        /// streams.stream.0.pix_fmt => yuv420p
        /// </summary>
        public string PixelFormat {
            get { return pixelFormat; }
            set {
                if (pixelFormat == value) return;

                pixelFormat = value;
                RaisePropertyChanged("PixelFormat");
            }
        }
        
        private int frameCount;
        /// <summary>
        /// streams.stream.0.nb_frames => 63038
        /// </summary>
        public int FrameCount {
            get { return frameCount; }
            set {
                if (frameCount == value) return;

                frameCount = value;
                RaisePropertyChanged("FrameCount");
            }
        }

        public VideoPid(Dictionary<string, string> pid) : base(pid) {
            foreach(var kv in pid) {
                string[] keyParts = kv.Key.Split('.');

                if (kv.Value == "N/A") continue;


                switch(keyParts[3]) {
                    case "r_frame_rate":
                        if (kv.Value.Contains("/")) {
                            var arr = kv.Value.Split('/');
                            FrameRate = Convert.ToDouble(arr[0]) / Convert.ToDouble(arr[1]);
                        } else {
                            FrameRate = Convert.ToDouble(kv.Value);
                        }
                        break;
                    case "codec_tag_string":
                        CodecTag = kv.Value;
                        break;
                    case "width":
                        Width = Convert.ToInt32(kv.Value);
                        break;
                    case "height":
                        Height = Convert.ToInt32(kv.Value);
                        break;
                    case "sample_aspect_ratio":
                        AspectRatio = kv.Value;
                        break;
                    case "pix_fmt":
                        PixelFormat = kv.Value;
                        break;
                    case "nb_frames":
                        FrameCount = Convert.ToInt32(kv.Value);
                        break;
                    default:
                        break;
                }
            }
        }

        public void UpdateWith(VideoPid pid) {
            base.UpdateWith(pid);
            FrameRate = pid.FrameRate;
            CodecTag = pid.CodecTag;
            Width = pid.Width;
            Height = pid.Height;
            AspectRatio = pid.AspectRatio;
            PixelFormat = pid.PixelFormat;
            FrameCount = pid.FrameCount;
        }
    }
}
