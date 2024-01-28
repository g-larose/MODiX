using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guilded.Content;

namespace MODiX.Services.Interfaces
{
    public interface IMessageHandler
    {
        Task HandleMessageAsync(Message message);
    }
}
