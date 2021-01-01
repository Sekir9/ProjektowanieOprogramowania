using ConnecticoApplication.Services;
using ConnecticoApplication.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace ConnecticoApplication.Controler
{
    class LoginWindowController
    {
        private LoginWindow _loginPage;
        private IUserService _userService;

        public LoginWindowController(LoginWindow loginpage, IUserService userService)
        {
            _loginPage = loginpage;
            _userService = userService;
        }

        public void LoginDataAccepted()
        {
            _loginPage.loading_icon.Visibility = Visibility.Collapsed;

            _loginPage.status_icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.CheckCircle;
            _loginPage.status_icon.Foreground = new SolidColorBrush(Color.FromArgb(255, (byte)48, (byte)171, (byte)91));
            _loginPage.status_icon.Visibility = Visibility.Visible;

            _loginPage.back_Button.Visibility = Visibility.Collapsed;
            _loginPage.status_block.Text = "Logged";
            DelayAction(2000, OpenMainWindow);
        }

        public void RefreshLoading()
        {
            _loginPage.loading_icon.Visibility = Visibility.Visible;
            _loginPage.status_icon.Visibility = Visibility.Collapsed;
            _loginPage.back_Button.Visibility = Visibility.Collapsed;
            _loginPage.status_block.Text = "Loading..";
        }

        public void OpenMainWindow()
        {
            MainWindow win = new MainWindow();
            win.Show();
            _loginPage.Close();
        }

        public static void DelayAction(int millisecond, Action action)
        {
            var timer = new DispatcherTimer();
            timer.Tick += delegate

            {
                action.Invoke();
                timer.Stop();
            };

            timer.Interval = TimeSpan.FromMilliseconds(millisecond);
            timer.Start();
        }

        public void LoginDataNotAccepted()
        {
            _loginPage.loading_icon.Visibility = Visibility.Collapsed;

            _loginPage.status_icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.CloseCircle;
            _loginPage.status_icon.Foreground = new SolidColorBrush(Colors.Red);
            _loginPage.status_icon.Visibility = Visibility.Visible;

            _loginPage.status_block.Text = "Your username or password is incorrect";
            _loginPage.back_Button.Visibility = Visibility.Visible;
        }

        public async Task CheckUserLoginDataAsync(string username, string password)
        {
            _loginPage.back_Button.Visibility = Visibility.Collapsed; //hide try again button

            bool success = await _userService.Login(username, password);
            if (success)
            {
                DelayAction(1000, LoginDataAccepted);
            }
            else
            {
                DelayAction(1000, LoginDataNotAccepted);
            }
        }
    }
}
