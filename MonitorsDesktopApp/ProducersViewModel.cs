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

namespace MonitorsApp
{
    public class ProducersViewModel : INotifyPropertyChanged
    {
        private BLC.BLC _blc;

        private ObservableCollection<IProducer> _producers;
        public ObservableCollection<IProducer> Producers
        {
            get => _producers;
            set
            {
                _producers = value;
                OnPropertyChanged(nameof(Producers));
            }
        }

        public ProducersViewModel(BLC.BLC blc)
        {
            _blc = blc;
            Producers = new ObservableCollection<IProducer>(blc.GetProducers());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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
    }
}
