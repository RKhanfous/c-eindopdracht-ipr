using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WpfClient.Utils
{
    class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
