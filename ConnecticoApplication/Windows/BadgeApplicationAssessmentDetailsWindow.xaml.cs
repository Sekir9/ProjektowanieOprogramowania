using ConnecticoApplication.Models.BadgeApplicationDetailsWindow;
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
using System.Windows.Shapes;
using static ConnecticoApplication.Models.BadgeApplicationDetailsWindow.BadgeApplicationStats;

namespace ConnecticoApplication.Windows
{
    public partial class BadgeApplicationAssessmentDetailsWindow : Window
    {
        public BadgeApplicationsAssessmentViewModel.DialogResult Result { get; set; } = BadgeApplicationsAssessmentViewModel.DialogResult.Canceled;

        public string Description { get { return DescriptionTextBox.Text; } }

        public BadgeApplicationAssessmentDetailsWindow(BadgeApplicationDetailsWindowPayload payload)
        {
            InitializeComponent();
            TuristNameLabel.Content = "Turysta: " + payload.TuristName;
            BadgeLabel.Content = "Wniosek o: " + payload.ApplicationFor;
            OwnedBadgesLabel.Content = payload.TuristHasBadges;

            ToursDataGrid.DataContext = payload.Tours;
            DescriptionTextBox.Text = payload.Description;

            WholeStatsLabel.Content = $"Suma punktów: {payload.Stats.PointsSum} Ilość wycieczek: {payload.Stats.TourCount} " +
                $"Trasy niestandardowe: {payload.Stats.CustomRouteCount} Średnia ilość punktów w roku: {payload.Stats.AveragePointsInYear}";

            foreach(YearStats stats in payload.Stats.YearStatistics)
            {
                Label label = new Label();
                label.FontSize = 14;
                label.Margin = new Thickness() { Left = 40 };
                label.Content = $"Ilość punktów: {stats.PointSum} Ilość wycieczek: {stats.TourCount} " +
                $"Trasy niestandardowe: {stats.CustomRouteCount} Średnia ilość punktów na wycieczkę: {stats.AveragePointsPerTour}";

                Expander expander = new Expander();
                expander.Background = Brushes.White;
                expander.Header = stats.Year;
                expander.Content = label;
                expander.IsExpanded = true;

                StatisticsStackPanel.Children.Add(expander);
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            Result = BadgeApplicationsAssessmentViewModel.DialogResult.Accept;
            Close();
        }

        private void RejectButton_Click(object sender, RoutedEventArgs e)
        {
            Result = BadgeApplicationsAssessmentViewModel.DialogResult.Reject;
            Close();
        }

        private void KeepInProgressButton_Click(object sender, RoutedEventArgs e)
        {
            Result = BadgeApplicationsAssessmentViewModel.DialogResult.KeppInProgress;
            Close();
        }
    }
}
