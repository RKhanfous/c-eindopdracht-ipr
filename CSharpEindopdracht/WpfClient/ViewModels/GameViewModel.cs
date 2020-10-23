using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
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
        public Border CanvasBorder { get; set; }

        public GameViewModel(MainViewModel mainViewModel, string username)
        {
            this.mainViewModel = mainViewModel;

            this.Lines = new ObservableCollection<Line>();

            this.MouseDownCommand = new RelayCommand<MouseButtonEventArgs>((param) =>
            {
                if (Mouse.LeftButton == MouseButtonState.Pressed)
                    if (CanvasBorder != null)
                        lastPoint = Mouse.GetPosition(CanvasBorder);
                    else
                        Debug.WriteLine("canvasborder null");
            });

            this.MouseMoveCommand = new RelayCommand<MouseEventArgs>((param) =>
            {
                if (true
                    && Mouse.LeftButton == MouseButtonState.Pressed
                    && IsDistanceGreater(lastPoint, Mouse.GetPosition(CanvasBorder), 1))
                {
                    Line line = new Line();

                    line.Stroke = SystemColors.WindowFrameBrush;
                    line.X1 = lastPoint.X;
                    line.Y1 = lastPoint.Y;

                    lastPoint = Mouse.GetPosition(CanvasBorder);

                    line.X2 = lastPoint.X;
                    line.Y2 = lastPoint.Y;

                    line.Visibility = Visibility.Visible;

                    line.Stroke = System.Windows.Media.Brushes.Black;


                    Lines.Add(line);
                }
            });
        }


        #region helpers

        private static bool IsDistanceGreater(Point point1, Point point2, double distance)
        {
            Vector difference = (point1 - point2);
            return (distance * distance) > ((difference.X * difference.X) + (difference.Y * difference.Y));
        }

        #endregion
    }
}
