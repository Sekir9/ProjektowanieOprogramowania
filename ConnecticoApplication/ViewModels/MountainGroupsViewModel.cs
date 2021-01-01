using ConnecticoApplication.Models;
using ConnecticoApplication.Services;
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

            _mountainGroups = await MountainGroupService.GetMountainGroups();
            _view.DataGrid.DataContext = _mountainGroups;
        }

        public async void OnCreateMountainGroupClicked()
        {
            string name = "";
            string abbreviation = "";

            bool confirmed = AskForGroupData(ref name, ref abbreviation);
            if (confirmed)
            {
                MountainGroup newMountainGroup = new MountainGroup();
                newMountainGroup.Name = name;
                newMountainGroup.Abbreviation = abbreviation; //TODO add validation

                bool success = await MountainGroupService.CreateMountainGroup(newMountainGroup);//TODO process success
                if (success)
                {
                    //TODO create own messsage box 
                }
            }

            _mountainGroups = await MountainGroupService.GetMountainGroups();
            _view.DataGrid.DataContext = _mountainGroups;
        }

        public void OnEditMountainGroupClicked(int id)
        {
            MountainGroup mountainGroup = _mountainGroups.FirstOrDefault(m => m.Id == id);
            if (mountainGroup == null)
                return;

            string name = mountainGroup.Name;
            string abbreviation = mountainGroup.Abbreviation;

            bool confirmed = AskForGroupData(ref name, ref abbreviation);
            if (confirmed)
            {

            }//TODO add validation
        }

        public bool AskForGroupData(ref string name, ref string abbreviation)
        {
            MountainGroupDetailsWindow detailsWindow = new MountainGroupDetailsWindow(name, abbreviation);
            detailsWindow.Owner = MainWindow.Instance;

            bool? result = detailsWindow.ShowDialog();
            if (result ?? false)
            {
                name = detailsWindow.GroupName;
                abbreviation = detailsWindow.GroupAbbreviation;
                return true;
            }

            return false;
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
