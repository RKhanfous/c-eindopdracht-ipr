using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using WpfClient.Utils;

namespace WpfClient.ViewModels
{
    class ScoreBoardViewModel : ObservableObject
    {
        public MainViewModel MainViewModel { get; private set; }

        public ICommand GoToLoginView { get; set; }

        public ScoreBoardViewModel(MainViewModel mainViewModel)
        {
            this.MainViewModel = mainViewModel;

            this.GoToLoginView = new RelayCommand(() =>
            {
                this.MainViewModel.resetToLogin();
            });
        }
    }
}
