using MonitorsApp.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace MonitorsApp.DAOSQL.BO
{
    public class Producer : IProducer
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public virtual ICollection<Monitor> Monitors { get; set; }
    }
}