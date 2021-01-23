using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VRChatFriends.Common
{
    [Serializable]
    public class Properties
    {
        public bool IsAutoLogin { get => Lastlogin_ID != "" && Lastlogin_Password != ""; }

        private string _lastlogin_ID;
        private string _lastlogin_Password;

        // will not return null
        public string Lastlogin_ID
        { get { return (_lastlogin_ID) ?? ""; } set { _lastlogin_ID = value; } }
        public string Lastlogin_Password
        { get { return (_lastlogin_Password) ?? ""; } set { _lastlogin_Password = value; } }

    }
}
