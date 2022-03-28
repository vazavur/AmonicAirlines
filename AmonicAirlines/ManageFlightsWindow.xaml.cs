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
    /// Логика взаимодействия для ManageFlightsWindow.xaml
    /// </summary>
    public partial class ManageFlightsWindow : Window
    {
        private AmonicdbContext _context;
        private Window parentWindow;

        public ManageFlightsWindow(Window parentWindow)
        {
            InitializeComponent();
            this.parentWindow = parentWindow;
        }

        /// <summary>
        /// Переход к окну пользователя
        /// </summary>        
        private void Window_Closed(object sender, EventArgs e)
        {
            parentWindow.Show();
        }

        /// <summary>
        /// Загрузка окна и списка рейсов и списка аэропортов
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _context = new AmonicdbContext();
            loadListAirports();
            writeDataGridSchedules();
        }

        /// <summary>
        /// Запись списка рейсов в таблицу
        /// </summary>
        public void writeDataGridSchedules()
        {
            dataGridSchedules.ItemsSource = null;
            dataGridSchedules.ItemsSource = loadDataSchedules();
            dataGridSchedules.Columns[0].Visibility = Visibility.Collapsed;
            dataGridSchedules.Columns[10].Visibility = Visibility.Collapsed;
            dataGridSchedules.Columns[11].Visibility = Visibility.Collapsed;
            dataGridSchedules.Columns[12].Visibility = Visibility.Collapsed;
            dataGridSchedules.Columns[5].Header = "Flight num";
            dataGridSchedules.Columns[6].Header = "Aircraft";
            dataGridSchedules.Columns[7].Header = "Economy";
            dataGridSchedules.Columns[8].Header = "Business";
            dataGridSchedules.Columns[9].Header = "First class";
            if (dataGridSchedules.Items.Count == 0) MessageBox.Show("No schedules found.\nTry change filter.", "No records found", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Возвращает список рейсов из бд
        /// </summary>
        private List<ScheduleView> loadDataSchedules()
        {
            var result = (from schedule in _context.Schedules
                         join route in _context.Routes on schedule.RouteId equals route.Id
                         join airFrom in _context.Airports on route.DepartureAirportId equals airFrom.Id
                         join airTo in _context.Airports on route.ArrivalAirportId equals airTo.Id
                         join aircraft in _context.Aircrafts on schedule.AircraftId equals aircraft.Id
                         select new ScheduleView()
                         {
                             Id = schedule.Id,
                             Date = schedule.Date.ToString("MM.dd.yyyy"),
                             Time = schedule.Time.ToString(@"hh\:mm"),
                             From = airFrom.Iatacode,
                             To = airTo.Iatacode,
                             AircraftName = aircraft.Name,
                             FlightNum = schedule.FlightNumber,
                             EconomyPrice = $"${Math.Round(schedule.EconomyPrice, 0)}",
                             BusinessPrice = $"${Math.Round(schedule.EconomyPrice * 1.35, 0)}",
                             FirstPrice = $"${Math.Round(schedule.EconomyPrice * 1.35 * 1.3, 0)}",
                             RowColor = ScheduleView.GetRowColor(schedule.Confirmed),
                             TextColor = ScheduleView.GetTextColor(ScheduleView.GetRowColor(schedule.Confirmed)),
                             Confirmed = schedule.Confirmed
                         }).ToList();

            //Выборка по фильтру
            result = result.Where(s =>
                s.From.Contains((cbFrom.SelectedIndex != 0) ? cbFrom.SelectedItem.ToString() : "") &&
                s.To.Contains((cbTo.SelectedIndex != 0) ? cbTo.SelectedItem.ToString() : "") &&
                s.Date.Contains((dtpOutbound.SelectedDate != null) ? dtpOutbound.SelectedDate.GetValueOrDefault().ToString("MM.dd.yyyy") : "") &&
                s.FlightNum.Contains(tbFlightNum.Text)).ToList();

            //Сортировка по фильтру
            if (cbSortBy.SelectedIndex == 0) result = result.OrderBy(s => s.Date).ToList();
            if (cbSortBy.SelectedIndex == 1) result = result.OrderBy(s => Convert.ToInt32(s.EconomyPrice.Trim('$'))).ToList();
            if (cbSortBy.SelectedIndex == 2) result = result.OrderBy(s => s.Confirmed).ToList();

            return result;
        }

        /// <summary>
        /// Загрузка списка аэропортов в comboBox
        /// </summary>
        private void loadListAirports()
        {
            List<string> airportsList = new List<string>() { "Any airports" };
            var result = _context.Airports.Select(a => a.Iatacode).ToList();
            airportsList.AddRange(result);

            cbFrom.ItemsSource = airportsList;
            cbTo.ItemsSource = airportsList;

            cbFrom.SelectedIndex = 0;
            cbTo.SelectedIndex = 0;
        }

        /// <summary>
        /// Отмена или подтверждение рейса (изменение и запись в бд)
        /// </summary>
        private void btnCancelFlight_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridSchedules.SelectedItem == null) { MessageBox.Show("Select schedule", "Error", MessageBoxButton.OK, MessageBoxImage.Error); return; }
            Schedule updateSchedule = _context.Schedules.Where(s => s.Id == (dataGridSchedules.SelectedItem as ScheduleView).Id).FirstOrDefault();
            updateSchedule.Confirmed = !updateSchedule.Confirmed;
            _context.Schedules.Update(updateSchedule);
            _context.SaveChanges();
            writeDataGridSchedules();
        }

        /// <summary>
        /// Переход к окну изменения информации рейса
        /// </summary>
        private void btnEditFlight_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridSchedules.SelectedItem == null) { MessageBox.Show("Select schedule", "Error", MessageBoxButton.OK, MessageBoxImage.Error); return; }
            this.Hide();
            new EditScheduleWindow(this) { schedule = dataGridSchedules.SelectedItem as ScheduleView }.Show();
        }

        /// <summary>
        /// Применить фильтры к поиску
        /// </summary>
        private void btnApplyFilter_Click(object sender, RoutedEventArgs e)
        {
            writeDataGridSchedules();
        }

        /// <summary>
        /// Событие измены выбранного элемента в таблице
        /// </summary>
        private void dataGridSchedules_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridSchedules.SelectedItem == null) return;
            btnCancelFlight.Content = (dataGridSchedules.SelectedItem as ScheduleView).Confirmed ? "Cancel Flight" : "Confirm flight";
        }
    }
}
