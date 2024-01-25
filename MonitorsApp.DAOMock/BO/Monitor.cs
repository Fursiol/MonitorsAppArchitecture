using MonitorsApp.Core;
using MonitorsApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorsApp.DAOMock.BO
{
    internal class Monitor : IMonitor
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public IProducer Producer { get; set; }
        public int ScreenSize { get; set; }
        public int RefreshRate { get; set; }
        public MatrixType Matrix { get; set; }
    }
}
