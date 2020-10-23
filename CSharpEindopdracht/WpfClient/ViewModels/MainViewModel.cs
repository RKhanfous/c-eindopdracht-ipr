using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Util.MagicCode;
using WpfClient.Models;
using WpfClient.Utils;

namespace WpfClient.ViewModels
{
    class MainViewModel : ObservableObject, IClientCallback
    {
        #region private members

        private Window mWindow;

        private int mOuterMarginSize = 10;
        private int mWindowRadius = 10;

        private Client client;

        #endregion

        #region commands

        public ICommand MinimizeCommand { get; set; }

        public ICommand MaximizeCommand { get; set; }

        public ICommand CloseCommand { get; set; }

        public ICommand MenuCommand { get; set; }

        #endregion

        #region public properties

        public ObservableObject SelectedViewModel { get; set; }

        public ObservableCollection<Player> Players { get; private set; } = new ObservableCollection<Player>();

        public Player MePlayer { get; set; } = new Player() { IsDrawing = true };

        public bool Connected
        {
            get
            {
                return client.Connected();
            }
        }


        /// <summary>
        /// size of the resize border around the window
        /// </summary>

        public double MinimumWidth { get; set; } = 400;

        public double MinimumHeight { get; set; } = 400;



        public int ResizeBorder { get; set; } = 6;

        public Thickness ResizeBorderThickness { get { return new Thickness(ResizeBorder + OuterMarginSize); } }

        public Thickness InnerContentPadding { get { return new Thickness(ResizeBorder); } }


        public Thickness OuterMarginThickness { get { return new Thickness(OuterMarginSize); } }

        public CornerRadius WindowCornerRadius { get { return new CornerRadius(WindowRadius); } }

        public int OuterMarginSize
        {
            get
            {
                return mWindow.WindowState == WindowState.Maximized ? 0 : mOuterMarginSize;
            }
            set
            {
                mOuterMarginSize = value;
            }
        }

        public int WindowRadius
        {
            get
            {
                return mWindow.WindowState == WindowState.Maximized ? 0 : mWindowRadius;
            }
            set
            {
                mWindowRadius = value;
            }
        }

        public int TitleHeight { get; set; } = 42;

        public GridLength TitleHeightGridLength { get { return new GridLength(TitleHeight + ResizeBorder); } }

        #endregion

        public MainViewModel(Window window)
        {
            this.mWindow = window;

            this.mWindow.StateChanged += (sender, e) =>
            {
                OnPropertyChanged(nameof(ResizeBorderThickness));
                OnPropertyChanged(nameof(OuterMarginThickness));
                OnPropertyChanged(nameof(WindowCornerRadius));
                OnPropertyChanged(nameof(OuterMarginSize));
                OnPropertyChanged(nameof(WindowRadius));
            };


            LoginViewModel loginViewModel = new LoginViewModel(this);
            this.SelectedViewModel = loginViewModel;

            this.MinimizeCommand = new RelayCommand(() => this.mWindow.WindowState = WindowState.Minimized);
            this.MaximizeCommand = new RelayCommand(() => this.mWindow.WindowState ^= WindowState.Maximized);
            this.CloseCommand = new RelayCommand(() => this.mWindow.Close());
            this.MenuCommand = new RelayCommand(() => SystemCommands.ShowSystemMenu(this.mWindow, GetMousePosition()));

            var resizer = new WindowResizer(this.mWindow);

            //test code

            this.Players.Add(this.MePlayer);
            Random random = new Random();
            for (uint i = 1; i < 4; i++)
            {
                this.Players.Add(new Player() { Username = "bob", Id = i, Score = (uint)random.Next(100), IsDrawing = false });
            }
        }

        //check needed
        internal async Task CreateClient(string username)
        {
            if (client == null)
            {
                TcpClient tcpClient = new TcpClient();
                await tcpClient.ConnectAsync("localhost", 5555);

                this.client = new Client(tcpClient, username, this);
            }
            else
            {
                Debug.WriteLine("[mainviewmodel] i don't want to implement singleton ok? client already exists");
            }
        }


        #region helper 

        internal Point GetMousePosition()
        {
            var p = Mouse.GetPosition(this.mWindow);
            return new Point(p.X + this.mWindow.Left, p.Y + this.mWindow.Top);
        }

        internal Point GetRawMousePosition()
        {
            return Mouse.GetPosition(this.mWindow);
        }

        #endregion
    }
}
