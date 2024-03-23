using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dariosoft.Framework
{
    public interface ILifetime
    {
        int Order { get; }

        bool Enabled { get; }

        Task OnAppStarted();
        
        Task OnAppStoping();
        
        Task OnAppStopped();
    }
}
