﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.18444
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MpcHcFrontend4.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "11.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("e:\\tools\\ffmpeg\\bin\\")]
        public string FFPROBE_FOLDER {
            get {
                return ((string)(this["FFPROBE_FOLDER"]));
            }
            set {
                this["FFPROBE_FOLDER"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("c:\\Users\\neo\\Desktop\\")]
        public string DATABASE_FOLDER {
            get {
                return ((string)(this["DATABASE_FOLDER"]));
            }
            set {
                this["DATABASE_FOLDER"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("mediadb")]
        public string DATABASE_NAME {
            get {
                return ((string)(this["DATABASE_NAME"]));
            }
            set {
                this["DATABASE_NAME"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("true")]
        public string ENABLE_LOGGING {
            get {
                return ((string)(this["ENABLE_LOGGING"]));
            }
            set {
                this["ENABLE_LOGGING"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("MpcHcFrontend4.log")]
        public string LOG_FILE {
            get {
                return ((string)(this["LOG_FILE"]));
            }
            set {
                this["LOG_FILE"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("e:\\tools\\mpc-hc-1.7.1-x86\\")]
        public string MPC_HC_FOLDER {
            get {
                return ((string)(this["MPC_HC_FOLDER"]));
            }
            set {
                this["MPC_HC_FOLDER"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("avi;divx;flv;mkv;mp4;mpg")]
        public string ALLOWED_FILE_EXTENSIONS {
            get {
                return ((string)(this["ALLOWED_FILE_EXTENSIONS"]));
            }
            set {
                this["ALLOWED_FILE_EXTENSIONS"] = value;
            }
        }
    }
}
