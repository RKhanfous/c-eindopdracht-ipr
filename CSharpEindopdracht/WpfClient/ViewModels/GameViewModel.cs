using GalaSoft.MvvmLight.Command;
using SharedSkribbl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
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



        public string Guess { get; set; }

        public ICommand MouseDownCommand { get; set; }
        public ICommand MouseMoveCommand { get; set; }
        public ICommand GuessCommand { get; set; }
        public ICommand Stroke1Command { get; set; }
        public ICommand Stroke2Command { get; set; }
        public ICommand Stroke3Command { get; set; }
        public ICommand EraserCommand { get; set; }
        public ICommand DeleteLinesCommand { get; set; }
        public Border CanvasBorder { get; set; }

        public GameViewModel(MainViewModel mainViewModel)
        {
            this.MainViewModel = mainViewModel;

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
                int stroke = -1;
                switch (this.MainViewModel.MePlayer.PenState)
                {
                    case PenState.STROKE1:
                        stroke = 1;
                        break;
                    case PenState.STROKE2:
                        stroke = 2;
                        break;
                    case PenState.STROKE3:
                        stroke = 3;
                        break;
                    case PenState.ERASOR:
                        stroke = 0;
                        break;
                }
                if (this.MainViewModel.MePlayer.IsDrawing
                    && Mouse.LeftButton == MouseButtonState.Pressed
                    && IsDistanceGreater(lastPoint, Mouse.GetPosition(CanvasBorder), stroke == 0 ? 10 : 1))
                {

                    if (stroke == -1)
                        throw new InvalidOperationException();
                    Line line = new Line();

                    line.X1 = (short)lastPoint.X;
                    line.Y1 = (short)lastPoint.Y;

                    lastPoint = Mouse.GetPosition(CanvasBorder);

                    line.X2 = (short)lastPoint.X;
                    line.Y2 = (short)lastPoint.Y;
                    if (stroke == 0)
                    {
                        ICollection<Line> toBeDeltedLines = new LinkedList<Line>();
                        Parallel.ForEach(this.MainViewModel.Lines, (otherLine) =>
                         {
                             if (line.Collide1(otherLine))
                             {
                                 lock (toBeDeltedLines)
                                 {
                                     toBeDeltedLines.Add(otherLine);
                                 }
                             }
                         });

                        //todo delelte lines
                        if (toBeDeltedLines.Count > 0)
                            Debug.WriteLine("toBeDeltedLines.Count = " + toBeDeltedLines.Count);
                    }
                    else
                    {
                        this.MainViewModel.Lines.Add(line);
                        this.MainViewModel.Client.SendMessage(SharedNetworking.Utils.DataParser.GetLineMessage(line.serialize()));
                    }
                }
            });

            this.GuessCommand = new RelayCommand(() =>
            {
                this.MainViewModel.Chat.Add(Guess);
                this.MainViewModel.Client.SendMessage(SharedNetworking.Utils.DataParser.GetGuessMessage(Guess));
                Guess = "";
            });

            this.Stroke1Command = new RelayCommand(() =>
            {
                this.MainViewModel.MePlayer.PenState = PenState.STROKE1;
            });

            this.Stroke2Command = new RelayCommand(() =>
            {
                this.MainViewModel.MePlayer.PenState = PenState.STROKE2;
            });

            this.Stroke3Command = new RelayCommand(() =>
            {
                this.MainViewModel.MePlayer.PenState = PenState.STROKE3;
            });

            this.EraserCommand = new RelayCommand(() =>
            {
                this.MainViewModel.MePlayer.PenState = PenState.ERASOR;
            });

            this.DeleteLinesCommand = new RelayCommand(() =>
            {
                this.MainViewModel.Lines.Clear();
                this.MainViewModel.Client.SendMessage(SharedNetworking.Utils.DataParser.GetClearLinesMessage());
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
