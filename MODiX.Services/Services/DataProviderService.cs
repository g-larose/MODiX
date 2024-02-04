using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MODiX.Data.Models;
using MODiX.Services.Interfaces;


namespace MODiX.Services.Services
{
    public class DataProviderService : IDataProvider
    {
        public List<LocalServerMember> GetAllServerMembers()
        {
            throw new NotImplementedException();
        }

        public LocalServerMember GetMemberFromDb(string username)
        {
            throw new NotImplementedException();
        }

        public int GetMemberWarningsByName(string username)
        {
            throw new NotImplementedException();
        }

        public int GetMemberXpByName(string username)
        {
            throw new NotImplementedException();
        }

        public LocalServerMember RemoveServerMemberFromDb(string username)
        {
            throw new NotImplementedException();
        }

        public LocalServerMember SaveMemberToDb(LocalServerMember member)
        {
            throw new NotImplementedException();
        }
    }
}
