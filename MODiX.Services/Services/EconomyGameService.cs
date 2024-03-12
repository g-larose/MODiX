using Microsoft.EntityFrameworkCore.Internal;
using MODiX.Data;
using MODiX.Data.Factories;
using MODiX.Data.Models;
using MODiX.Services.BaseModules;
using MODiX.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODiX.Services.Services
{
    public class EconomyGameService : IEconomyGame, IDisposable
    {
        //chores: 100 - 999
        //hobby: 500 - 1599
        //community: 1000 - 1999
        //work: 1500 - 4999
        //daily: 4000 - 9999
        private readonly ModixDbContextFactory? _dbFactory = new();

        public Task<Result<int, string>> AddToMemberBalance(string memberId)
        {
            throw new NotImplementedException();
        }


        public Result<int, string> GetChores()
        {
            var rnd = new Random();
            var chores = rnd.Next(100, 999);
            if (chores < 100 || chores > 999) return Result<int, string>.Err("failure: unable to generate chores points")!;
            return chores;
        }

        public Result<int, string> GetCommunity()
        {
            throw new NotImplementedException();
        }

        public Result<int, string> GetDaily()
        {
            throw new NotImplementedException();
        }

        public Result<int, string> GetHobby()
        {
            var rnd = new Random();
            var hobby = rnd.Next(500, 1599);
            if (hobby < 100 || hobby > 999) return Result<int, string>.Err("failure: unable to generate hobby points")!;
            return hobby;
        }

        public Task<Result<int, string>> GetMemberBalance(string memberId)
        {
            throw new NotImplementedException();
        }

        public Result<int, string> GetWork()
        {
            throw new NotImplementedException();
        }

        public Task<Result<int, string>> SetMemberBalance(string memberId)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            DisposableBase disposableBase = new();
            disposableBase.Dispose();
        }
    }
}
