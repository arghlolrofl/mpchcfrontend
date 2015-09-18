using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MpcHcFrontend4.ViewModel;

namespace MpcHcFrontend4.View {
    public partial class MainWindow : Window {
        private HwndSource hWndSrc;
        private MainViewModel mainViewModel;

        public MainWindow() {
            InitializeComponent();
        }

        protected override void OnSourceInitialized(EventArgs e) {
            base.OnSourceInitialized(e);

            hWndSrc = PresentationSource.FromVisual(this) as HwndSource;
            DataContext = mainViewModel = new MainViewModel(this.Dispatcher, hWndSrc.Handle);
            hWndSrc.AddHook(mainViewModel.ApiInstance.WndProc);
            mainViewModel.ConnectToHost();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e) {
            (sender as TextBox).ScrollToEnd();
        }
    }
}
