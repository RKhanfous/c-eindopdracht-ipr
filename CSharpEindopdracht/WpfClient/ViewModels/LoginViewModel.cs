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
        public string RoomName { get; set; }
        public LoginViewModel(MainViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
            this.RoomName = "random";
            this.JoinCommand = new RelayCommand(async () =>
            {
                if (Username != null)
                {
                    Task createClient = this.mainViewModel.CreateClient(Username);
                    Debug.WriteLine("awaiting client connect");
                    await createClient;
                    Debug.WriteLine("client connected");

                    if (RoomName == "random" || RoomName == "")
                    {
                        this.mainViewModel.SelectedViewModel = new GameViewModel(this.mainViewModel, Username);
                    }
                    else
                    {
                        this.mainViewModel.SelectedViewModel = new RoomViewModel(this.mainViewModel, Username, RoomName);
                    }
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
