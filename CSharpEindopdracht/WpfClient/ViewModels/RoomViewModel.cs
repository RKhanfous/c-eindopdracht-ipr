using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using WpfClient.Utils;

namespace WpfClient.ViewModels
{
    class RoomViewModel : ObservableObject
    {
        private readonly MainViewModel mainViewModel;
        public ObservableCollection<string> Players { get; set; }

        public string Roomname { get; set; } = "Roomcode";

        public RoomViewModel(MainViewModel mainViewModel, string Username, string RoomName)
        {
            this.mainViewModel = mainViewModel;
            this.Roomname = RoomName;
            //this.Players = new ObservableCollection<string> { "me", "myself", "and", "I" };
        }
    }
}
