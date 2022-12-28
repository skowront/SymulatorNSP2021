using MahApps.Metro.Controls;
using SymulatorNSP.Core;
using SymulatorNSP.GUI.Core.Resources;
using SymulatorNSP.GUI.WPF.WPFViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SymulatorNSP.Startup
{
    /// <summary>
    /// Interaction logic for LeaderboardSendWindow.xaml
    /// </summary>
    public partial class LeaderboardSendWindow : MetroWindow
    {
        public LeaderboardRecordViewModel ViewModel { get; set; } = null;
        public SymulatorNSP.GUI.Core.ViewModels.QueryViewModel Query { get; set; } = null;

        public LeaderboardSendWindow()
        {
            InitializeComponent();
        }

        public LeaderboardSendWindow(SymulatorNSP.GUI.Core.ViewModels.QueryViewModel queryViewModel)
        {
            this.Query = queryViewModel;
            this.ViewModel = new LeaderboardRecordViewModel(new LeaderboardRecord()
            {
                Source = LeaderboardRecord.eSource.Windows,
                QueryCount = this.Query.QueriedRecordsCount,
                StartTime = this.Query.StartTime,
                EndTime = this.Query.EndTime,
                Factor = GUS.CalculateTimesFasterFactor((this.Query.EndTime - this.Query.StartTime), this.Query.QueriedRecordsCount) ?? 0

            },
            new Action<string, string>((title, message) =>
            {
                var window = new YesMessageWindow(new YesMessageWindow.YesMessageWindowViewModel(title,message));
                window.ShowDialog();
            }));
            this.ViewModel.CancelCommand = new GUI.WPF.Commands.RelayCommand((o) => { this.Close(); });
            this.DataContext = ViewModel;
            InitializeComponent();
        }
    }
}
