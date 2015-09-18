using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using MpcHcDotNet;
using MpcHcFrontend4.Model;
using MpcHcFrontend4.Properties;
using MpcHcFrontend4.ViewModel.Commands;
using SharpUtilities;

namespace MpcHcFrontend4.ViewModel {
    public class MainViewModel : SharpUtilities.Mvvm.INotifyBase {
        private Dispatcher dispatcher;

        private const string STATUS_CONNECTED = "Connected";
        private const string STATUS_PLAY = "Play";
        private const string STATUS_PAUSE = "Pause";

        #region Properties
        private string _statusMessage;
        public string StatusMessage {
            get {
                return _statusMessage;
            }
            set {
                if(_statusMessage == value)
                    return;

                _statusMessage = value;
                RaisePropertyChanged("StatusMessage");
            }
        }

        private Api apiInstance;
        public Api ApiInstance {
            get {
                return apiInstance;
            }
            set {
                apiInstance = value;
                RaisePropertyChanged("ApiInstance");
            }
        }

        private string logString;
        public string LogString {
            get {
                return logString;
            }
            set {
                logString = value;
                RaisePropertyChanged("LogString");
            }
        }

        private BitmapImage playPauseImage;
        public BitmapImage PlayPauseImage {
            get { return playPauseImage; }
            set {
                playPauseImage = value;
                RaisePropertyChanged("PlayPauseImage");
            }
        }

        private ObservableCollection<Media> mediae;
        public ObservableCollection<Media> Mediae {
            get { return mediae; }
            set {
                if (mediae == value) return;

                mediae = value;
                RaisePropertyChanged("Mediae");
            }
        }

        public FolderBrowserDialog FolderBrowser { get; set; }
        #endregion

        #region Commands
        public ICommand ConnectCommand { get; set; }
        public ICommand PlayPauseCommand { get; set; }
        public ICommand ImportFolderCommand { get; set; }
        public ICommand ImportMediaFileCommand { get; set; }
        public ICommand ImportSeriesFolderCommand { get; set; }
        public ICommand ProbeFileCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        #endregion

        public MainViewModel(Dispatcher uiDispatcher, IntPtr hWndSrc) {
            dispatcher = uiDispatcher;
            Mediae = new ObservableCollection<Media>();

            ConnectCommand = new ConnectCommand(this);
            PlayPauseCommand = new PlayPauseCommand(this);
            ImportFolderCommand = new ImportFolderCommand(this);
            ImportMediaFileCommand = new ImportMediaFileCommand(this);
            ImportSeriesFolderCommand = new ImportSeriesFolderCommand(this);
            ProbeFileCommand = new ProbeFileCommand(this);
            ExitCommand = new ExitCommand(this);

            ApiInstance = new Api(hWndSrc);
            ApiInstance.ChangingCurrentPlayback += OnNowPlayingChanged;
            ApiInstance.NewLogMessage += Log;
            ApiInstance.ConnectedToHost += OnConnected;
            ApiInstance.Playing += args => StatusMessage = STATUS_PLAY;
            ApiInstance.Pausing += args => StatusMessage = STATUS_PAUSE;

            FolderBrowser = new FolderBrowserDialog {ShowNewFolderButton = false};
        }

        private void OnConnected(Win32EventArgs args) {
            StatusMessage = STATUS_CONNECTED;
        }

        private void OnNowPlayingChanged(Win32EventArgs args) {
            Log("OnCurrentPlaybackChanged");
            addNowPlaying();
        }

        private void Log(string msg) {
            LogString += msg + Environment.NewLine;
            App.Log(msg);
        }

        internal void ConnectToHost() {
            apiInstance.Connect(App.Instance.MpcHcExeFile);
        }

        internal void PlayPause() {
            apiInstance.PlayPause();
        }

        internal void ImportMediaFolder() {
            string[] allowedFileExtensions = Settings.Default.ALLOWED_FILE_EXTENSIONS.Split(';');
            var filesToImport = new List<FileInfo>();

            FolderBrowser.Description = "Import media folder ...";
            FolderBrowser.ShowDialog();
            
            var selFolder = new DirectoryInfo(FolderBrowser.SelectedPath);

            var files = selFolder.GetFiles("*.*", SearchOption.AllDirectories);
            foreach (var file in files) {
                if (!allowedFileExtensions.Contains(file.Extension.Substring(1)))
                    continue;

                filesToImport.Add(file);
            }

            foreach (var file in filesToImport) {
                addToMediae(new Media(file));
            }

        }

        internal void ImportMediaFile() {
            throw new NotImplementedException();
        }

        internal void ImportSeriesFolder() {
            throw new NotImplementedException();
        }

        internal void ProbeCurrentMedia() {
            Probe(ApiInstance.NowPlaying.Path);
        }

        internal void Probe(FileInfo fileToProbe) {
            Probe(fileToProbe.FullName);
        }

        internal void Probe(string fileToProbe) {
            Task<Dictionary<string, string>>.Factory.StartNew(
                () => Ffmpeg.Probe(App.Instance.FfprobeFile, fileToProbe)    
            ).ContinueWith(
                t => parseInfo(t.Result)
            ).ContinueWith(
                t => addToMediae(t.Result)
            );
        }

        private Media parseInfo(Dictionary<string, string> dictionary) {
            return new Media(dictionary);
        }

        private void addNowPlaying() {
            var media = new Media(ApiInstance.NowPlaying.Path);
            addToMediae(media);
        }

        private void addToMediae(Media m) {
            var mediaInList = Mediae.FirstOrDefault(media => media.Path == m.Path);

            if (mediaInList == null) {
                dispatcher.BeginInvoke((Action)(() => Mediae.Add(m)), DispatcherPriority.DataBind);
            } else {
                dispatcher.BeginInvoke((Action)(() => mediaInList.UpdateWith(m)), DispatcherPriority.DataBind);
            }
        }


        internal void ShutdownApplication() {
            App.Current.Shutdown(0);
        }
    }
}