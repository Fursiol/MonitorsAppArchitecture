using MonitorsApp.Interfaces;
using System.Reflection;

namespace MonitorsApp.BLC
{
    public class BLC
    {
        private IDAO dao;

        public BLC(String libraryName, String connectionString) 
        {
            Type? typeToCreate = null;

            Assembly assembly = Assembly.UnsafeLoadFrom(libraryName);

            foreach (Type type in assembly.GetTypes()) 
            {
                if (type.IsAssignableTo(typeof(IDAO))) 
                {
                    typeToCreate = type;
                    break;
                }
            }
            dao = (IDAO)Activator.CreateInstance(typeToCreate, new object[] { connectionString });
        }

        public IEnumerable<IProducer> GetProducers()
        {
            return dao.GetAllProducers();
        }
        public IEnumerable<IMonitor> GetMonitors()
        {
            return dao.GetAllMonitors();
        }

        public void AddNewProducer(string name)
        {
            Console.WriteLine("BLC dodano producenta:", name);
            dao.CreateNewProducer(name);
        }

        public void AddNewMonitor(string name, int producerID, int refreshRate, int screenSize, int matrixID)
        {
            dao.CreateNewMonitor(name, producerID, refreshRate, screenSize, matrixID);
        }

        public void RemoveProducer(string name)
        {
            dao.DeleteProducer(name);
        }

        public void RemoveMonitor(string name)
        {
            dao.DeleteMonitor(name);
        }

        public void EditProducer(int ID, string name)
        {
            dao.EditProducer(ID, name);
        }

        public void EditMonitor(int ID, string name, int producerID, int refreshRate, int screenSize, int matrixID)
        {
            dao.EditMonitor(ID, name, producerID, refreshRate, screenSize, matrixID);
        }
    }
}