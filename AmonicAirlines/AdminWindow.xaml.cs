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
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public User user;
        private Window parentWindow;
        private AmonicdbContext _context;

        public AdminWindow(Window parentWindow)
        {
            InitializeComponent();
            this.parentWindow = parentWindow;
        }

        /// <summary>
        /// При загрузке окна заполняется таблица с пользователями
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _context = new AmonicdbContext();
            loadDataUsers("");
            loadOffices();
        }

        /// <summary>
        /// Загрузка списка офисов в comboBox
        /// </summary>
        private void loadOffices()
        {
            List<string> officesList = new List<string>() { "All offices" };
            var result = _context.Offices.Select(o => o.Title).ToList();
            officesList.AddRange(result);
            cbOffices.ItemsSource = officesList;
            cbOffices.SelectedIndex = 0;
        }

        /// <summary>
        /// Заполнение таблици пользователей по офису
        /// </summary>
        private void loadDataUsers(string officeName)
        {
            var result = (from user in _context.Users
                          join role in _context.Roles on user.RoleId equals role.Id
                          join office in _context.Offices on user.OfficeId equals office.Id
                          where office.Title.Contains(officeName)
                          select new UserView()
                          {
                              Id = user.Id,
                              Name = user.FirstName,
                              RowColor = UserView.GetRowColor(user.RoleId.ToString(), (bool)user.Active),
                              TextColor = UserView.GetTextColor(UserView.GetRowColor(user.RoleId.ToString(), (bool)user.Active)),
                              LastName = user.LastName,
                              Age = UserView.GetAge((DateTime)user.Birthdate),
                              Role = role.Title,
                              Email = user.Email,
                              Office = office.Title
                          }).ToList();

            dataGridUsers.ItemsSource = result;
            dataGridUsers.Columns[0].Visibility = Visibility.Collapsed;
            dataGridUsers.Columns[7].Visibility = Visibility.Collapsed;
            dataGridUsers.Columns[8].Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Переход к окну авторизации
        /// </summary>
        private void Window_Closed(object sender, EventArgs e)
        {
            parentWindow.Show();
        }

        /// <summary>
        /// Переход к окну добавления нового пользователя
        /// </summary>
        private void menuAddUser_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new AddUserWindow(this).Show();
        }

        /// <summary>
        /// Переход к окну авторизации
        /// </summary>
        private void menuExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Переход к окну смены роли выбранного пользователя
        /// </summary>
        private void btnChangeRole_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridUsers.SelectedItem == null) { MessageBox.Show("Select user", "Error", MessageBoxButton.OK, MessageBoxImage.Error); return; }
            var selectedUser = dataGridUsers.SelectedItem as UserView;
            if (selectedUser.Id == user.Id) { MessageBox.Show("You can't select yourself", "Error", MessageBoxButton.OK, MessageBoxImage.Error); return; }

            this.Hide();
            new EditRoleWindow(this) { user = selectedUser }.Show();
        }

        /// <summary>
        /// Блокировка/разблокировка выбранного пользователя
        /// </summary>
        private void btnSwapLogin_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridUsers.SelectedItem == null) { MessageBox.Show("Select user", "Error", MessageBoxButton.OK, MessageBoxImage.Error); return; }
            var selectedUser = dataGridUsers.SelectedItem as UserView;
            if (selectedUser.Id == user.Id) { MessageBox.Show("You can't select yourself", "Error", MessageBoxButton.OK, MessageBoxImage.Error); return; }

            string message = (bool)_context.Users.Where(u => u.Id == selectedUser.Id).FirstOrDefault().Active ?
                $"User #{selectedUser.Id}, {selectedUser.LastName} {selectedUser.Name}, was be locked.\nContinue?" :
                $"User #{selectedUser.Id}, {selectedUser.LastName} {selectedUser.Name}, was be unlocked.\nContinue?";
            if (MessageBox.Show(message, "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No) return;

            var updateUser = _context.Users.Where(u => u.Id == selectedUser.Id).FirstOrDefault();
            updateUser.Active = !updateUser.Active;
            _context.Users.Update(updateUser);
            _context.SaveChanges();

            selectOffice();
        }

        /// <summary>
        /// Событие выбора офиса в comboBox
        /// </summary>
        private void cbOffices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectOffice();
        }

        /// <summary>
        /// Если выбран определенный офис - выполняется соответствующий метод
        /// </summary>
        public void selectOffice()
        {
            try
            {
                if (cbOffices.SelectedIndex == 0)
                {
                    loadDataUsers("");
                    return;
                }

                loadDataUsers(cbOffices.SelectedItem.ToString());
            }
            catch { }
        }
    }
}
