using System;
using System.Collections.Generic;
using System.Text;
using WpfClient.Utils;

namespace WpfClient.ViewModels
{
    class RoomViewModel : ObservableObject
    {
        private readonly MainViewModel mainViewModel;

        public string Roomname { get; }

        public RoomViewModel(MainViewModel mainViewModel, string Username, string RoomName)
        {
            this.mainViewModel = mainViewModel;
            this.Roomname = Roomname;
        }
    }
}
