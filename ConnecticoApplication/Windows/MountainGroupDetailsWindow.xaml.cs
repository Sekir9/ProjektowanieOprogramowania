using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ConnecticoApplication.Windows
{
    /// <summary>
    /// Interaction logic for MountainGroupDetailsWindow.xaml
    /// </summary>
    public partial class MountainGroupDetailsWindow : Window
    {
        public string GroupName { get { return NameTextBox.Text; } }
        public string GroupAbbreviation { get { return AbbreviationTextBox.Text; } }

        public MountainGroupDetailsWindow(string name, string abbreviation)
        {
            InitializeComponent();
            NameTextBox.Text = name;
            AbbreviationTextBox.Text = abbreviation;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_EnterData(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
