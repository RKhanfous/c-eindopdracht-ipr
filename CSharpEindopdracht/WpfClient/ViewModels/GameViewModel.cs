using GalaSoft.MvvmLight.Command;
using SharedSkribbl;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfClient.Models;
using WpfClient.Utils;

namespace WpfClient.ViewModels
{
    class GameViewModel : ObservableObject
    {
        private Point lastPoint;

        public MainViewModel MainViewModel { get; private set; }

        public ObservableCollection<string> Chat { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<Line> Lines { get; set; } = new ObservableCollection<Line>();

        public string Guess { get; set; }

        public ICommand MouseDownCommand { get; set; }
        public ICommand MouseMoveCommand { get; set; }
        public ICommand GuessCommand { get; set; }
        public ICommand DeleteLinesCommand { get; set; }
        public Border CanvasBorder { get; set; }

        public GameViewModel(MainViewModel mainViewModel)
        {
            this.MainViewModel = mainViewModel;

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
                if (this.MainViewModel.MePlayer.IsDrawing
                    && Mouse.LeftButton == MouseButtonState.Pressed
                    && IsDistanceGreater(lastPoint, Mouse.GetPosition(CanvasBorder), 1))
                {
                    Line line = new Line();

                    line.X1 = (short)lastPoint.X;
                    line.Y1 = (short)lastPoint.Y;

                    lastPoint = Mouse.GetPosition(CanvasBorder);

                    line.X2 = (short)lastPoint.X;
                    line.Y2 = (short)lastPoint.Y;


                    Lines.Add(line);
                }
            });

            this.GuessCommand = new RelayCommand(() =>
            {
                this.Chat.Add(Guess);
                Guess = "";
            });

            this.DeleteLinesCommand = new RelayCommand(() =>
            {
                this.Lines.Clear();
            });
        }


        #region helpers

        private static bool IsDistanceGreater(Point point1, Point point2, double distance)
        {
            Vector difference = (point1 - point2);
            return (distance * distance) < ((difference.X * difference.X) + (difference.Y * difference.Y));
            //Debug.WriteLine($"defference {difference.Length}");
            //return distance > difference.Length;
        }

        #endregion
    }
}
