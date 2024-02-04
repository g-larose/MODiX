using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MODiX.Data.Factories;
using MODiX.Data.Models;
using MODiX.Services.Interfaces;


namespace MODiX.Services.Services
{
    public class DataProviderService : IDataProvider
    {
        private readonly ModixDbContextFactory dbFactory;
        public List<LocalServerMember> GetAllServerMembers()
        {
            var db = dbFactory.CreateDbContext();
            var members = db!.ServerMembers!.ToList();


            return members;
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

        public void SaveMemberToDb(LocalServerMember member)
        {
            var db = dbFactory.CreateDbContext();
            var user = db!.ServerMembers!.Where(x => x.Nickname.Equals(member.Nickname));
            if (user is null)
            {
                db!.ServerMembers!.Add(member);
                db.SaveChanges();
            }

        }
    }
}
