using MODiX.Services.BaseModules;
using MODiX.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODiX.Services.Features.Economy
{
    public class EconomyProviderService : IEconomyProvider, IDisposable
    {

        public void Dispose()
        {
            DisposableBase disposableBase = new();
            disposableBase.Dispose();
        }

        public int GetChores()
        {
            var rnd = new Random();
            var points = rnd.Next(100, 500);
            return points;
        }

        public int GetCommunity()
        {
            var rnd = new Random();
            var points = rnd.Next(300, 600);
            return points;
        }

        public int GetDaily()
        {
            var rnd = new Random();
            var points = rnd.Next(1000, 10000);
            return points;
        }

        public int GetHobby()
        {
            var rnd = new Random();
            var points = rnd.Next(100, 1000);
            return points;
        }

        public int GetWork()
        {
            var rnd = new Random();
            var points = rnd.Next(1000, 5000);
            return points;
        }
    }
}
