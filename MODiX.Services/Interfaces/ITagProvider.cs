using MODiX.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODiX.Services.Interfaces
{
    public interface ITagProvider
    {
        Task<Tag> HandleTagCommandAsync(string cmd, string[] args);
    }
}
