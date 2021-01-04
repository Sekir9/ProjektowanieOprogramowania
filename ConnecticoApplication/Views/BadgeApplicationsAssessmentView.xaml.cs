using ConnecticoApplication.Models;
using ConnecticoApplication.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ConnecticoApplication.Views
{
    public partial class BadgeApplicationsAssessmentView : Page
    {
        public BadgeApplicationsAssessmentViewModel viewModel { get; private set; }

        public BadgeApplicationsAssessmentView(Frame container)
        {
            InitializeComponent();
            viewModel = new BadgeApplicationsAssessmentViewModel(container, this);
        }

        private void AssessBadgeApplication_Click(object sender, RoutedEventArgs e)
        {
            int mountainGroupId = (int)((Button)sender).Tag;
            viewModel.OnAssessMountainGroupClicked(mountainGroupId);
        }

        private void FiltersChanged(object sender, TextChangedEventArgs e)
        {
            viewModel.OnFiltersChanged();
        }

        private void FiltersChanged(object sender, SelectionChangedEventArgs e)
        {
            viewModel.OnFiltersChanged();
        }
	}

    public class BadgeTypeToDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            BadgeType type = (BadgeType)value;

            return BadgeApplicationsAssessmentViewModel.descriptionToBadgeType.Where(t => t.Type == type).Select(t => t.Description).FirstOrDefault();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string description = (string)value;
            return BadgeApplicationsAssessmentViewModel.descriptionToBadgeType.Where(t => t.Description == description).Select(t => t.Type).FirstOrDefault();
        }
    }
}
