using ConnecticoApplication.Models;
using ConnecticoApplication.Models.BadgeApplicationDetailsWindow;
using ConnecticoApplication.Services;
using ConnecticoApplication.Utils;
using ConnecticoApplication.Views;
using ConnecticoApplication.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Effects;
using static ConnecticoApplication.Models.BadgeApplicationDetailsWindow.BadgeApplicationDetailsWindowPayload;
using static ConnecticoApplication.Models.BadgeApplicationDetailsWindow.BadgeApplicationStats;

namespace ConnecticoApplication.ViewModels
{
    public class BadgeApplicationsAssessmentViewModel
    {
        public enum DialogResult
        {
            Canceled,
            KeppInProgress,
            Accept,
            Reject
        }

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

        public async void OnAssessMountainGroupClicked(int id)
        {
            bool firstTime = true;
            bool valid;
            bool success;
            DialogResult result;
            string description;

            do
            {
                do
                {
                    (result, description) = await AskForActions(id, firstTime);
                    if (result == DialogResult.Canceled)
                        return; //user resigned

                    firstTime = false;

                    valid = ValidateData(result, description);
                } while (!valid);

                BadgeApplication applicationToChange = new BadgeApplication(); //Used only to transfer ID, description and new status
                applicationToChange.Id = id;
                applicationToChange.Description = description;

                if (result == DialogResult.Accept)
                    applicationToChange.Status = VerificationStatus.Approved;
                else if (result == DialogResult.Reject)
                    applicationToChange.Status = VerificationStatus.Rejected;
                else if (result == DialogResult.KeppInProgress)
                    applicationToChange.Status = VerificationStatus.InProgress;

                success = await BadgeApplicationService.UpdateBadgeApplication(applicationToChange);
                ProcessServiceActionResponse(success);
            } while (!success);
        }

        private void ProcessServiceActionResponse(bool success)
        {
            _ = ReadBadgeApplicationsAsync(); //Continue while ui is being refreshed

            if (success)
            {
                CustomMessageBox.ShowSuccess(MainWindow.Instance, "Dane zostały pomyślnie zapisane.");
            }
            else
            {
                CustomMessageBox.ShowWarning(
                    MainWindow.Instance,
                    "Nie udało się zapisać danych!",
                    "Spróbuj ponownie!\nBłąd po stronie aplikacji lub połączenia internetowego!");
            }
        }

        private bool ValidateData(DialogResult result, string description)
        {
            if (string.IsNullOrEmpty(description) && result == DialogResult.Reject)
            {
                CustomMessageBox.ShowWarning(
                    MainWindow.Instance,
                    "Brak opisu",
                    "W przypadku odrzucenia wniosku opis musi być wypełniony");

                return false;
            }

            if (description.Length > BadgeApplication.DESCRIPTION_MAX_LENGTH)
            {
                CustomMessageBox.ShowWarning(
                    MainWindow.Instance,
                    "Za długi opis",
                    $"Maksymalna długość opisu to {BadgeApplication.DESCRIPTION_MAX_LENGTH} znaków");

                return false;
            }

            return true;
        }

        private async Task<(DialogResult Result, string Description)> AskForActions(int applicationId, bool fistTime)
        {
            BadgeApplicationDetailsWindowPayload payload = await PreparePayload(applicationId);
            if (payload == null)
                return (DialogResult.Canceled, null);

            BadgeApplicationAssessmentDetailsWindow detailsWindow = new BadgeApplicationAssessmentDetailsWindow(payload);
            detailsWindow.Owner = MainWindow.Instance;

            MainWindow.Instance.Opacity = 0.5;
            MainWindow.Instance.Effect = new BlurEffect();

            if (fistTime)
            {
                detailsWindow.Loaded += (sender, e) =>
                {
                    BadgeApplication application = _badgeApplications.FirstOrDefault(ba => ba.Id == applicationId);
                    if (application.Rank.Quota > payload.Stats.PointsSum)
                    {
                        CustomMessageBox.ShowWarning(
                            detailsWindow,
                            "Suma punktów we wniosku mniejsza niż norma odznaki!",
                            "Zwróć szczególną uwagę\nnp. czy turysta przedstawił dowód niepełnosprawności");
                    }
                };
            }

            detailsWindow.ShowDialog();

            MainWindow.Instance.Effect = null;
            MainWindow.Instance.Opacity = 1;

            return (detailsWindow.Result, detailsWindow.Description);
        }

