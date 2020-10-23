using GalaSoft.MvvmLight.Command;
using System.Diagnostics;
using System.Windows.Input;
using WpfClient.Utils;

namespace WpfClient.ViewModels
{
    class RoomViewModel : ObservableObject
    {
        public MainViewModel MainViewModel { get; private set; }

        public string Roomname { get; set; } = "Roomcode not found";

        public ICommand GoToGameCommand { get; set; }

        public RoomViewModel(MainViewModel mainViewModel, string Username, string RoomName)
        {
            this.MainViewModel = mainViewModel;
            this.Roomname = RoomName;

            this.GoToGameCommand = new RelayCommand(() =>
            {
                this.MainViewModel.SelectedViewModel = new GameViewModel(this.MainViewModel, Username);
                Debug.WriteLine(this.MainViewModel.SelectedViewModel);
            });
        }
    }
}
