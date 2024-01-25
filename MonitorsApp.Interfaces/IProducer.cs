namespace MonitorsApp.Interfaces
{
    public interface IProducer
    {
        int ID { get; }
        string Name { get; set; }
        string Address { get; set; }
    }
}