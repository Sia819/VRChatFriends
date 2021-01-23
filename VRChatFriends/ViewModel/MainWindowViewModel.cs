using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VRChatFriends.ViewModel
{
    public class MainWindowViewModel
    {
        public Uri PageNavigation { get; set; }

        public MainWindowViewModel()
        {
            VRChatFriends.Common.Startup.Run();
            if (Global.properties.IsAutoLogin)
            {
                // TODO : goto friendList
            }
            else
            {
                PageNavigation = new Uri("pack://application:,,,/View/LoginPage.xaml");
            }
        }
    }
}
