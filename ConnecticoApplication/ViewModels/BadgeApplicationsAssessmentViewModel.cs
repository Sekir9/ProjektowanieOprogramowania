using ConnecticoApplication.Models;
using ConnecticoApplication.Services;
using ConnecticoApplication.Views;
using ConnecticoApplication.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Effects;

namespace ConnecticoApplication.ViewModels
{
    public class BadgeApplicationsAssessmentViewModel
    {
        private const string ALL_DESCRIPTION = "Wszystkie";


        private BadgeApplicationsAssessmentView _view;
        private Frame _container;

        public IBadgeApplicationService BadgeApplicationService { get; set; }

        private IEnumerable<BadgeApplication> _badgeApplications;


        public static (string Description, BadgeType Type)[] descriptionToBadgeType = new (string Description, BadgeType Type)[]
        {
            ("W góry", BadgeType.IntoMountains),
            ("Popularna", BadgeType.Popular),
            ("Mała", BadgeType.Small),
            ("Duża", BadgeType.Big),
            ("Za wytrwałość", BadgeType.ForPersistance)
        };

        public BadgeApplicationsAssessmentViewModel(Frame container, BadgeApplicationsAssessmentView view)
        {
            _view = view;
            _container = container;

            _view.BadgeKindComboBox.ItemsSource = descriptionToBadgeType.Select(t => t.Description).Prepend(ALL_DESCRIPTION).ToArray();
        }

        public async void Activate()
        {
            _container.Content = _view;

            _view.FirstnameTextBox.Text = "";
            _view.LastnameTextBox.Text = "";
            _view.BadgeKindComboBox.SelectedIndex = 0;

            await ReadBadgeApplicationsAsync();
        }

        private async Task ReadBadgeApplicationsAsync()
        {
            _badgeApplications = await BadgeApplicationService.GetBadgeApplication();
            _view.DataGrid.DataContext = _badgeApplications;
        }

        public void OnAssessMountainGroupClicked(int id)
        {
            BadgeApplicationAssessmentDetailsWindow detailsWindow = new BadgeApplicationAssessmentDetailsWindow();
            detailsWindow.Owner = MainWindow.Instance;

            MainWindow.Instance.Opacity = 0.5;
            MainWindow.Instance.Effect = new BlurEffect();

            bool? result = detailsWindow.ShowDialog();

            MainWindow.Instance.Effect = null;
            MainWindow.Instance.Opacity = 1;
        }

        public void OnFiltersChanged()
        {
            if (_badgeApplications == null)
                return;

            var badgeApplications = _badgeApplications;

            if (!string.IsNullOrEmpty(_view.FirstnameTextBox.Text))
            {
                badgeApplications = badgeApplications.Where(p => p.Turist.FirstName.IndexOf(_view.FirstnameTextBox.Text, StringComparison.CurrentCultureIgnoreCase) >= 0);
            }

            if (!string.IsNullOrEmpty(_view.LastnameTextBox.Text))
            {
                badgeApplications = badgeApplications.Where(p => p.Turist.SureName.IndexOf(_view.LastnameTextBox.Text, StringComparison.CurrentCultureIgnoreCase) >= 0);
            }

            if (!ALL_DESCRIPTION.Equals(_view.BadgeKindComboBox.SelectedItem))
            {
                var tuple = descriptionToBadgeType.FirstOrDefault(t => t.Description.Equals(_view.BadgeKindComboBox.SelectedItem));
                if (tuple != default)
                {
                    badgeApplications = badgeApplications.Where(p => p.Rank.Badge.Type == tuple.Type);
                }
            }

            _view.DataGrid.DataContext = badgeApplications.ToArray();
        }
    }
}
