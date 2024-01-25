using MonitorsApp.Core;
using MonitorsApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorsApp.DAOMock
{
    public class DAOMock : IDAO
    {

        private List<IProducer> producers;
        private List<IMonitor> monitors;

        private int producersID;
        private int monitorsID;

        public DAOMock(String connectionString) 
        {
            producers = new List<IProducer>()
            {
                new BO.Producer() {ID = 1, Name="MSI"},
                new BO.Producer() {ID = 2, Name="Asus"}
            };

            monitors = new List<IMonitor>()
            {
                new BO.Monitor() {ID = 1, Name="ROG Strix", Producer=producers[1], RefreshRate=144, ScreenSize=28, Matrix=MonitorsApp.Core.MatrixType.IPS},
                new BO.Monitor() {ID = 2, Name="ROG", Producer=producers[1], RefreshRate=144, ScreenSize=28, Matrix=MonitorsApp.Core.MatrixType.VA},
                new BO.Monitor() {ID = 3, Name="Kings", Producer=producers[0], RefreshRate=240, ScreenSize=28, Matrix=MonitorsApp.Core.MatrixType.IPS},
            };

            producersID = producers.Max(p => p.ID);
            monitorsID = monitors.Max(m => m.ID);
        }    
        public void CreateNewMonitor(string name, int producerID, int refreshRate, int screenSize, int matrixID)
        {
            IProducer producer = producers.FirstOrDefault(producer => producer.ID == producerID);

            monitorsID++;
            monitors.Add(new BO.Monitor() {ID = monitorsID, Name = name, Producer=producer, RefreshRate=refreshRate, ScreenSize=screenSize, Matrix = (MatrixType)Enum.GetValues(typeof(MatrixType)).GetValue(matrixID) });
        }

        public void CreateNewProducer(string name)
        {
            producersID++;
            producers.Add( new BO.Producer() {ID = producersID, Name = name});
            Console.WriteLine("DAO dodano producenta:", name);
        }

        public void DeleteMonitor(string name)
        {
            var monitor = monitors.FirstOrDefault(p => p.Name == name);

            if (monitor != null)
            {
                monitors.Remove(monitor);
            }
        }

        public void DeleteProducer(string name)
        {
            var producer = producers.FirstOrDefault(p => p.Name == name);

            if (producer != null)
            {
                producers.Remove(producer);
            }
        }

        public void EditMonitor(int ID, string name, int producerID, int refreshRate, int screenSize, int matrixID)
        {
            var monitor = monitors.FirstOrDefault(p => p.ID == ID);

            if (monitor != null)
            {
                IProducer producer = producers.FirstOrDefault(p => p.ID == producerID);

                monitor.Name = name;
                monitor.Producer = producer;
                monitor.RefreshRate = refreshRate;
                monitor.ScreenSize = screenSize;
                monitor.Matrix = (MatrixType)Enum.GetValues(typeof(MatrixType)).GetValue(matrixID);
            }
        }

        public void EditProducer(int ID, string name)
        {
            var producer = producers.FirstOrDefault(p => p.ID == ID);

            if (producer != null)
            {
                producer.Name = name;
            }
        }

        public IEnumerable<IMonitor> GetAllMonitors()
        {
            return monitors;
        }

        public IEnumerable<IProducer> GetAllProducers()
        {
            return producers;
        }
    }
}
