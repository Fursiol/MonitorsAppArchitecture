using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MonitorsApp.BLC;
using MonitorsApp.Interfaces;
using System.Windows.Input;
using System.Windows;
using MonitorsApp.Core;
using System.Security.Policy;

namespace MonitorsApp
{
    public class ProducersViewModel : INotifyPropertyChanged
    {
        private BLC.BLC _blc;

        private ObservableCollection<IProducer> _producers;
        private ObservableCollection<IMonitor> _monitors;
        private Visibility _addProducerFormVisibility = Visibility.Collapsed;
        public Visibility AddProducerFormVisibility
        {
            get => _addProducerFormVisibility;
            set
            {
                _addProducerFormVisibility = value;
                OnPropertyChanged(nameof(AddProducerFormVisibility));
            }
        }

        private Visibility _deleteProducerFormVisibility = Visibility.Collapsed;
        public Visibility DeleteProducerFormVisibility
        {
            get => _deleteProducerFormVisibility;
            set
            {
                _deleteProducerFormVisibility = value;
                OnPropertyChanged(nameof(DeleteProducerFormVisibility));
            }
        }

        private Visibility _editProducerFormVisibility = Visibility.Collapsed;
        public Visibility EditProducerFormVisibility
        {
            get => _editProducerFormVisibility;
            set
            {
                _editProducerFormVisibility = value;
                OnPropertyChanged(nameof(EditProducerFormVisibility));
            }
        }

        public ObservableCollection<IProducer> Producers
        {
            get => _producers;
            set
            {
                _producers = value;
                OnPropertyChanged(nameof(Producers));
            }
        }

        public ObservableCollection<IMonitor> Monitors
        {
            get => _monitors;
            set
            {
                _monitors = value;
                OnPropertyChanged(nameof(Monitors));
            }
        }

