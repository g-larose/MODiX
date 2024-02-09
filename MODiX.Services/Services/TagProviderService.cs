using MODiX.Data.Models;
using MODiX.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODiX.Services.Services
{
    public class TagProviderService : ITagProvider
    {
        public async Task<Tag> HandleTagCommandAsync(string cmd, string[] args)
        {
            var tag = new Tag();
            switch (cmd)
            {
                case "create":

                    break;
                case "edit":

                    break;
                case "find":

                    break;
                case "delete":

                    break;
                default:
                    return tag;
            }
            return null;
        }
    }
}
