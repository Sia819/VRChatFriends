using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace VRChatFriends.Common
{
    static class Startup
    {
        static public void Run()
        {
            if (File.Exists("Properties.xml"))
            {

            }
            else
            {
                Global.properties = new Properties();
            }
        }
    }
}
