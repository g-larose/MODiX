using Humanizer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Humanizer.Localisation;
using MODiX.Data.Models;

namespace MODiX.Services.Services
{
    public class BotTimerService
    {
        private static Stopwatch _timer = new Stopwatch();
        private static string? StartDate { get; set; }
        private static string? StartTime { get; set; }

        public BotTimerService()
        {
            _timer.Start();
            StartDate = DateTime.Now.ToShortDateString();
            StartTime = DateTime.Now.ToString("hh:mm tt");
        }

        public static string GetBotUptime()
        {
            using var process = Process.GetCurrentProcess();
            var seconds = _timer.Elapsed.Seconds;
            var minutes = _timer.Elapsed.Minutes;
            var hours = _timer.Elapsed.Hours;
            var days = _timer.Elapsed.Days;
            var weeks = (days % 365) / 7;
            var months = (days % 365) / 12;
            var years = (days / 365);
            days -= ((years * 365) + (weeks * 7));

            var uptime = new TimerModel(seconds, minutes, hours, days, weeks, months, years);
            var uptime1 = "";
            try
            {
                var newTime = StartTime.Replace(":", ".").Replace("AM", string.Empty).Replace("PM", string.Empty).Trim();
                var uptimeDate = double.Parse(newTime);
                uptime1 = $"{DateTimeOffset.UtcNow.Subtract(process.StartTime).Humanize(2, minUnit: TimeUnit.Minute, maxUnit: TimeUnit.Year)}";
            }
            catch (Exception ex)
            {
                var test = ex.Message;
            }
            return uptime1;
        }

        public static string GetStartDate()
        {
            return StartDate!;
        }

        public static string GetStartTime()
        {
            return StartTime!;
        }

    }
}
