using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace VRChat_ScreenSizeEdit.Converter
{

    public class ResolutionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //var res = (Common.ObservableList<Model.ScreenSettings.Resolution>)value;
            //List<string> result = new List<string>();
            //foreach (var i in res)
            //{
            //    result.Add(i.Width + ", " + i.Height);
            //}
            //return result;
            var res = (Model.ScreenSettings.Resolution)value;
            return res.Width + ", " + res.Height;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
