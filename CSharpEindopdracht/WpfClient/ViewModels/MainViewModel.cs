using GalaSoft.MvvmLight.Command;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using Util.MagicCode;
using WpfClient.Utils;
using WpfClient.ViewModels;

namespace WpfClient.ViewModels
{
    class MainViewModel : ObservableObject
    {
        #region private members

        private Window mWindow;

        private int mOuterMarginSize = 10;
        private int mWindowRadius = 10;

        #endregion

        #region commands

        public ICommand MinimizeCommand { get; set; }

        public ICommand MaximizeCommand { get; set; }

        public ICommand CloseCommand { get; set; }

        public ICommand MenuCommand { get; set; }

        #endregion

        #region public properties
        //public Info InfoModel { get; set; }

        public ObservableObject SelectedViewModel { get; set; }

        //public Client client { get; }

        /// <summary>
        /// size of the resize border around the window
        /// </summary>

        public double MinimumWidth { get; set; } = 250;

        public double MinimumHeight { get; set; } = 250;



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
        }


        #region helper 

        private Point GetMousePosition()
        {
            Debug.WriteLine("getmousePosition called");
            var p = Mouse.GetPosition(this.mWindow);
            return new Point(p.X + this.mWindow.Left, p.Y + this.mWindow.Top);
        }

        #endregion
    }
}
