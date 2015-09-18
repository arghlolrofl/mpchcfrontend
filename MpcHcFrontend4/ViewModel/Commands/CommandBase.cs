using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace MpcHcFrontend4.ViewModel.Commands {
    public abstract class CommandBase : ICommand {
        public MainViewModel mainViewModel {
            get;
            set;
        }

        protected CommandBase(MainViewModel mvm) {
            mainViewModel = mvm;
        }

        public event EventHandler CanExecuteChanged {
            add {
                CommandManager.RequerySuggested += value;
            }
            remove {
                CommandManager.RequerySuggested -= value;
            }
        }

        public abstract bool CanExecute(object parameter);

        public abstract void Execute(object parameter);
    }
}
