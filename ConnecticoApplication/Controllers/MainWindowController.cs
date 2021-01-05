using ConnecticoApplication.Models;
using ConnecticoApplication.Services;
using ConnecticoApplication.ViewModels;
using ConnecticoApplication.Views;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ConnecticoApplication.Controler
{
    public class MainWindowController
    {
        private readonly MainWindow _mainWindow;
        public DataGridTestView dataGridTestView;
        public EmptyView emptyView;

        public MountainGroupsViewModel mountainGroupsViewModel;
        public RoutePointsViewModel routePointsViewModel;
        public BadgeApplicationsAssessmentViewModel badgeApplicationsAssessmentViewModel;

        private User _loggedUser;

        public MainWindowController(MainWindow mainWindow, IUserService userService)
        {
            this._mainWindow = mainWindow;
            dataGridTestView = new DataGridTestView();
            emptyView = new EmptyView();

            mountainGroupsViewModel = new MountainGroupsView(_mainWindow.GridMain).viewModel;
            mountainGroupsViewModel.MountainGroupService = new MountainGroupService();

            routePointsViewModel = new RoutePointsView(_mainWindow.GridMain).viewModel;
            routePointsViewModel.RoutePointService = new RoutePointService();

            badgeApplicationsAssessmentViewModel = new BadgeApplicationsAssessmentView(_mainWindow.GridMain).viewModel;
            badgeApplicationsAssessmentViewModel.BadgeApplicationService = new BadgeApplicationService();

            _loggedUser = Task.Run(() => userService.GetLoggedUser()).Result;
        }

        public void ListViewSelected()
        {
            int index = _mainWindow.ListViewMenu.SelectedIndex;

            switch (index)
            {
                case 0:
                    _mainWindow.GridMain.Content = dataGridTestView; 
                    break;
                case 1:
                    _mainWindow.GridMain.Content = emptyView;
                    break;
                case 2:
                    if (_loggedUser.IsAdmin)
                        mountainGroupsViewModel.Activate();
                    else
                        _mainWindow.GridMain.Content = emptyView;
                    break;
                case 3:
                    if (_loggedUser.IsAdmin)
                        routePointsViewModel.Activate();
                    else
                        _mainWindow.GridMain.Content = emptyView;
                    break;
                case 4:
                    if (_loggedUser.isLeader)
                        badgeApplicationsAssessmentViewModel.Activate();
                    else
                        _mainWindow.GridMain.Content = emptyView;
                    break;
                default:
                    _mainWindow.GridMain.Content = dataGridTestView;
                    break;
            }
        }
    }
}
