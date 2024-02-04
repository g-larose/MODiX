using MODiX.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MODiX.Services.Interfaces
{
    public interface IDataProvider
    {
        LocalServerMember GetMemberFromDb(string username);
        LocalServerMember SaveMemberToDb(LocalServerMember member);
        List<LocalServerMember> GetAllServerMembers();
        LocalServerMember RemoveServerMemberFromDb(string username);
        int GetMemberXpByName(string username);
        int GetMemberWarningsByName(string username);
    }
}
