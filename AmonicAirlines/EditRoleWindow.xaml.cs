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
using System.Windows.Shapes;
using AmonicAirlines.Models;
using AmonicAirlines.Models.Views;

namespace AmonicAirlines
{
    /// <summary>
    /// Логика взаимодействия для EditRoleWindow.xaml
    /// </summary>
    public partial class EditRoleWindow : Window
    {
        private Window parentWindow;
        private AmonicdbContext _context;
        public UserView user;

        public EditRoleWindow(Window parentWindow)
        {
            InitializeComponent();
            this.parentWindow = parentWindow;
        }

        /// <summary>
        /// Заполнение формы данными редактируемого пользователя
        /// </summary>
        private void loadForm()
        {
            tbEmail.Text = user.Email;
            tbFirstName.Text = user.Name;
            tbLastName.Text = user.LastName;
            tbOffice.Text = user.Office;
            rbAdmin.IsChecked = (user.Role == "Administrator") ? true : false;
            rbUser.IsChecked = (user.Role == "User") ? true : false;
        }

        /// <summary>
        /// Отмена и переход к окну администратора
        /// </summary>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Изменяет роль и записывает обновленного пользователя в бд
        /// </summary>
        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                User updateUser = _context.Users.Where(u => u.Id == user.Id).FirstOrDefault();
                updateUser.RoleId = ((bool)rbAdmin.IsChecked) ? 1 : 2;
                _context.Users.Update(updateUser);
                _context.SaveChanges();

                MessageBox.Show("User role was updated", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                (parentWindow as AdminWindow).selectOffice();
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Загрузка окна
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _context = new AmonicdbContext();
            loadForm();
        }

        /// <summary>
        /// Закрытие окна и переход к окну администратора
        /// </summary>
        private void Window_Closed(object sender, EventArgs e)
        {
            parentWindow.Show();
        }
    }
}
