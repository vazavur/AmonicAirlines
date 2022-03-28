using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmonicAirlines.Models.Views
{
    public class ScheduleView
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string FlightNum { get; set; }
        public string AircraftName { get; set; }
        public string EconomyPrice { get; set; }
        public string BusinessPrice { get; set; }
        public string FirstPrice { get; set; }
        public string RowColor { get; set; }
        public string TextColor { get; set; }
        public bool Confirmed { get; set; }

        /// <summary>
        /// Возвращает цвет строки в зависимости от подтвержденности полёта
        /// </summary>
        public static string GetRowColor(bool confirmed)
        {
            if (!confirmed) return "#FF0000";
            return "#FFFFFF";
        }

        /// <summary>
        /// Возвращает цвет текста в зависимости от цвета строки
        /// </summary>
        public static string GetTextColor(string rowColor)
        {
            if (rowColor == "#FF0000") return "#FFFFFF";
            return "#000000";
        }
    }
}
