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
    /// Логика взаимодействия для LogoutReasonWindow.xaml
    /// </summary>
    public partial class LogoutReasonWindow : Window
    {
        private AmonicdbContext _context;
        private Window parentWindow;
        public Tracking lastLogin;
        private bool reasonWrited = false;
        public LogoutReasonWindow(Window parentWindow)
        {
            InitializeComponent();
            this.parentWindow = parentWindow;
        }

        /// <summary>
        /// Переход к окну пользователя или окну авторизации
        /// </summary>
        private void Window_Closed(object sender, EventArgs e)
        {
            if (!reasonWrited)
                parentWindow.Show();
            else
                new UserWindow(parentWindow) { user = _context.Users.Where(u => u.Id == lastLogin.UserId).FirstOrDefault() }.Show();
        }

        /// <summary>
        /// Загрузка окна и вывод информации
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _context = new AmonicdbContext();
            tbInfo.Text = $"No logout detected for your last login on {lastLogin.LoginDateTime.ToString("MM.dd.yyyy")} ar {lastLogin.LoginDateTime.ToString("HH:mm")}";
        }

        /// <summary>
        /// Обновление записи в бд с добавлением причины краша
        /// </summary>
        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                validForm();

                lastLogin.LogoutReason = $"{((rbSoftware.IsChecked.GetValueOrDefault()) ? "Software crash:" : "System crash:")} " +
                    $"{new TextRange(rtbReasonMessage.Document.ContentStart, rtbReasonMessage.Document.ContentEnd).Text}";
                _context.Trackings.Update(lastLogin);
                _context.SaveChanges();

                reasonWrited = true;
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Валидация формы
        /// </summary>
        private void validForm()
        {
            if (!rbSoftware.IsChecked.GetValueOrDefault() && !rbSystem.IsChecked.GetValueOrDefault()) throw new Exception("Select reason crash");
            if (new TextRange(rtbReasonMessage.Document.ContentStart, rtbReasonMessage.Document.ContentEnd).Text == "") throw new Exception("Write reason crash");
        }
    }
}
