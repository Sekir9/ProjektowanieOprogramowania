using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for RoutePointDetailsWindow.xaml
    /// </summary>
    public partial class RoutePointDetailsWindow : Window
    {
        private readonly Regex _regexNoDigits = new Regex("[^0-9]+");

        public string PointName { get { return NameTextBox.Text; } }
        public string PointDescription { get { return DescriptionTextBox.Text; } }
        public string PointCordinates { get { return CordinatesTextBox.Text; } }
        public string PointHeight { get { return HeightTextBox.Text; } }

        public RoutePointDetailsWindow(string name, string description, string cordinates, string height)
        {
            InitializeComponent();
            NameTextBox.Text = name;
            DescriptionTextBox.Text = description;
            CordinatesTextBox.Text = cordinates;
            HeightTextBox.Text = height;
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

        private new void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = _regexNoDigits.IsMatch(e.Text);
        }
    }
}
