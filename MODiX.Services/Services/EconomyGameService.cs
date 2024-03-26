using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using MODiX.Data;
using MODiX.Data.Factories;
using MODiX.Data.Models;
using MODiX.Services.BaseModules;
using MODiX.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        #region ADD TO MEMBER BALANCE
        public Task<Result<int, SystemError>> AddToMemberBalance(string memberId)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region GET CHORES
        public Result<int, SystemError> GetChores()
        {
            var rnd = new Random();
            var chores = rnd.Next(100, 999);
            if (chores < 100 || chores > 999) return Result<int, SystemError>.Err(new SystemError()
            {
                ErrorCode = Guid.NewGuid(),
                ErrorMessage = "index out of range, the int returned was either to low or to high."
            })!;
            return Result<int, SystemError>.Ok(chores)!;
        }
        #endregion

        #region GET COMMUNITY
        public Result<int, SystemError> GetCommunity()
        {
            var rnd = new Random();
            var luck = rnd.Next(1000, 1999);
            var community = rnd.Next(1000, 1999);
            if (community < 1000 || community > 1999)
                return Result<int, SystemError>.Err(new SystemError()
                {
                    ErrorCode = Guid.NewGuid(),
                    ErrorMessage = "index out of range, the int returned was either to low or to high."
                })!;
            if (luck == community)
                community += 2000;
            return Result<int, SystemError>.Ok(community)!;
        }
        #endregion

        #region GET DAILY
        public Result<int, SystemError> GetDaily()
        {
            var rnd = new Random();
            var luck = rnd.Next(4000, 9999);
            var daily = rnd.Next(4000, 9999);
            if (daily < 4000 || daily > 9999) 
                return Result<int, SystemError>.Err(new SystemError()
                {
                    ErrorCode = Guid.NewGuid(),
                    ErrorMessage = "index out of range, the int returned was either to low or to high."
                })!;
            if (luck == daily)
                daily += 2000;
            return Result<int, SystemError>.Ok(daily)!;
        }
        #endregion

        #region IS VALID DAILY

        public Result<bool, SystemError> IsValidDaily(string memberId)
        {
            using var db = _dbFactory!.CreateDbContext();
            var mem = db.ServerMembers!.Where(x => x.UserId!.Equals(memberId)).Include(b => b.Bank).FirstOrDefault();
            if (mem is null)
            {
                //no member in the db return SystemError
                return Result<bool, SystemError>.Err(new SystemError()
                {
                    ErrorCode = Guid.NewGuid(),
                    ErrorMessage = "not found, member does not exist in the database, run m?addmem member to add the member to the database"
                })!;
            }
            else
            {
                DateTime pDate;
                var daily = mem.Bank.LastDaily;
                var now = DateTime.UtcNow;
                var isValid = now > daily;
                return Result<bool, SystemError>.Ok(isValid)!;
            }
        }

        #endregion

        #region GET HOBBY
        public Result<int, SystemError> GetHobby()
        {
            var rnd = new Random();
            var luck = rnd.Next(500, 1599);
            var hobby = rnd.Next(500, 1599);
            if (hobby < 500 || hobby > 1599)
                return Result<int, SystemError>.Err(new SystemError()
                {
                    ErrorCode = Guid.NewGuid(),
                    ErrorMessage = "index out of range, the int returned was either to low or to high."
                })!;
            if (luck == hobby)
                hobby += 2000;
            return Result<int, SystemError>.Ok(hobby)!;
        }
        #endregion

        #region GET MEMBER BALANCE
        public Result<double, SystemError> GetMemberBankBalance(string memberId)
        {
            using var db = _dbFactory?.CreateDbContext();
            var member = db?.ServerMembers?.Where(x => x.UserId!.Equals(memberId))
                .Include(b => b.Bank)
                .FirstOrDefault();
            if (member is null)
            {
                return Result<double, SystemError>.Err(new SystemError()
                {
                    ErrorCode = Guid.NewGuid(),
                    ErrorMessage = "Not Found Error: user not found in Database."
                })!;
            }
            else
            {
                return Result<double, SystemError>.Ok(member.Bank.AccountTotal)!;
            }
        }
        #endregion

        #region GET MEMBER WALLET BALANCE
        public Result<int, SystemError> GetMemberWalletBalance(string memberId)
        {
            using var db = _dbFactory?.CreateDbContext();
            var member = db?.ServerMembers?.Where(x => x.UserId!.Equals(memberId))
                .Include(w => w.Wallet)
                .FirstOrDefault();
            if (member is null)
            {
                return Result<int, SystemError>.Err(new SystemError()
                {
                    ErrorCode = Guid.NewGuid(),
                    ErrorMessage = "Not Found Error: user not found in Database."
                })!;
            }
            else
            {
                return Result<int, SystemError>.Ok(member.Wallet.Points)!;
            }
        }

        #endregion

        #region GET WORK
        public Result<int, SystemError> GetWork()
        {
            var rnd = new Random();
            var luck = rnd.Next(1500, 4999);
            var work = rnd.Next(1500, 4999);
            if (work < 1500 || work > 4999)
                return Result<int, SystemError>.Err(new SystemError()
                {
                    ErrorCode = Guid.NewGuid(),
                    ErrorMessage = "index out of range, the int returned was either to low or to high."
                })!;
            if (luck == work)
                work += 2000;
            return Result<int, SystemError>.Ok(work)!;
        }
        #endregion

        #region SET MEMBER BALANCE
        public Task<Result<int, SystemError>> SetMemberBalance(string memberId)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region DISPOSE
        public void Dispose()
        {
            DisposableBase disposableBase = new();
            disposableBase.Dispose();
        }
        #endregion
    }
}