        public ICommand AddProducerCommand { get; private set; }
        public ICommand ShowAddProducerFormCommand { get; private set; }
        public ICommand ShowDeleteProducerFormCommand { get; private set; }
        public ICommand ShowEditProducerFormCommand { get; private set; }
        public ICommand DeleteProducerCommand { get; private set; }
        public ICommand EditProducerCommand { get; private set; }
        public ProducersViewModel(BLC.BLC blc)
        {
            _blc = blc;
            Producers = new ObservableCollection<IProducer>(blc.GetProducers());
            Monitors = new ObservableCollection<IMonitor>(blc.GetMonitors());

            AddProducerCommand = new RelayCommand(AddProducer);
            ShowAddProducerFormCommand = new RelayCommand(ShowAddProducerForm);
            ShowDeleteProducerFormCommand = new RelayCommand(ShowDeleteProducerForm);
            DeleteProducerCommand = new RelayCommand(DeleteProducer);
            ShowEditProducerFormCommand = new RelayCommand(ShowEditProducerForm);
            EditProducerCommand = new RelayCommand(EditProducer);

            ShowAddMonitorFormCommand = new RelayCommand(ShowAddMonitorForm);
            AddMonitorCommand = new RelayCommand(AddMonitor);
            ShowDeleteMonitorFormCommand = new RelayCommand(ShowDeleteMonitorForm);
            DeleteMonitorCommand = new RelayCommand(DeleteMonitor);
            ShowEditMonitorFormCommand = new RelayCommand(ShowEditMonitorForm);
            EditMonitorCommand = new RelayCommand(EditMonitor);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //Producenci----------------------------------------------------------------------------------------

        private string _filter;

        public string Filter
        {
            get => _filter;
            set
            {
                if (_filter != value)
                {
                    _filter = value;
                    OnPropertyChanged();
                    FilterProducers();
                }
            }
        }
        private void FilterProducers()
        {
            if (string.IsNullOrEmpty(_filter))
            {
                Producers = new ObservableCollection<IProducer>(_blc.GetProducers());
            }
            else
            {
                var filteredProducers = _blc.GetProducers()
                    .Where(p => p.Name.Contains(Filter, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                Producers = new ObservableCollection<IProducer>(filteredProducers);
            }
        }

        private void ShowAddProducerForm()
        {
            DeleteProducerFormVisibility = Visibility.Collapsed;
            EditProducerFormVisibility = Visibility.Collapsed;

            if (AddProducerFormVisibility != Visibility.Visible)
            {
                AddProducerFormVisibility = Visibility.Visible;
            }
            else
            {
                AddProducerFormVisibility = Visibility.Collapsed;
            }
        }

        private void ShowDeleteProducerForm()
        {
            AddProducerFormVisibility = Visibility.Collapsed;
            EditProducerFormVisibility = Visibility.Collapsed;

            if (DeleteProducerFormVisibility != Visibility.Visible)
            {
                DeleteProducerFormVisibility = Visibility.Visible;
            }
            else
            {
                DeleteProducerFormVisibility = Visibility.Collapsed;
            }
        }

        private void ShowEditProducerForm()
        {
            AddProducerFormVisibility = Visibility.Collapsed;
            DeleteProducerFormVisibility = Visibility.Collapsed;

            if (EditProducerFormVisibility != Visibility.Visible)
            {
                EditProducerFormVisibility = Visibility.Visible;
            }
            else
            {
                EditProducerFormVisibility = Visibility.Collapsed;
            }
        }


        private string _newProducerName;
        public string NewProducerName
        {
            get => _newProducerName;
            set
            {
                _newProducerName = value;
                OnPropertyChanged(nameof(NewProducerName));
            }
        }
        private void AddProducer()
        {
            var existingProducer = _blc.GetProducers().FirstOrDefault(p => p.Name.Equals(NewProducerName, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(NewProducerName) && existingProducer == null)
            {
                _blc.AddNewProducer(NewProducerName);
                Producers = new ObservableCollection<IProducer>(_blc.GetProducers());
                NewProducerName = string.Empty;
            }
            AddProducerFormVisibility = Visibility.Collapsed;
        }

        private IProducer _selectedProducer;
        public IProducer SelectedProducer
        {
            get => _selectedProducer;
            set
            {
                if (_selectedProducer != value)
                {
                    _selectedProducer = value;
                    OnPropertyChanged(nameof(SelectedProducer));
                }
            }
        }
        private void DeleteProducer()
        {

            var existingProducer = _blc.GetProducers().FirstOrDefault(p => p.Name.Equals(SelectedProducer.Name, StringComparison.OrdinalIgnoreCase));
            var monitorsForProducer = _blc.GetMonitors().Any(m => m.Producer.Name == SelectedProducer.Name);

            if (SelectedProducer != null && existingProducer != null && !monitorsForProducer)
            {
                _blc.RemoveProducer(SelectedProducer.Name);
                Producers = new ObservableCollection<IProducer>(_blc.GetProducers());
            }
            DeleteProducerFormVisibility = Visibility.Collapsed;
        }

        private void EditProducer()
        {
            var existingProducer = _blc.GetProducers().FirstOrDefault(p => p.Name.Equals(NewProducerName, StringComparison.OrdinalIgnoreCase));
            if (SelectedProducer != null && !string.IsNullOrWhiteSpace(NewProducerName) && existingProducer == null)
            {
                _blc.EditProducer(SelectedProducer.ID, NewProducerName);
                Producers = new ObservableCollection<IProducer>(_blc.GetProducers());
                Monitors = new ObservableCollection<IMonitor>(_blc.GetMonitors());
                NewProducerName = string.Empty;
            }
            EditProducerFormVisibility = Visibility.Collapsed;
        }

        //Monitory----------------------------------------------------------------------------------------------
        private string _selectedMonitorFilterOption;
        public string SelectedMonitorFilterOption
        {
            get => _selectedMonitorFilterOption;
            set
            {
                _selectedMonitorFilterOption = value;
                OnPropertyChanged();
                FilterMonitors();
            }
        }

        private string _monitorFilter;
        public string MonitorFilter
        {
            get => _monitorFilter;
            set
            {
                _monitorFilter = value;
                OnPropertyChanged();
                FilterMonitors();
            }
        }

        private void FilterMonitors()
        {
            var monitors = _blc.GetMonitors();
            IEnumerable<IMonitor> filteredMonitors = monitors;

            if (!string.IsNullOrEmpty(MonitorFilter))
            {
                switch (SelectedMonitorFilterOption)
                {
                    case "Nazwa producenta":
                        filteredMonitors = monitors.Where(m => m.Producer.Name.Contains(MonitorFilter, StringComparison.OrdinalIgnoreCase)).ToList();
                        break;
                    case "Nazwa monitora":
                        filteredMonitors = monitors.Where(m => m.Name.Contains(MonitorFilter, StringComparison.OrdinalIgnoreCase)).ToList();
                        break;
                    case "Częstotliwość odświeżania":
                        if (int.TryParse(MonitorFilter, out var refreshRate))
                        {
                            filteredMonitors = monitors.Where(m => m.RefreshRate >= refreshRate).ToList();
                        }
                        break;
                    case "Przekątna ekranu":
                        if (int.TryParse(MonitorFilter, out var screenSize))
                        {
                            filteredMonitors = monitors.Where(m => m.ScreenSize >= screenSize).ToList();
                        }
                        break;
                    case "Typ matrycy":
                        if (Enum.TryParse(MonitorFilter, out MatrixType matrixType))
                        {
                            filteredMonitors = monitors.Where(m => m.Matrix == matrixType).ToList();
                        }
                        break;
                }
            }
            Monitors = new ObservableCollection<IMonitor>(filteredMonitors);
        }

        public string NewMonitorName { get; set; }
        public string NewMonitorRefreshRate { get; set; }
        public string NewMonitorScreenSize { get; set; }
        public string NewMonitorMatrixType { get; set; }
        public ICommand ShowAddMonitorFormCommand { get; private set; }
        public ICommand AddMonitorCommand { get; private set; }


        private Visibility _addMonitorFormVisibility = Visibility.Collapsed;
        public Visibility AddMonitorFormVisibility
        {
            get => _addMonitorFormVisibility;
            set
            {
                _addMonitorFormVisibility = value;
                OnPropertyChanged(nameof(AddMonitorFormVisibility));
            }
        }
        private void ShowAddMonitorForm()
        {
            DeleteMonitorFormVisibility = Visibility.Collapsed;
            EditMonitorFormVisibility = Visibility.Collapsed;

            if (AddMonitorFormVisibility != Visibility.Visible)
            {
                AddMonitorFormVisibility = Visibility.Visible;
            }
            else
            {
                AddMonitorFormVisibility = Visibility.Collapsed;
            }
        }

        private void AddMonitor()
        {
            int refreshRate = int.Parse(NewMonitorRefreshRate);
            int screenSize = int.Parse(NewMonitorScreenSize);

            if (!string.IsNullOrWhiteSpace(NewMonitorName) && SelectedProducer != null && refreshRate > 0 && screenSize > 0)
            {
                int NewMonitorMatrixID;

                if (NewMonitorMatrixType == "IPS") NewMonitorMatrixID = 0;
                else if (NewMonitorMatrixType == "VA") NewMonitorMatrixID = 1;
                else NewMonitorMatrixID = 2;

                _blc.AddNewMonitor(NewMonitorName, SelectedProducer.ID, refreshRate, screenSize, NewMonitorMatrixID);
                Monitors = new ObservableCollection<IMonitor>(_blc.GetMonitors());
            }
            AddMonitorFormVisibility = Visibility.Collapsed;
        }

        public IMonitor SelectedMonitorForDeletion { get; set; }

        public ICommand ShowDeleteMonitorFormCommand { get; private set; }
        public ICommand DeleteMonitorCommand { get; private set; }


        private Visibility _deleteMonitorFormVisibility = Visibility.Collapsed;
        public Visibility DeleteMonitorFormVisibility
        {
            get => _deleteMonitorFormVisibility;
            set
            {
                _deleteMonitorFormVisibility = value;
                OnPropertyChanged(nameof(DeleteMonitorFormVisibility));
            }
        }
        private void ShowDeleteMonitorForm()
        {
            AddMonitorFormVisibility = Visibility.Collapsed;
            EditMonitorFormVisibility = Visibility.Collapsed;

            if (DeleteMonitorFormVisibility != Visibility.Visible)
            {
                DeleteMonitorFormVisibility = Visibility.Visible;
            }
            else
            {
                DeleteMonitorFormVisibility = Visibility.Collapsed;
            }
        }

        private void DeleteMonitor()
        {
            var existingMonitor = _blc.GetMonitors().FirstOrDefault(p => p.Name.Equals(SelectedMonitorForDeletion.Name, StringComparison.OrdinalIgnoreCase));
            
            if (SelectedMonitorForDeletion != null && existingMonitor != null)
            {
                _blc.RemoveMonitor(SelectedMonitorForDeletion.Name);
                Monitors = new ObservableCollection<IMonitor>(_blc.GetMonitors());
            }
            DeleteMonitorFormVisibility = Visibility.Collapsed;
        }

        public ICommand ShowEditMonitorFormCommand { get; private set; }
        public ICommand EditMonitorCommand { get; private set; }
        public IMonitor SelectedMonitorForEdition { get; set; }

        public string SelectedMonitorForEditionScreenSize {  get; set; }
        public string SelectedMonitorForEditionRefreshRate { get; set; }
        public string SelectedMonitorForEditionName { get; set; }

        private Visibility _editMonitorFormVisibility = Visibility.Collapsed;
        public Visibility EditMonitorFormVisibility
        {
            get => _editMonitorFormVisibility;
            set
            {
                _editMonitorFormVisibility = value;
                OnPropertyChanged(nameof(EditMonitorFormVisibility));
            }
        }
        private void ShowEditMonitorForm()
        {
            DeleteMonitorFormVisibility = Visibility.Collapsed;
            AddMonitorFormVisibility = Visibility.Collapsed;

            if (EditMonitorFormVisibility != Visibility.Visible)
            {
                EditMonitorFormVisibility = Visibility.Visible;
            }
            else
            {
                EditMonitorFormVisibility = Visibility.Collapsed;
            }
        }

        public string EditMonitorMatrixType { get; set; }
        private void EditMonitor()
        {
            int SelectedMonitorForEditionRefreshRateInt = int.Parse(SelectedMonitorForEditionRefreshRate);
            int SelectedMonitorForEditionScreenSizeInt = int.Parse(SelectedMonitorForEditionScreenSize);
            int NewMonitorMatrixID;

            if (EditMonitorMatrixType == "IPS") NewMonitorMatrixID = 0;
            else if (EditMonitorMatrixType == "VA") NewMonitorMatrixID = 1;
            else NewMonitorMatrixID = 2;

            if (SelectedMonitorForEdition != null)
            {
                _blc.EditMonitor(
                    SelectedMonitorForEdition.ID,
                    SelectedMonitorForEditionName,
                    SelectedProducer.ID,
                    SelectedMonitorForEditionRefreshRateInt,
                    SelectedMonitorForEditionScreenSizeInt,
                    NewMonitorMatrixID
                );
                Monitors = new ObservableCollection<IMonitor>(_blc.GetMonitors());
            }
            EditMonitorFormVisibility = Visibility.Collapsed;
        }
    }
}
