using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace MpcHcFrontend4.ViewModel.Commands {
    public class ConnectCommand : CommandBase {
        public ConnectCommand(MainViewModel mvm) : base(mvm) {
        }

        #region ICommand Member
        public override bool CanExecute(object parameter) {
            return mainViewModel.ApiInstance != null && mainViewModel.ApiInstance.IsConnected == false;
        }

        public override void Execute(object parameter) {
            mainViewModel.ConnectToHost();
        }
        #endregion
    }
}
