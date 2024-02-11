using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MODiX.Services.Features._8Ball
{
    public class _8BallProviderService : IDisposable
    {
        private bool _disposedValue;
        private SafeHandle? _safeHandle = new SafeFileHandle(IntPtr.Zero, true);
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _safeHandle?.Dispose();
                    _safeHandle = null;
                }

                _disposedValue = true;
            }
        }

        public string GetEightBallResponse()
        {
            var jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Features", "8Ball", "eightball_response.json");
            var json = File.ReadAllText(jsonPath);
            var response = JsonSerializer.Deserialize<EightBallResponse>(json);
            var rnd = new Random();
            var index = rnd.Next(1, response!.responses!.Count);
            return response.responses[index];
        }
    }
}
