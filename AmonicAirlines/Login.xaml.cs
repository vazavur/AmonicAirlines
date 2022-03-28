using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AmonicAirlines.Models;

namespace AmonicAirlines
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private AmonicdbContext _context;

        public Login()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Выбрасывает исключение при невалидных значениях в форме
        /// </summary>
        private void validForm()
        {
            if (tbUsername.Text.Trim() == "" || pbPassword.Password.Trim() == "")
                throw new Exception("Fill in all fields");
        }

        /// <summary>
        /// Авторизация пользователя и переход на соответствующее окно
        /// </summary>
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                validForm();

                var user = _context.Users.Where(u => u.Email == tbUsername.Text && u.Password == pbPassword.Password).FirstOrDefault();
                if (user == null)
                    throw new Exception("Login and/or password invalid");

                if (!(bool)user.Active)
                    throw new Exception("Your account has been blocked");

                this.Hide();
                if(user.RoleId == 1)
                    new AdminWindow(this) { user = user }.Show();
                else if(user.RoleId == 2)
                {
                    var lastLogin = _context.Trackings.Where(t => t.UserId == user.Id).OrderByDescending(t => t.Id).FirstOrDefault();
                    if (lastLogin == null || lastLogin.LogoutDateTime.ToString() != "") new UserWindow(this) { user = user }.Show();
                    else new LogoutReasonWindow(this) { lastLogin = lastLogin }.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        /// <summary>
        /// Выход из приложения
        /// </summary>
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Событие загрузки окна
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _context = new AmonicdbContext();
        }
    }
}
