using Guilded.Servers;
using MODiX.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MODiX.Services.Services
{
    public class DiceRollerProviderService
    {
        public DiceRoller Roll(Member member, int dieAmount = 6, int sides = 6)
        {
            var roller = new DiceRoller();
            var rnd = new Random();
            var date = DateTime.Now.ToShortDateString();
            var time = DateTime.Now.ToShortTimeString();
            try
            {
                
                for (int i = 0; i < dieAmount; i++)
                {
                    var num = rnd.Next(1, sides);
                    roller.Die!.Add(num);
                }
                roller.Id = Guid.NewGuid();
                roller.Sides = sides;
                roller.RolledAt = $"{date} {time}";
                roller.IsValid = true;
                roller.Member = member;
            }
            catch
            {
                roller.Id = Guid.NewGuid();
                roller.IsValid = false;
                roller.Member = member;
                roller.RolledAt = $"{date} {time}";
            }
            
            return roller;
        }
    }
}
