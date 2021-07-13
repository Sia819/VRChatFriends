using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using GalaSoft.MvvmLight;

namespace VRChat_ScreenSizeEdit.Model
{
    public class ScreenSettings
    {
        public ScreenSettings()
        {
            LoadFromFile();
        }

        public void LoadFromFile()
        {
            if (Resolutions is null)
            {
                Resolutions = new Common.ObservableList<Model.ScreenSettings.Resolution>();
            }
            else if (Resolutions.Count > 0)
            {
                Resolutions.Clear();
            }

            Resolutions = Common.SerializeEx.DeSerialization<Common.ObservableList<Model.ScreenSettings.Resolution>>(Environment.CurrentDirectory + "\\settings.xml");
        }
        public void SaveToFile()
        {
            if (Resolutions is null)
            {
                return;
            }
            Common.SerializeEx.Serialization(Resolutions, Environment.CurrentDirectory + "\\settings.xml");
        }

        public Common.ObservableList<Resolution> Resolutions { get; private set; }


        public struct Resolution
        {
            public Resolution(int Width, int Height)
            {
                this.Width = Width;
                this.Height = Height;
            }
            public int Width { get; set; }
            public int Height { get; set; }

        }
    }
}
