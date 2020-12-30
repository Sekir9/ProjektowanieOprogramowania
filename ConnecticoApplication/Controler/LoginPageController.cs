using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;

namespace ConnecticoApplication.Controler
{
    class LoginPageController
    {
        Login loginPage;
        public LoginPageController(Login loginpage)
        {
            loginPage = loginpage;
        }

        public void LoginDataAccepted()
        {
            loginPage.loading_icon.Visibility = System.Windows.Visibility.Collapsed;
            loginPage.status_icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.CheckCircle;
            loginPage.status_icon.Foreground = new SolidColorBrush(Color.FromArgb(255, (byte)48, (byte)171, (byte)91));
            loginPage.status_icon.Visibility = System.Windows.Visibility.Visible;
            loginPage.status_block.Text = "Logged";
            DelayAction(2000, xd);
        }

        public void RefreshLoading()
        {
            loginPage.loading_icon.Visibility = System.Windows.Visibility.Visible;
            loginPage.status_icon.Visibility = System.Windows.Visibility.Collapsed;
            loginPage.status_block.Text = "Loading..";
        }

        public void xd()
        {

            MainWindow win = new MainWindow();
            win.Show();
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
            loginPage.loading_icon.Visibility = System.Windows.Visibility.Collapsed;
            loginPage.status_icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.CloseCircle;
            loginPage.status_icon.Foreground = new SolidColorBrush(Colors.Red);
            loginPage.status_icon.Visibility = System.Windows.Visibility.Visible;
            loginPage.status_block.Text = "Your username or password is incorrect";
        }

        public void CheckUserLoginData(string username, string password)
        {

            if ("admin" == username && "admin" == password)
            {

                DelayAction(1000, LoginDataAccepted);
            }
            else DelayAction(1000, LoginDataNotAccepted);

            /*DataSet ds = new DataSet();
            SqlConnection cs = new SqlConnection("Data Source=NEPTUN; Initial Catalog=LogIn; Integrated Security=TRUE");
            cs.Open();
            SqlDataAdapter da = new SqlDataAdapter();

            string templogin = "";
            string temppassword = "";

            using (SqlCommand command = new SqlCommand("SELECT * FROM UsersLogins", cs))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            
                            switch (i)
                            {
                                case 0:
                                    templogin = reader.GetValue(i).ToString();
                                    Console.WriteLine(templogin);
                                    break;
                                case 1:
                                    temppassword = reader.GetValue(i).ToString();
                                    Console.WriteLine(temppassword);
                                    break;
                                default:
                                    break;
                            }
                        }
                        if (templogin == username && temppassword == password)
                        {

                            DelayAction(1000, LoginDataAccepted);
                            break;
                        }
                        else DelayAction(1000, LoginDataNotAccepted);
                    }
                }
            }*/
        }
    }
}