        private async Task<BadgeApplicationDetailsWindowPayload> PreparePayload(int applicationId)
        {
            BadgeApplication application = _badgeApplications.FirstOrDefault(ba => ba.Id == applicationId);
            if (application == null)
                return null;

            BadgeApplicationDetailsWindowPayload payload = new BadgeApplicationDetailsWindowPayload();
            payload.TuristName = $"{application.Turist.FirstName} {application.Turist.SureName}";

            payload.TuristHasBadges = await GetBadgesStringForTurist(application.TuristId);

            string badgeDescription = descriptionToBadgeType.First(t => t.Type == application.Rank.Badge.Type).Description;
            payload.ApplicationFor = $"{badgeDescription} {application.Rank.Name}";

            payload.Tours = GetTourEntriesForApplication(application);

            payload.Description = application.Description;
            payload.Stats = GetStatsForApplication(application);

            return payload;
        }

        private async Task<string> GetBadgesStringForTurist(int turistId)
        {
            IEnumerable<BadgeApplication> applications = await BadgeApplicationService.GetApprovedBadgeApplicationForTurist(turistId);
            if (applications == null)
                return "Błąd pobierania danych!";

            string result = "";
            applications = applications.OrderBy(a => a.AwardDate);
            foreach (BadgeApplication application in applications)
            {
                string badgeDescription = descriptionToBadgeType.First(t => t.Type == application.Rank.Badge.Type).Description;
                string date = application.AwardDate?.ToString("MM.yyyy");
                result += $"{badgeDescription} {application.Rank.Name} ({date}), ";
            }

            if (!string.IsNullOrEmpty(result))
                result = result.Substring(0, result.Length - 2);
            else
                result = "Brak odznak";

            return result;
        }

        private IEnumerable<TourEntry> GetTourEntriesForApplication(BadgeApplication application)
        {
            List<TourEntry> entries = new List<TourEntry>();
            
            foreach (Tour tour in application.Tours)
            {
                TourEntry entry = new TourEntry();

                entry.TourStart = tour.Entries.Min(e => e.DateOfPassing);
                entry.TourEnd = tour.Entries.Max(e => e.DateOfPassing);

                string[] groups = tour.Entries.Select(e => e.Route.MountainGroup.Abbreviation).Distinct().ToArray();
                entry.MountainGroupsString = string.Join(", ", groups);

                entry.RouteCount = tour.Entries.Count;
                entry.Points = tour.Entries.Sum(e => e.Route.Points);

                entries.Add(entry);
            }

            return entries;
        }

        private BadgeApplicationStats GetStatsForApplication(BadgeApplication application)
        {
            BadgeApplicationStats stats = new BadgeApplicationStats();

            stats.PointsSum = application.Tours.SelectMany(t => t.Entries).Sum(e => e.Route.Points);
            stats.TourCount = application.Tours.Count;
            stats.CustomRouteCount = application.Tours.SelectMany(t => t.Entries).Count(e => e.Route.isCustomRoute);

            List<YearStats> yearsStats = new List<YearStats>();
            var groups = application.Tours.SelectMany(t => t.Entries).GroupBy(e => e.DateOfPassing.Year);
            foreach (var group in groups)
            {
                YearStats yearStats = new YearStats();

                yearStats.Year = group.Key;
                yearStats.TourCount = application.Tours.Count(t => t.Entries.Any(e => e.DateOfPassing.Year == yearStats.Year));
                yearStats.PointSum = group.Sum(e => e.Route.Points);
                yearStats.CustomRouteCount = group.Count(e => e.Route.isCustomRoute);

                yearsStats.Add(yearStats);
            }

            stats.YearStatistics = yearsStats;
            stats.AveragePointsInYear = yearsStats.Average(s => s.PointSum);

            return stats;
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
