using MonitorsApp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorsApp.Interfaces
{
    public interface IMonitor
    {
        int ID { get; }
        string Name { get; set; }
        IProducer Producer { get; set; }
        int ScreenSize { get; set; }
        int RefreshRate { get; set; }
        MatrixType Matrix { get; set; }
    }
}
