using MonitorsApp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorsApp.DAOSQL
{
    public class EfCoreProducer
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ICollection<EfCoreMonitor> Monitors { get; set; }
    }

    public class EfCoreMonitor
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int ProducerID { get; set; }
        public EfCoreProducer Producer { get; set; }
        public int ScreenSize { get; set; }
        public int RefreshRate { get; set; }
        public MatrixType Matrix { get; set; }
    }
}
