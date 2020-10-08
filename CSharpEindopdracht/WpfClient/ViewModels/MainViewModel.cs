using System;
using System.Collections.Generic;
using System.Text;
using WpfClient.Utils;

namespace WpfClient.ViewModels
{

    class MainViewModel : ObservableObject
    {

        public ObservableObject SelectedModel { get; set; }

        public MainViewModel()
        {
            SelectedModel = new LoginViewModel(this);
        }
    }
}
