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

namespace AmonicAirlines
{
    /// <summary>
    /// Логика взаимодействия для AddEditUser.xaml
    /// </summary>
    public partial class AddUserWindow : Window
    {
        private Window parentWindow;
        private AmonicdbContext _context;

        public AddUserWindow(Window parentWindow)
        {
            InitializeComponent();
            this.parentWindow = parentWindow;
        }

        /// <summary>
        /// Переход к окну администратора
        /// </summary>
        private void Window_Closed(object sender, EventArgs e)
        {
            parentWindow.Show();
        }

        /// <summary>
        /// Загрузка окна и списка офисов в comboBox
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _context = new AmonicdbContext();
            loadOffices();
        }

        /// <summary>
        /// Загрузка списка офисов в comboBox
        /// </summary>
        private void loadOffices()
        {
            List<string> officesList = new List<string>() { "Office name" };
            var result = _context.Offices.Select(o => o.Title).ToList();
            officesList.AddRange(result);
            cbOffices.ItemsSource = officesList;
            cbOffices.SelectedIndex = 0;
        }

        /// <summary>
        /// Добавление нового пользователя в бд
        /// </summary>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                validForm();
                User newUser = new User()
                {
                    Id = _context.Users.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1,
                    FirstName = tbFirstName.Text,
                    LastName = tbLastName.Text,
                    Email = tbEmail.Text,
                    Password = pbPassword.Password,
                    RoleId = 2,
                    Birthdate = dtpBirthdate.SelectedDate,
                    OfficeId = _context.Offices.Where(o => o.Title == cbOffices.SelectedItem.ToString()).Select(o => o.Id).FirstOrDefault(),
                    Active = true
                };
                _context.Add(newUser);
                _context.SaveChanges();

                MessageBox.Show("User was added", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                (parentWindow as AdminWindow).selectOffice();
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Валидация формы (при некорректном вводе генерируется исключение)
        /// </summary>
        private void validForm()
        {
            if (tbEmail.Text.Trim() == "") throw new Exception("Write email");
            if(_context.Users.Where(u => u.Email == tbEmail.Text).FirstOrDefault() != null) throw new Exception("A user with this email already exists");
            if (tbFirstName.Text.Trim() == "") throw new Exception("Write first name");
            if (tbLastName.Text.Trim() == "") throw new Exception("Write last name");
            if (cbOffices.SelectedIndex == 0) throw new Exception("Select office");
            if (dtpBirthdate.SelectedDate == null) throw new Exception("Select birthdate");
            if (pbPassword.Password.Trim() == "") throw new Exception("Write password");
            if (pbPassword.Password.Length < 8) throw new Exception("Password length < 8");
        }

        /// <summary>
        /// Отмена и переход к окну администратора
        /// </summary>        
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
