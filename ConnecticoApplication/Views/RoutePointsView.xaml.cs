using ConnecticoApplication.Models;
using ConnecticoApplication.Services;
using ConnecticoApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ConnecticoApplication.Views
{
    public partial class RoutePointsView : Page
    {
        private readonly Regex _regexNoDigits = new Regex("[^0-9]+");
        public RoutePointsViewModel viewModel { get; private set; }

        public RoutePointsView(Frame container)
        {
            InitializeComponent();
            viewModel = new RoutePointsViewModel(container, this);
        }

        private void EditRoutePoint_Click(object sender, RoutedEventArgs e)
        {
            int routePointId = (int)((Button)sender).Tag;
            viewModel.OnEditRoutePointClicked(routePointId);
        }

        private void FiltersChanged(object sender, TextChangedEventArgs e)
        {
            viewModel?.OnFiltersChanged();
        }

        private void HeightFromChanged(object sender, TextChangedEventArgs e)
        {
            if (HeightFromTextBox == null || HeightToTextBox == null)
                return;

            if (!int.TryParse(HeightFromTextBox.Text, out int from))
                from = 0;

            if (!int.TryParse(HeightToTextBox.Text, out int to))
                to = 9999;

            if (from > to)
                HeightToTextBox.Text = from.ToString();

            FiltersChanged(sender, e);
        }

        private void HeightToChanged(object sender, TextChangedEventArgs e)
        {
            if (HeightFromTextBox == null || HeightToTextBox == null)
                return;

            if (!int.TryParse(HeightFromTextBox.Text, out int from))
                from = 0;

            if (!int.TryParse(HeightToTextBox.Text, out int to))
                to = 9999;

            if (to < from)
                HeightFromTextBox.Text = to.ToString();

            FiltersChanged(sender, e);
        }

        private void RoutePointGroup_Click(object sender, RoutedEventArgs e)
        {
            viewModel.OnCreateRoutePointClicked();
        }

        private new void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = _regexNoDigits.IsMatch(e.Text);
        }
    }
}
