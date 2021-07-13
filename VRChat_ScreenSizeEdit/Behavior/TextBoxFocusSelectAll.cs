using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace VRChat_ScreenSizeEdit.Behavior
{
    public class TextBoxFocusSelectAll
    {
        public static readonly DependencyProperty SelectTextOnFocusProperty = DependencyProperty
            .RegisterAttached("SelectTextOnFocus", typeof(bool), typeof(TextBoxFocusSelectAll), new FrameworkPropertyMetadata(false, GotFocus));

        public static void SetSelectTextOnFocus(DependencyObject obj, bool value)
        {
            obj.SetValue(SelectTextOnFocusProperty, value);
        }

        private static void GotFocus(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textbox = d as TextBox;

            if (null == textbox) return;
            textbox.GotKeyboardFocus += SelectTextOnFocus;
            //textbox.PreviewGotKeyboardFocus += SelectTextOnFocus;
            //textbox.GotMouseCapture += SelectTextOnFocus;
        }

        private static void SelectTextOnFocus(object sender, RoutedEventArgs e)
        {
            if (!(sender is TextBox)) return;
            ((TextBox)sender).SelectAll();
        }
    }
}
