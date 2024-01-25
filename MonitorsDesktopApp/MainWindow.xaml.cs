using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Extensions.Configuration;
using MonitorsApp.BLC;

namespace MonitorsApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ProducersViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();

            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

            var libraryPath = configuration.GetSection("DAOLibrary:Path").Value;
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            BLC.BLC blc = new BLC.BLC(libraryPath, connectionString);

            _viewModel = new ProducersViewModel(blc);
            DataContext = _viewModel;
        }
    }
}
