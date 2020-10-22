using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Input;
using WpfClient.Models;
using WpfClient.Utils;

namespace WpfClient.ViewModels
{
    class GameViewModel : ObservableObject
    {
        private readonly MainViewModel mainViewModel;
        private Point currentPoint;

        public ObservableCollection<Player> Players { get; set; }
        public ObservableCollection<string> Chat { get; set; }

        public ICommand MouseDownCommand { get; set; }
        public ICommand MouseMoveCommand { get; set; }

        public GameViewModel(MainViewModel mainViewModel, string username)
        {
            this.mainViewModel = mainViewModel;

            this.MouseDownCommand = new RelayCommand<object>((param) => { Debug.WriteLine("mouseDown " + param.GetType()); });

            this.MouseMoveCommand = new RelayCommand<object>((param) => { Debug.WriteLine("mouseMove " + param.GetType()); });
        }
    }
}
