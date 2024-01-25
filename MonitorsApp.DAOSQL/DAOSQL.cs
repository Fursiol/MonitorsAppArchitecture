using Microsoft.EntityFrameworkCore;
using MonitorsApp.Core;
using MonitorsApp.Interfaces;

namespace MonitorsApp.DAOSQL
{
    public class DAOSQL : IDAO
    {
        private readonly AppDbContext _context;

        public DAOSQL(String connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlite(connectionString);
            _context = new AppDbContext(optionsBuilder.Options);
        }
        public void CreateNewMonitor(string name, int producerID, int refreshRate, int screenSize, int matrixType)
        {
            var monitor = new EfCoreMonitor
            {
                Name = name,
                ProducerID = producerID,
                RefreshRate = refreshRate,
                ScreenSize = screenSize,
                Matrix = (MatrixType)Enum.GetValues(typeof(MatrixType)).GetValue(matrixType)
            };
            _context.Monitors.Add(monitor);
            _context.SaveChanges();
        }

        public void CreateNewProducer(string name)
        {
            var producer = new EfCoreProducer
            {
                Name = name
            };
            _context.Producers.Add(producer);
            _context.SaveChanges();
        }

        public void DeleteMonitor(string name)
        {
            var monitor = _context.Monitors.FirstOrDefault(m => m.Name == name);
            if (monitor != null)
            {
                _context.Monitors.Remove(monitor);
                _context.SaveChanges();
            }
        }

        public void DeleteProducer(string name)
        {
            var producer = _context.Producers.FirstOrDefault(p => p.Name == name);
            if (producer != null)
            {
                _context.Producers.Remove(producer);
                _context.SaveChanges();
            }
        }

        public void EditMonitor(int ID, string name, int producerID, int refreshRate, int screenSize, int matrixType)
        {
            var monitor = _context.Monitors.Find(ID);
            if (monitor != null)
            {
                monitor.Name = name;
                monitor.ProducerID = producerID;
                monitor.RefreshRate = refreshRate;
                monitor.ScreenSize = screenSize;
                monitor.Matrix = (MatrixType)Enum.GetValues(typeof(MatrixType)).GetValue(matrixType);

                _context.Monitors.Update(monitor);
                _context.SaveChanges();
            }
        }

        public void EditProducer(int ID, string name)
        {
            var producer = _context.Producers.Find(ID);
            if (producer != null)
            {
                producer.Name = name;

                _context.Producers.Update(producer);
                _context.SaveChanges();
            }
        }

        public IEnumerable<IMonitor> GetAllMonitors()
        {
            var monitors = _context.Monitors.Include(m => m.Producer).ToList();
            return monitors.Select(m => new BO.Monitor
            {
                ID = m.ID,
                Name = m.Name,
                Producer = new BO.Producer
                {
                    ID = m.Producer.ID,
                    Name = m.Producer.Name
                },
                ScreenSize = m.ScreenSize,
                RefreshRate = m.RefreshRate,
                Matrix = m.Matrix,
                ProducerID = m.ProducerID
            }).Cast<IMonitor>().ToList();
        }
    
        public IEnumerable<IProducer> GetAllProducers()
        {
            var producers = _context.Producers.ToList();
            return producers.Select(p => new BO.Producer
            {
                ID = p.ID,
                Name = p.Name
            }).Cast<IProducer>().ToList();
        }
    }
}