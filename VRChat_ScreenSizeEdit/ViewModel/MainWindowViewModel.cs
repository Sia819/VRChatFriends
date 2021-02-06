using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

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

        public int XAxis { get; set; }
        public int YAxis { get; set; }



        public MainWindowViewModel()
        {
            XAxis = 1832;
            YAxis = 1950;
        }

        public ICommand Change_Button { get => new RelayCommand(Change_Button_Commnad); }
        private void Change_Button_Commnad()
        {
            try
            {
                IntPtr windowHandle = FindWindow(null, "vrchat");
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
    }
}
