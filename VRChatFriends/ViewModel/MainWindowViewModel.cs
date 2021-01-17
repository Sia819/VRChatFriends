using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VRChatFriends.ViewModel
{
    public class MainWindowViewModel
    {
        public string FrameSource { get; set; }

        public MainWindowViewModel()
        {
            VRChatFriends.Common.Startup.Run();
            
        }

    }
}
