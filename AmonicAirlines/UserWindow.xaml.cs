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
using System.Threading.Tasks;
using System.Threading;

namespace AmonicAirlines
{
    /// <summary>
    /// Логика взаимодействия для UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        public User user;
        private Window parentWindow;
        private AmonicdbContext _context;
        private DateTime loginTime;

        public UserWindow(Window parentWindow)
        {
            InitializeComponent();
            this.parentWindow = parentWindow;
        }

        /// <summary>
        /// Переход к окну авторизации
        /// </summary>
        private void Window_Closed(object sender, EventArgs e)
        {
            parentWindow.Show();
            logoutTracking();
        }

        /// <summary>
        /// Загрузка окна и общей информации
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _context = new AmonicdbContext();
            tbWelcome.Text = $"Hi {user.FirstName}, Welcome to AMONIC Airlines";

            var progress = new Progress<string>(s => tbTimeInSystem.Text = s );
            Task.Run(() => timerThread(progress));

            loadDataTracking();
            loginTracking();

            tbCrashNums.Text = $"Number of crashes: {TrackingView.CrashNums}";
        }

        /// <summary>
        /// Метод потока с таймером
        /// </summary>
        private void timerThread(IProgress<string> progress)
        {
            TimeSpan timeInSystem = new TimeSpan();
            while (true)
            {
                progress.Report($"Time in system: {timeInSystem}");
                Task.Delay(1000).Wait();
                timeInSystem += TimeSpan.FromSeconds(1);
            }
        }

        /// <summary>
        /// Создание записи о входе пользователя в запись в бд
        /// </summary>
        private void loginTracking()
        {
            loginTime = DateTime.Now;
            _context.Trackings.Add(new Tracking()
            {
                LoginDateTime = loginTime,
                UserId = user.Id
            });
            _context.SaveChanges();
        }
        
        /// <summary>
        /// Обновление записи о выходе и запись в бд
        /// </summary>
        private void logoutTracking()
        {
            Tracking tracking = _context.Trackings.Where(t => t.UserId == user.Id).OrderByDescending(t => t.Id).FirstOrDefault();
            tracking.LogoutDateTime = DateTime.Now;
            tracking.TimeInSystem = tracking.LogoutDateTime - tracking.LoginDateTime;
            _context.Trackings.Update(tracking);
            _context.SaveChanges();
        }

        /// <summary>
        /// Загрузка данных в таблицу
        /// </summary>
        private void loadDataTracking()
        {
            var result = (from tracking in _context.Trackings
                         where tracking.UserId == user.Id
                         orderby tracking.Id descending
                         select new TrackingView()
                         {
                             RowColor = TrackingView.GetRowColor(tracking.LogoutDateTime.GetValueOrDefault().ToString()),
                             TextColor = TrackingView.GetTextColor(TrackingView.GetRowColor(tracking.LogoutDateTime.GetValueOrDefault().ToString())),
                             LoginDate = tracking.LoginDateTime.ToString("MM.dd.yyyy"),
                             LoginTime = tracking.LoginDateTime.ToString("HH:mm"),
                             LogoutTime = TrackingView.GetDateTimeByFormatOrEmpty(tracking.LogoutDateTime, "HH:mm"),
                             TimeInSystem = TrackingView.GetDateTimeByFormatOrEmpty(tracking.TimeInSystem, @"mm\:ss"),
                             LogoutReason = tracking.LogoutReason
                         }).ToList();

            dataGridTracking.ItemsSource = result;
            dataGridTracking.Columns[5].Visibility = Visibility.Collapsed;
            dataGridTracking.Columns[6].Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Переход к окну авторизации
        /// </summary>
        private void menuExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void menuManageFlights_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new ManageFlightsWindow(this).Show();
        }
    }
}
