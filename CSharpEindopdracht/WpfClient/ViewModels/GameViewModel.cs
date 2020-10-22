using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using WpfClient.Models;
using WpfClient.Utils;

namespace WpfClient.ViewModels
{
    class GameViewModel : ObservableObject
    {
        private readonly MainViewModel mainViewModel;

        public ObservableCollection<Player> Players { get; set; }
        public ObservableCollection<string> Chat { get; set; }

        public GameViewModel(MainViewModel mainViewModel, string username)
        {
            this.mainViewModel = mainViewModel;
        }
    }
}
