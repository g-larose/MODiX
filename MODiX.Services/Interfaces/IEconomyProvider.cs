using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODiX.Services.Interfaces
{
    public interface IEconomyProvider
    {
        int GetDaily();
        int GetWork();
        int GetCommunity();
        int GetHobby();
        int GetChores();

    }
}
