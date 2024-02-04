using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODiX.Data.Models
{
    public class TimerModel
    {
        public int Seconds { get; set; }
        public int Minutes { get; set; }
        public int Hours { get; set; }
        public int Days { get; set; }
        public int Weeks { get; set; }
        public int Months { get; set; }
        public int Years { get; set; }

        public TimerModel(int seconds, int minutes, int hours, int days, int weeks, int months, int years)
        {
            Seconds = seconds;
            Minutes = minutes;
            Hours = hours;
            Days = days;
            Weeks = weeks;
            Months = months;
            Years = years;
        }
    }
}
