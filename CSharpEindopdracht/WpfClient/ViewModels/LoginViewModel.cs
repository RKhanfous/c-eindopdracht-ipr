using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfClient.Utils;

namespace WpfClient.ViewModels
{
    class LoginViewModel : ObservableObject
    {
        private readonly MainViewModel mainViewModel;
        public string Username { get; set; }
        public string RoomCode { get; set; }
        public LoginViewModel(MainViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
            this.RoomCode = "";
            this.JoinCommand = new RelayCommand(async () =>
            {
                if (Username != null)
                {
                    Debug.WriteLine("awaiting client connect");
                    await this.mainViewModel.connectToServer;
                    Debug.WriteLine("client connected");
                    Debug.WriteLine("username is " + Username);
                    this.mainViewModel.Client.LogOn(Username, RoomCode);


                    //if (RoomCode == "random" || RoomCode == "")
                    //{
                    //    this.mainViewModel.SelectedViewModel = new GameViewModel(this.mainViewModel, Username);
                    //}
                    //else
                    //{
                    //    this.mainViewModel.SelectedViewModel = new RoomViewModel(this.mainViewModel, Username, RoomCode);
                    //}
                }
                else
                {
                    //TODO
                }
                Debug.WriteLine("joinCommand");
                Debug.WriteLine(this.mainViewModel.SelectedViewModel);
            });
        }

        public ICommand JoinCommand { get; set; }
    }
}
