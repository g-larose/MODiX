using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODiX.Services.Interfaces
{
    public interface ISystemTimer
    {
        void Start();
        void Stop();
        void Reset();
        int Interval { get; set; }
        bool IsRunning { get; set; }
    }
}
