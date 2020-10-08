using System;
using System.Collections.Generic;
using System.Text;
using WpfClient.Utils;

namespace WpfClient.ViewModels
{
    class GameViewModel : ObservableObject
    {
        private readonly MainViewModel mainViewModel;
        public GameViewModel(MainViewModel mainViewModel, string username)
        {
            this.mainViewModel = mainViewModel;
        }
    }
}
