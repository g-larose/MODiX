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
        Result<int, SystemError> GetDaily();
        Result<int, SystemError> GetWork();
        Result<int, SystemError> GetCommunity();
        Result<int, SystemError> GetHobby();
        Result<int, SystemError> GetChores();
        Result<double, SystemError> GetMemberBankBalance(string memberId);
        Result<int, SystemError> GetMemberWalletBalance(string memberId);
        Task<Result<int, SystemError>> SetMemberBalance(string memberId);
        Task<Result<int, SystemError>> AddToMemberBalance(string memberId);
    }
}
