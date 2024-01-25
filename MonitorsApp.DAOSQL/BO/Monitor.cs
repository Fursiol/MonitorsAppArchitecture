using MonitorsApp.Core;
using MonitorsApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonitorsApp.DAOSQL.BO
{
    public class Monitor : IMonitor
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }

        [ForeignKey("ProducerID")]
        public int ProducerID { get; set; }
        public IProducer Producer { get; set; }
        public int ScreenSize { get; set; }
        public int RefreshRate { get; set; }
        public MatrixType Matrix { get; set; }
    }
}
