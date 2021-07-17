using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Input;
using System.Windows.Threading;

namespace VRChat_ScreenSizeEdit.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region DllImports
        public static readonly int SWP_NOMOVE = 0x0002;         //창을 이동시키지 않는 옵션의 SetWindowPos의 추가 명령어입니다.

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool EnableWindow(IntPtr hWnd, bool bEnable);

        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "FindWindow")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref Rectangle rectangle);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool GetClientRect(IntPtr hwnd, ref Rectangle rectangle);

        # endregion

        #region Public Properties
        public int XAxis { get; set; }
        public int YAxis { get; set; }

        public int TimerCounted { get; set; }
        public bool IsTimerOn { get => Timer.IsEnabled; set => _ = value; }
        public bool IsExcludeSize { get; set; }
        public Common.ObservableList<Model.ScreenSettings.Resolution> Resolutions { get; }
        public int AddRes_Height { get; set; }
        public int AddRes_Width { get; set; }
        public int List_SelectedIndex { get; set; }
        #endregion

        #region Private Fields
        private int XAxis_Exclude = -1;
        private int YAxis_Exclude = -1;
        private Model.ScreenSettings settings;
        private DispatcherTimer Timer = new DispatcherTimer();
        //private IntPtr VRChat_Handle;
        //private Model.ScreenSettings settings = new Model.ScreenSettings();
        #endregion

        public MainWindowViewModel()
        {
            settings = new Model.ScreenSettings();
            Resolutions = settings.Resolutions;
            Resolutions.CollectionChanged += CollectionChangedEvent;
            if (Resolutions.Count > 0)
            {
                this.XAxis = Resolutions[0].Width;
                this.YAxis = Resolutions[0].Height;
            }
            else
            {
                this.XAxis = 1920;
                this.YAxis = 1080;
            }
            // Make Timer
            Timer.Interval = TimeSpan.FromMilliseconds(700);
            Timer.Tick += new EventHandler(Timer_Tick);          //이벤트 추가
            IsExcludeSize = true;   // 타이틀바 사이즈 제외여부 기본설정
        }



        public ICommand Change_Button { get => new RelayCommand(Change_Button_Commnad); }
        public ICommand Timer_Button { get => new RelayCommand(Timer_Button_Commnad); }
        public ICommand Add_Button { get => new RelayCommand(Add_Button_Commnad); }
        public ICommand Remove_Button { get => new RelayCommand(Remove_Button_Commnad); }
        public ICommand Up_Button { get => new RelayCommand(Up_Button_Commnad); }
        public ICommand Down_Button { get => new RelayCommand(Down_Button_Commnad); }
        public ICommand Debug_Button { get => new RelayCommand(Debug_Button_Commnad); }

        #region Command Function
        private void Change_Button_Commnad()
        {
            var rect = new Rectangle();
            IntPtr winPtr;
            GetWindowRect(winPtr = FindWindow(null, "vrchat"), ref rect);
            if (winPtr != IntPtr.Zero)
            {
                bool needChangeSize;
                if (IsExcludeSize)
                    needChangeSize = rect.Width - rect.Left != XAxis + XAxis_Exclude || rect.Height - rect.Top != YAxis + YAxis_Exclude;
                else
                    needChangeSize = rect.Width - rect.Left != XAxis || rect.Height - rect.Top != YAxis;

                // change vrchat window size
                if (needChangeSize)
                {
                    if (IsExcludeSize)
                    {
                        if (XAxis_Exclude == -1) // checking "XAxis_Exclude" is not yet decided
                        {
                            var clientSize = new Rectangle();
                            GetClientRect(FindWindow(null, "vrchat"), ref clientSize);
                            XAxis_Exclude = (rect.Width - rect.Left) - clientSize.Width;
                            YAxis_Exclude = (rect.Height - rect.Top) - clientSize.Height;
                        }
                        SetWindowPos(FindWindow(null, "vrchat"),
                                        IntPtr.Zero,
                                        0,
                                        0,
                                        XAxis + XAxis_Exclude,
                                        YAxis + YAxis_Exclude,
                                        0);
                    }
                    else
                        SetWindowPos(FindWindow(null, "vrchat"), IntPtr.Zero, 0, 0, XAxis, YAxis, 0);
                }
            }
        }
        private void Timer_Button_Commnad()
        {
            if (Timer.IsEnabled)
            {
                Timer.Stop();
                RaisePropertyChanged(nameof(IsTimerOn));

            }
            else
            {
                Timer.Start();
                RaisePropertyChanged(nameof(IsTimerOn));
            }
        }
        private void Add_Button_Commnad()
        {
            Resolutions.Add(new Model.ScreenSettings.Resolution(AddRes_Width, AddRes_Height));
            settings.SaveToFile();
        }
        private void Remove_Button_Commnad()
        {
            if (List_SelectedIndex >= 0)
            {
                int order = List_SelectedIndex;
                Resolutions.RemoveAt(order);
                settings.SaveToFile();
                if (Resolutions.Count > order)
                    List_SelectedIndex = order;
                else
                    List_SelectedIndex = order - 1;
            }
        }
        private void Up_Button_Commnad()
        {
            if (List_SelectedIndex >= 0)
                if (List_SelectedIndex > 0)
                {
                    int order = List_SelectedIndex;
                    var temp = Resolutions[order];
                    Resolutions.RemoveAt(order);
                    Resolutions.Insert(order - 1, temp);
                    settings.SaveToFile();
                    List_SelectedIndex = order - 1;
                }
        }
        private void Down_Button_Commnad()
        {
            if (List_SelectedIndex >= 0)
                if (Resolutions.Count > List_SelectedIndex + 1)
                {
                    int order = List_SelectedIndex;
                    var temp = Resolutions[order];
                    Resolutions.RemoveAt(order);
                    Resolutions.Insert(order + 1, temp);
                    settings.SaveToFile();
                    List_SelectedIndex = order + 1;
                }
        }
        private void Debug_Button_Commnad()
        {
            var rect = new Rectangle();
            GetClientRect(FindWindow(null, "vrchat"), ref rect);
            // 가로 + 16, 세로 + 39
        }


        #endregion

        #region Private Function

        private void Timer_Tick(object sender, EventArgs e)
        {
            TimerCounted = TimerCounted + 1;
            Change_Button_Commnad();
        }
        private void CollectionChangedEvent(object o, EventArgs e)
        {
            var a = o as Common.ObservableList<Model.ScreenSettings.Resolution>;
            if (a[0].Width != XAxis && YAxis != a[0].Height)
            {
                XAxis = a[0].Width;
                YAxis = a[0].Height;
            }
        }
        #endregion
    }
}
