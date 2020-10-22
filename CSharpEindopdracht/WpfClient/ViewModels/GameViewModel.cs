using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;
using WpfClient.Models;
using WpfClient.Utils;

namespace WpfClient.ViewModels
{
    class GameViewModel : ObservableObject
    {
        private readonly MainViewModel mainViewModel;
        private Point lastPoint;

        public ObservableCollection<Player> Players { get; set; }
        public ObservableCollection<string> Chat { get; set; }
        public ObservableCollection<Line> Lines { get; set; }

        public ICommand MouseDownCommand { get; set; }
        public ICommand MouseMoveCommand { get; set; }

        public GameViewModel(MainViewModel mainViewModel, string username)
        {
            this.mainViewModel = mainViewModel;

            this.Lines = new ObservableCollection<Line>();

            this.MouseDownCommand = new RelayCommand<MouseButtonEventArgs>((param) =>
            {
                if (Mouse.LeftButton == MouseButtonState.Pressed)
                    lastPoint = this.mainViewModel.GetRawMousePosition();
            });

            this.MouseMoveCommand = new RelayCommand<MouseEventArgs>((param) =>
            {
                if (Mouse.LeftButton == MouseButtonState.Pressed)
                {
                    Line line = new Line();

                    line.Stroke = SystemColors.WindowFrameBrush;
                    line.X1 = lastPoint.X;
                    line.Y1 = lastPoint.Y;

                    lastPoint = this.mainViewModel.GetRawMousePosition();

                    line.X2 = lastPoint.X;
                    line.Y2 = lastPoint.Y;

                    line.Visibility = Visibility.Visible;

                    line.Stroke = System.Windows.Media.Brushes.Black;


                    Lines.Add(line);
                }
            });
        }
    }
}
