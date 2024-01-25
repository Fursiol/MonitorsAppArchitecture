using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorsApp.Interfaces
{
    public interface IDAO
    {
        IEnumerable<IProducer> GetAllProducers();
        IEnumerable<IMonitor> GetAllMonitors();
        void CreateNewProducer(string name);
        void CreateNewMonitor(string name, int producerID, int refreshRate, int screenSize, int matrixID);
        void DeleteProducer(string name);
        void DeleteMonitor(string name);
        void EditProducer(int ID, string name);
        void EditMonitor(int ID, string name, int producerID, int refreshRate, int screenSize, int matrixID);
    }
}
