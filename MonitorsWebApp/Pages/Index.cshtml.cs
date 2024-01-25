using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MonitorsApp.Interfaces;
using MonitorsApp.BLC;
using MonitorsApp.Core;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MonitorsWebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly BLC blc;

        public IndexModel(ILogger<IndexModel> logger, BLC blc)
        {
            _logger = logger;
            this.blc = blc;
        }

        public List<IProducer> Producers { get; set; }
        public List<IMonitor> Monitors { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ProducerFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public string MonitorFilter { get; set; }

        [BindProperty (SupportsGet = true)]
        public string FilterType { get; set; }

        public void OnGet()
        {
            Producers = (List<IProducer>)blc.GetProducers();
            Monitors = (List<IMonitor>)blc.GetMonitors();

            if (!string.IsNullOrEmpty(ProducerFilter))
            {
                Producers = Producers.Where(p => p.Name.Contains(ProducerFilter, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!string.IsNullOrEmpty(MonitorFilter))
            {
                Console.WriteLine(MonitorFilter, FilterType);
                switch (FilterType)
                {
                    case "producer":
                        Monitors = Monitors.Where(m => m.Producer.Name.Contains(MonitorFilter, StringComparison.OrdinalIgnoreCase)).ToList();
                        Console.WriteLine("PRODUCER");
                        break;
                    case "name":
                        Monitors = Monitors.Where(m => m.Name.Contains(MonitorFilter, StringComparison.OrdinalIgnoreCase)).ToList();
                        Console.WriteLine("name");
                        break;
                    case "size":
                        Monitors = Monitors.Where(m => m.ScreenSize.ToString().Equals(MonitorFilter)).ToList();
                        Console.WriteLine("size");
                        break;
                    case "rate":
                        Monitors = Monitors.Where(m => m.RefreshRate.ToString().Equals(MonitorFilter)).ToList();
                        Console.WriteLine("rate");
                        break;
                    case "matrix":
                        Console.WriteLine("matrrix");
                        if (Enum.TryParse(MonitorFilter, out MatrixType matrixType))
                        {
                            Monitors = Monitors.Where(m => m.Matrix == matrixType).ToList();
                        }
                        break;
                    default:
                        Console.WriteLine("Blad filtra:", FilterType);
                        break;
                }
            }
        }

        public async Task<IActionResult> OnPostAddProducerAsync(string ProducerName)
        {
            var existingProducer = blc.GetProducers().FirstOrDefault(p => p.Name.Equals(ProducerName, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(ProducerName) && existingProducer == null)
            {
                blc.AddNewProducer(ProducerName);
                Producers = (List<IProducer>)blc.GetProducers();
                Console.WriteLine("UI dodano producenta:", ProducerName);
            }
            else if (existingProducer != null)
            {
                Console.WriteLine("Producent o tej nazwie już istnieje");
                ModelState.AddModelError("", "Producent o tej nazwie już istnieje.");
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteProducerAsync(int ProducerID)
        {
            var producer = blc.GetProducers().FirstOrDefault(p => p.ID.Equals(ProducerID));
            var existingProducer = blc.GetProducers().FirstOrDefault(p => p.Name.Equals(producer.Name, StringComparison.OrdinalIgnoreCase));
            var monitorsForProducer = blc.GetMonitors().Any(m => m.Producer.Name == producer.Name);

            if (!string.IsNullOrWhiteSpace(producer.Name) && existingProducer != null && !monitorsForProducer)
            {
                blc.RemoveProducer(producer.Name);
                Producers = (List<IProducer>)blc.GetProducers();
                Console.WriteLine("UI usunięto producenta:", producer.Name);
            }
            else if (existingProducer == null)
            {
                Console.WriteLine("Producent o tej nazwie nie istnieje");
                ModelState.AddModelError("", "Producent o tej nazwie nie istnieje.");
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditProducerAsync(int ProducerId, string NewName)
        {
            var existingProducer = blc.GetProducers().FirstOrDefault(p => p.Name.Equals(NewName, StringComparison.OrdinalIgnoreCase));
            if (existingProducer == null)
            {
                blc.EditProducer(ProducerId, NewName);
                Producers = (List<IProducer>)blc.GetProducers();
                Console.WriteLine("UI zmieniono producenta:", NewName);
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostAddMonitorAsync(string MonitorName, int ProducerId, int ScreenSize, int RefreshRate, int MatrixTypeId)
        {
            if (!string.IsNullOrEmpty(MonitorName) && RefreshRate > 0 && ScreenSize > 0)
            {
                blc.AddNewMonitor(MonitorName, ProducerId, RefreshRate, ScreenSize, MatrixTypeId);
                Monitors = (List<IMonitor>)blc.GetMonitors();
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteMonitorAsync(int MonitorID)
        {
            var monitor = blc.GetMonitors().FirstOrDefault(m => m.ID == MonitorID);
            var existingMonitor = blc.GetMonitors().FirstOrDefault(p => p.Name.Equals(monitor.Name, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(monitor.Name) && existingMonitor != null)
            {
                blc.RemoveMonitor(monitor.Name);
                Monitors = (List<IMonitor>)blc.GetMonitors();
                Console.WriteLine("UI usunięto producenta:", monitor.Name);
            }
            else if (existingMonitor == null)
            {
                Console.WriteLine("Monitor o tej nazwie nie istnieje");
                ModelState.AddModelError("", "Monitor o tej nazwie nie istnieje.");
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditMonitorAsync(int MonitorId, string MonitorName, int ProducerId, int RefreshRate, int ScreenSize, int MatrixTypeId)
        {
            if (!string.IsNullOrWhiteSpace(MonitorName) && RefreshRate > 0 && ScreenSize > 0)
            {
                blc.EditMonitor(MonitorId, MonitorName, ProducerId, RefreshRate, ScreenSize, MatrixTypeId);
                Monitors = (List<IMonitor>)blc.GetMonitors();
            }
            
            return RedirectToPage();
        }
    }
}