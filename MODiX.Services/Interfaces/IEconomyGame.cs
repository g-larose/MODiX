using MODiX.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODiX.Services.Interfaces
{
    public interface IEconomyGame
    {
        Result<int, string> GetDaily();
        Result<int, string> GetWork();
        Result<int, string> GetCommunity();
        Result<int, string> GetHobby();
        Result<int, SystemError> GetChores();
        Task<Result<int, string>> GetMemberBalance(string memberId);
        Task<Result<int, string>> SetMemberBalance(string memberId);
        Task<Result<int, string>> AddToMemberBalance(string memberId);
    }
}
