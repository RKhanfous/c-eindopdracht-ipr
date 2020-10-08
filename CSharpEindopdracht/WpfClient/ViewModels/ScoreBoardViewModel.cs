using System;
using System.Collections.Generic;
using System.Text;
using WpfClient.Utils;

namespace WpfClient.ViewModels
{
    class ScoreBoardViewModel : ObservableObject
    {
        private readonly MainViewModel mainViewModel;

        public ScoreBoardViewModel(MainViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
        }
    }
}
