using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnecticoApplication.Controler
{
    class Controller
    {
        private readonly MainWindow form;
        public SynchronizationPage synchronizationPage;
        public SynchronizationConfiguration synchronizationConfiguration;

        public Controller(MainWindow form)
        {
            this.form = form;
            synchronizationPage = new SynchronizationPage();
            synchronizationConfiguration = new SynchronizationConfiguration();
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
                   // form.Page_Label.Text = "Home Page";
                    break;
                case 1:
                    form.GridMain.Content = synchronizationConfiguration;
                    // form.Page_Label.Text = "Simulation Page";
                    break;           
                default:
                    form.GridMain.Content = synchronizationPage;
                    break;
            }
        }
    }
}
