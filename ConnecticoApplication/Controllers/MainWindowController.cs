using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnecticoApplication.Controler
{
    public class MainWindowController
    {
        private readonly MainWindow form;
        public DataGridTestView synchronizationPage;
        public EmptyView synchronizationConfiguration;

        public MainWindowController(MainWindow form)
        {
            this.form = form;
            synchronizationPage = new DataGridTestView();
            synchronizationConfiguration = new EmptyView();
        }

        public void ResizeMainFrame()
        {
            form.GridMain.Width = form.GridMain.Width + 190;
        }

        public void ListViewSelected()
        {
            int index = form.ListViewMenu.SelectedIndex;

            switch (index)
            {
                case 0:
                    form.GridMain.Content = synchronizationPage; 
                    break;
                case 1:
                    form.GridMain.Content = synchronizationConfiguration;
                    break;           
                default:
                    form.GridMain.Content = synchronizationPage;
                    break;
            }
        }
    }
}
