using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmonicAirlines.Models.Views
{
    public class TrackingView
    {
        public string LoginDate { get; set; }
        public string LoginTime { get; set; }
        public string LogoutTime { get; set; }
        public string TimeInSystem { get; set; }
        public string LogoutReason { get; set; }
        public string RowColor { get; set; }
        public string TextColor { get; set; }

        public static int CrashNums { get; set; } = 0;

        /// <summary>
        /// Возвращает цвет строки в зависимости от наличия время выхода из приложения
        /// </summary>
        public static string GetRowColor(string logoutTime)
        {
            if (logoutTime == "01.01.0001 0:00:00") return "#FF0000";
            return "#FFFFFF";
        }

        /// <summary>
        /// Возвращает цвет текста в зависимости от наличия время выхода из приложения
        /// </summary>
        public static string GetTextColor(string rowColor)
        {
            if (rowColor == "#FF0000") { CrashNums++; return "#FFFFFF"; }
            return "#000000";
        }

        /// <summary>
        /// Возвращает дату в нужном формате
        /// </summary>
        public static string GetDateTimeByFormatOrEmpty(DateTime? dateTime, string format)
        {
            if (dateTime == null) return "**:**";
            return dateTime.GetValueOrDefault().ToString(format);
        }

        /// <summary>
        /// Возвращает время в нужном формате
        /// </summary>
        public static string GetDateTimeByFormatOrEmpty(TimeSpan? timeSpan, string format)
        {
            if (timeSpan == null) return "**:**";
            return timeSpan.GetValueOrDefault().ToString(format);
        }
    }
}
