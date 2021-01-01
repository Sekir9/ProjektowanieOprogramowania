using ConnecticoApplication.Models;
using ConnecticoApplication.Services;
using ConnecticoApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ConnecticoApplication.Views
{
    public partial class MountainGroupsView : Page
    {
        public MountainGroupsViewModel viewModel { get; private set; }

        public MountainGroupsView(Frame container)
        {
            InitializeComponent();
            viewModel = new MountainGroupsViewModel(container, this);
        }

        private void EditMountainGroup_Click(object sender, RoutedEventArgs e)
        {
            int mountainGroupId = (int) ((Button)sender).Tag;
            viewModel.OnEditMountainGroupClicked(mountainGroupId);
        }

        private void FiltersChanged(object sender, TextChangedEventArgs e)
        {
            viewModel.OnFiltersChanged();
        }

        private void CreateMountainGroup_Click(object sender, RoutedEventArgs e)
        {
            viewModel.OnCreateMountainGroupClicked();
        }
    }
}
