using MODiX.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODiX.Services.Services
{
    public class SystemTimerProviderService : ISystemTimer
    {
        public int Interval { get; set; }
        public bool IsRunning { get; set; }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
