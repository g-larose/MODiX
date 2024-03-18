using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MODiX.Data.Factories;
using MODiX.Data.Models;
using MODiX.Services.Interfaces;


namespace MODiX.Services.Services
{
    public class DataProviderService : IDataProvider
    {
        private readonly ModixDbContextFactory dbFactory = new();
        public List<LocalServerMember> GetAllServerMembers()
        {
            var db = dbFactory.CreateDbContext();
            var members = db!.ServerMembers!.ToList();


            return members;
        }

        public async Task<LocalServerMember> GetMemberFromDb(string username)
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

        public Result<Riddle, SystemError> GetRiddle()
        {
            var error = new SystemError();
            using var db = dbFactory.CreateDbContext();
            try
            {
                var riddleFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Riddles", "riddles.json");
                var json = File.ReadAllText(riddleFile);
                var riddles = JsonSerializer.Deserialize<RiddleRoot>(json);
                var rnd = new Random();
                var riddle = riddles?.riddles?[rnd.Next(1, riddles.riddles.Count)];
                if (riddle is not null)
                    return Result<Riddle, SystemError>.Ok(riddle!)!;
            }
            catch(Exception e)
            {
                error.ErrorMessage = e.Message;
                error.ErrorCode = Guid.NewGuid();
                db.Add(error);
                db.SaveChanges();
                return Result<Riddle, SystemError>.Err(error)!;
            }

            error.ErrorMessage = "could not load riddle";
            error.ErrorCode = Guid.NewGuid();
            db.Add(error);
            db.SaveChanges();
            return Result<Riddle, SystemError>.Err(error)!;
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
