using MonitorsApp.Interfaces;

namespace MonitorsApp.DAOMock.BO
{
    public class Producer : IProducer
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
}