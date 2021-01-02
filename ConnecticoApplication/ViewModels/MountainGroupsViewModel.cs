using ConnecticoApplication.Models;
using ConnecticoApplication.Services;
using ConnecticoApplication.Utils;
using ConnecticoApplication.Views;
using ConnecticoApplication.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Effects;

namespace ConnecticoApplication.ViewModels
{
    public class MountainGroupsViewModel
    {
        private MountainGroupsView _view;
        private Frame _container;

        public IMountainGroupService MountainGroupService { get; set; }

        private IEnumerable<MountainGroup> _mountainGroups;

        public MountainGroupsViewModel(Frame container, MountainGroupsView view)
        {
            _view = view;
            _container = container;
        }

        public async void Activate()
        {
            _container.Content = _view;

            _view.NameTextBox.Text = "";
            _view.NameTextBox.Text = "";

            await ReadMountainGroupsAsync();
        }

        private async Task ReadMountainGroupsAsync()
        {
            _mountainGroups = await MountainGroupService.GetMountainGroups();
            _view.DataGrid.DataContext = _mountainGroups;
        }

        public async void OnCreateMountainGroupClicked()
        {
            string name = "";
            string abbreviation = "";
            bool valid;

            do
            {
                bool confirmed = AskForGroupData(ref name, ref abbreviation);
                if (!confirmed)
                    return; //user resigned

                valid = ValidateData(name, abbreviation);
            } while (!valid);
            
            MountainGroup newMountainGroup = new MountainGroup();
            newMountainGroup.Name = name;
            newMountainGroup.Abbreviation = abbreviation;

            bool success = await MountainGroupService.CreateMountainGroup(newMountainGroup);
            ProcessServiceActionResponse(success);
        }

        public async void OnEditMountainGroupClicked(int id)
        {
            MountainGroup mountainGroup = _mountainGroups.FirstOrDefault(m => m.Id == id);
            if (mountainGroup == null)
                return;

            string name = mountainGroup.Name;
            string abbreviation = mountainGroup.Abbreviation;
            bool valid;

            do
            {
                bool confirmed = AskForGroupData(ref name, ref abbreviation);
                if (!confirmed)
                    return; //user resigned

                valid = ValidateData(name, abbreviation, mountainGroup);
            } while (!valid);

            mountainGroup.Name = name;
            mountainGroup.Abbreviation = abbreviation;

            bool success = await MountainGroupService.EditMountainGroup(mountainGroup);
            ProcessServiceActionResponse(success);
        }

        private void ProcessServiceActionResponse(bool success)
        {
            _ = ReadMountainGroupsAsync(); //Continue while ui is being refreshed

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

        public bool AskForGroupData(ref string name, ref string abbreviation)
        {
            MountainGroupDetailsWindow detailsWindow = new MountainGroupDetailsWindow(name, abbreviation);
            detailsWindow.Owner = MainWindow.Instance;

            MainWindow.Instance.Opacity = 0.5;
            MainWindow.Instance.Effect = new BlurEffect();

            bool? result = detailsWindow.ShowDialog();

            MainWindow.Instance.Effect = null;
            MainWindow.Instance.Opacity = 1;

            if (result ?? false)
            {
                name = detailsWindow.GroupName;
                abbreviation = detailsWindow.GroupAbbreviation;
                return true;
            }

            return false;
        }

        private bool ValidateData(string name, string abbreviation, MountainGroup exclude = null)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(abbreviation))
            {
                CustomMessageBox.ShowWarning(
                    MainWindow.Instance,
                    "Błąd danych",
                    "Nazwa oraz skrót grupy górskiej nie mogą być puste");

                return false;
            }

            var mountainGroups = _mountainGroups;
            if (exclude != null)
            {
                mountainGroups = mountainGroups.Where(m => m.Id != exclude.Id);
            }

            MountainGroup duplicate = mountainGroups.FirstOrDefault(m => m.Name == name);
            if (duplicate != null)
            {
                CustomMessageBox.ShowWarning(
                    MainWindow.Instance,
                    "Duplikat",
                    "Grupa górska z taką nazwą już istnieje!");
                return false;
            }

            return true;
        }

        public void OnFiltersChanged()
        {
            var mountainGroups = _mountainGroups;

            if (!string.IsNullOrEmpty(_view.NameTextBox.Text))
            {
                mountainGroups = mountainGroups.Where(m => m.Name.IndexOf(_view.NameTextBox.Text, StringComparison.CurrentCultureIgnoreCase) >= 0);
            }

            if (!string.IsNullOrEmpty(_view.AbbreviationTextBox.Text))
            {
                mountainGroups = mountainGroups.Where(m => m.Abbreviation.IndexOf(_view.AbbreviationTextBox.Text, StringComparison.CurrentCultureIgnoreCase) >= 0);
            }

            _view.DataGrid.DataContext = mountainGroups.ToArray();
        }
    }
}
