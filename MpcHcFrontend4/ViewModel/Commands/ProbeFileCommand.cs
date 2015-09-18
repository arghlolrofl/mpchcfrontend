using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MpcHcFrontend4.ViewModel.Commands {
    public class ProbeFileCommand : CommandBase {
        public ProbeFileCommand(MainViewModel mvm) : base(mvm) {
        }

        #region ICommand Member
        public override bool CanExecute(object parameter) {
            return true;
        }

        public override void Execute(object parameter) {
            mainViewModel.ProbeCurrentMedia();
        }
        #endregion
    }
}
