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
    /// Логика взаимодействия для EditScheduleWindow.xaml
    /// </summary>
    public partial class EditScheduleWindow : Window
    {
        private AmonicdbContext _context;
        private Window parentWindow;
        public ScheduleView schedule;

        public EditScheduleWindow(Window parentWindow)
        {
            InitializeComponent();
            this.parentWindow = parentWindow;
        }

        /// <summary>
        /// Переход назад к окну менеджера рейсов
        /// </summary>
        private void Window_Closed(object sender, EventArgs e)
        {
            (parentWindow as ManageFlightsWindow).writeDataGridSchedules();
            parentWindow.Show();
        }

        /// <summary>
        /// Загрузка окна и информации о изменяемом рейсе
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _context = new AmonicdbContext();
            loadForm();
        }

        /// <summary>
        /// Загрузка информации о изменяемом рейсе
        /// </summary>
        private void loadForm()
        {
            tbFrom.Text = $"From: {schedule.From.ToUpper()}";
            tbTo.Text = $"To: {schedule.To.ToUpper()}";
            tbAircraft.Text = $"Aircraft: {schedule.AircraftName}";
            dtpDate.SelectedDate = DateTime.Parse(schedule.Date);
            tbTime.Text = schedule.Time;
            tbPrice.Text = schedule.EconomyPrice.Trim('$');
        }

        /// <summary>
        /// Переход назад к окну менеджера рейсов
        /// </summary>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Изменение информации о рейсе и запись в бд
        /// </summary>
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                validForm();
                Schedule updateSchedule = _context.Schedules.Where(s => s.Id == schedule.Id).FirstOrDefault();
                updateSchedule.Date = dtpDate.SelectedDate.GetValueOrDefault();
                updateSchedule.Time = TimeSpan.Parse(tbTime.Text);
                updateSchedule.EconomyPrice = Convert.ToDouble(tbPrice.Text);
                _context.Schedules.Update(updateSchedule);
                _context.SaveChanges();
                MessageBox.Show("Schedule changes confirmed", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Валидация формы
        /// </summary>
        private void validForm()
        {
            if (dtpDate.SelectedDate == null)
                throw new Exception("Select date");
        }
    }
}
