using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MpcHcFrontend4.ViewModel.Commands {
    class ImportSeriesFolderCommand : CommandBase {
        public ImportSeriesFolderCommand(MainViewModel mvm) : base(mvm) {
        }

        #region ICommand Member
        public override bool CanExecute(object parameter) {
            return true;
        }

        public override void Execute(object parameter) {
            mainViewModel.ImportSeriesFolder();
        }
        #endregion
    }
}
