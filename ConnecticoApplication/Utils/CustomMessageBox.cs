using ConnecticoApplication.Windows;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Effects;

namespace ConnecticoApplication.Utils
{
    public static class CustomMessageBox
    {
        public static void ShowSuccess(Window owner, string title)
        {
            ShowMessageBox(owner, title, null, "OK", PackIconKind.Tick);
        }

        public static void ShowWarning(Window owner, string title, string description)
        {
            ShowMessageBox(owner, title, description, "OK", PackIconKind.Warning);
        }

        public static void ShowMessageBox(Window owner, string title, string description, string buttonText, PackIconKind iconKind)
        {
            owner.Effect = new BlurEffect();
            owner.Opacity = 0.5;

            var messageBox = new MessageBoxWindow();

            messageBox.TitleLabel.Content = title;

            if (!string.IsNullOrEmpty(description))
                messageBox.DescriptionLabel.Content = description;
            else
                messageBox.DescriptionLabel.Visibility = Visibility.Collapsed;

            messageBox.Icon.Kind = iconKind;
            messageBox.ConfirmButton.Content = buttonText;

            messageBox.Owner = owner;
            messageBox.ShowDialog();

            owner.Effect = null;
            owner.Opacity = 1;
        }
    }
}
