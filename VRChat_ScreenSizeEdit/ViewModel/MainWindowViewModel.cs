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
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool EnableWindow(IntPtr hWnd, bool bEnable);

        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "FindWindow")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);
        public static readonly int SWP_NOMOVE = 0x0002;         //창을 이동시키지 않는 옵션의 SetWindowPos의 추가 명령어입니다.

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref Rectangle rectangle);

        public int XAxis { get; set; }
        public int YAxis { get; set; }
        public bool IsTimerOn { get => Timer.IsEnabled; set => _ = value; }
        public int TimerCounted { get; set; }

        private DispatcherTimer Timer = new DispatcherTimer();
        private IntPtr VRChat_Handle;
        

        public MainWindowViewModel()
        {
            XAxis = 1832;
            YAxis = 1950;
            Timer.Interval = TimeSpan.FromMilliseconds(700);
            Timer.Tick += new EventHandler(Timer_Tick);          //이벤트 추가
            VRChat_Handle = FindWindow(null, "vrchat");
        }

        public ICommand Change_Button { get => new RelayCommand(Change_Button_Commnad); }
        private void Change_Button_Commnad()
        {
            try
            {
                IntPtr windowHandle = VRChat_Handle;
                EnableWindow(windowHandle, true);

                int Width = XAxis;
                int Height = YAxis;
                // no move window posision
                //SetWindowPos(windowHandle, IntPtr.Zero, 0, 0, Width, Height, SWP_NOMOVE);
                // move window posision to 0, 0
                SetWindowPos(windowHandle, IntPtr.Zero, 0, 0, Width, Height, 0);
            }
            catch (FormatException)
            {
                System.Windows.MessageBox.Show("크기와 위치변경을 하려는 창을 클릭 해 주세요!");
            }
        }

        public ICommand Timer_Button { get => new RelayCommand(Timer_Button_Commnad); }
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

        private void Timer_Tick(object sender, EventArgs e)
        {
            TimerCounted = TimerCounted + 1;
            var rect = new Rectangle();
            GetWindowRect(VRChat_Handle, ref rect);
            if (rect.Width - rect.Left != XAxis || rect.Height - rect.Top != YAxis)
            {
                //Change_Button_Commnad();
                SetWindowPos(VRChat_Handle, IntPtr.Zero, 0, 0, XAxis, YAxis, 0);
            }
        }
    }
}
