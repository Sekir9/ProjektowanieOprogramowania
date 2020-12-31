using ConnecticoApplication.Controler;
using ConnecticoApplication.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ConnecticoApplication
{
   
    public partial class LoginWindow : Window
    {
        LoginWindowController controllerLoginPage; 

        public LoginWindow()
        {
            InitializeComponent();
            controllerLoginPage = new LoginWindowController(this, new UserService());
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }


        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            controllerLoginPage.CheckUserLoginDataAsync(username_box.Text,password_box.Password);
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            controllerLoginPage.RefreshLoading();
        }
    }
}
