using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using WpfClient.Utils;

namespace WpfClient.ViewModels
{
    class LoginViewModel : ObservableObject
    {
        private MainViewModel mainViewModel;
        public string Username { get; set; }
        public string RoomName { get; set; }
        public LoginViewModel(MainViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
            this.RoomName = "random";
            this.JoinCommand = new RelayCommand(() =>
            {
                if (RoomName == "random" || RoomName == "")
                {
                    this.mainViewModel.SelectedModel = new GameViewModel(Username);
                }
                else
                {
                    this.mainViewModel.SelectedModel = new RoomViewModel(Username, RoomName);
                }
                Debug.WriteLine("joinCommand");
                Debug.WriteLine(this.mainViewModel.SelectedModel);
            });
        }

        public ICommand JoinCommand { get; set; }
    }
}
