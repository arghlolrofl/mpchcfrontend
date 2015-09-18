using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using ModernWPF;
using MpcHcFrontend4.Properties;
using MpcHcFrontend4.View;
using SharpUtilities;

namespace MpcHcFrontend4 {
    public partial class App : Application {
        #region Properties
        /// <summary>
        /// Reference to our app instance
        /// </summary>
        internal static App Instance {
            get;
            set;
        }
        /// <summary>
        /// Reference to the application's executable
        /// </summary>
        internal static FileInfo ExecutableFile {
            get;
            set;
        }
        /// <summary>
        /// The connection string for database access used by
        /// the database context
        /// </summary>
        internal static string ConnectionString {
            get { return String.Format("Data Source={0}", Instance.DbFile.FullName); }
        }
        /// <summary>
        /// Returns true if the application logs messages to a log file.
        /// </summary>
        internal static bool IsLoggingEnabled { get; set; }
        /// <summary>
        /// Reference to the ffprobe.exe file
        /// </summary>
        internal FileInfo FfprobeFile { get; set; }
        /// <summary>
        /// MediaPlayer Classic HomeCinema executable file.
        /// </summary>
        internal FileInfo MpcHcExeFile { get; set; }
        /// <summary>
        /// Reference to the database file
        /// </summary>
        internal FileInfo DbFile { get; set; }
        /// <summary>
        /// Reference to the current log file.
        /// </summary>
        internal FileInfo LogFile { get; set; }
        /// <summary>
        /// Reference to the log stream.
        /// </summary>
        internal StreamWriter LogWriter { get; set; }
        #endregion

        /// <summary>
        /// Our application entry point.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_Startup(object sender, StartupEventArgs e) {
            try {
                doStartup();

                Log("Is64BitProcess: " + Environment.Is64BitProcess);

                Exit += (o, args) => Application_Exit(o, args);

                // The rest of the initialization (connecting to mpc-hc),
                // we have to do in the overriden mainwindow method.
                //
                // @TODO: Find a better way to initialize everything in composition root
                //
            } catch (FileNotFoundException fnfEx) {
                MessageBox.Show(fnfEx.Message + Environment.NewLine + Environment.NewLine + fnfEx.FileName, "FILE MISSING");
                Current.Shutdown(1);
            } catch (NotImplementedException niEx) {
                MessageBox.Show(niEx.Message, "NOT IMPLEMENTED");
                Current.Shutdown(1);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "ERROR");
                Current.Shutdown(1);
            }

            (new MainWindow()).Show();
            // to change to dark theme
            ModernTheme.ApplyTheme(ModernTheme.Theme.Dark, ModernTheme.CurrentAccent);
        }
        /// <summary>
        /// Called, when the application exits.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private object Application_Exit(object o, ExitEventArgs args) {
            if (LogWriter != null) {
                if (LogWriter.BaseStream.CanWrite)
                    LogWriter.Close();

                LogWriter.Dispose();
                LogWriter = null;
            }

            return o;
        }
        

        /// <summary>
        /// Logs a message to the IDE output, if in debug mode.
        /// </summary>
        /// <param name="msg"></param>
        internal static void Log(string msg) {
            #if DEBUG
            Debug.WriteLine(msg);
            #endif

            if (!IsLoggingEnabled) return;

            Instance.LogWriter.WriteLine("[{0}] {1}", DateTime.Now.ToString("s"), msg);
        }


        /// <summary>
        /// Checks for ffmpeg and the database and intializes it if necessary.
        /// </summary>
        private void doStartup() {
            Instance = this;
            ExecutableFile = Env.GetExecutableFile();
            
            IsLoggingEnabled = Convert.ToBoolean(Settings.Default.ENABLE_LOGGING);
            if (IsLoggingEnabled) {
                LogFile = new FileInfo(ExecutableFile.FullName + ".log");

                LogWriter = !LogFile.Exists ? LogFile.CreateText() : LogFile.AppendText();

                if (!LogWriter.BaseStream.CanWrite) throw new Exception("Unable to open log file!");
            }

            FfprobeFile = new FileInfo(Path.Combine(Settings.Default.FFPROBE_FOLDER, "ffprobe.exe"));
            if (!FfprobeFile.Exists)
                throw new FileNotFoundException("File not found or missing!", FfprobeFile.FullName);
            
            MpcHcExeFile = new FileInfo(Path.Combine(Settings.Default.MPC_HC_FOLDER, "mpc-hc.exe"));
            if (!MpcHcExeFile.Exists)
                throw new FileNotFoundException("Could not find MPC HC executable file!", MpcHcExeFile.FullName);

            DbFile = new FileInfo(Path.Combine(
                Settings.Default.DATABASE_FOLDER, String.Format("{0}.sdf", Settings.Default.DATABASE_NAME)
            ));

            if (!DbFile.Exists)
                initializeDatabase();
            
            //@TODO: Check database integrity
        }
        /// <summary>
        /// Initializes the database.
        /// </summary>
        private void initializeDatabase() {
            Log("Auto db init not yet implemented ...");
        }
    }
}
