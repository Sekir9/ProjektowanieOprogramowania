using ConnecticoApplication.Services;
using ConnecticoApplication.ViewModels;
using ConnecticoApplication.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnecticoApplication.Controler
{
    public class MainWindowController
    {
        private readonly MainWindow _mainWindow;
        public DataGridTestView dataGridTestView;
        public EmptyView emptyView;
        public MountainGroupsViewModel mountainGroupsViewModel;

        public MainWindowController(MainWindow mainWindow)
        {
            this._mainWindow = mainWindow;
            dataGridTestView = new DataGridTestView();
            emptyView = new EmptyView();

            mountainGroupsViewModel = new MountainGroupsView(_mainWindow.GridMain).viewModel;
            mountainGroupsViewModel.MountainGroupService = new MountainGroupService();
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
                    mountainGroupsViewModel.Activate();
                    break;
                default:
                    _mainWindow.GridMain.Content = dataGridTestView;
                    break;
            }
        }
    }
}
