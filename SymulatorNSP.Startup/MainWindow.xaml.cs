using MahApps.Metro.Controls;
using SymulatorNSP.Core;
using SymulatorNSP.GUI.WPF.WPFViewModels;
using System;
using System.Collections.Generic;
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

namespace SymulatorNSP.Startup
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            var vm = new GUSViewModel();
            vm.OnQueryPerformed = new Action<SymulatorNSP.GUI.Core.ViewModels.QueryViewModel, GUSViewModel>((q,g) =>
            {
                //var res = GUSViewModel.GetComparisonString(q);
                //if (res == null)
                //    return;
                //var window = new CongratulationsWindow(res);
                //window.ShowDialog();
            });
            vm.OnResultSend = new Action<SymulatorNSP.GUI.Core.ViewModels.QueryViewModel>((q) =>
            {
                var window = new LeaderboardSendWindow(q);
                window.ShowDialog();
                return;
            });
            this.DataContext = vm;
        }
    }
}
