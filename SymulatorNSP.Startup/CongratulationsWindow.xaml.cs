using MahApps.Metro.Controls;
using SymulatorNSP.GUI.Core.Resources;
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
using System.Windows.Shapes;

namespace SymulatorNSP.Startup
{
    /// <summary>
    /// Interaction logic for CongratulationsWindow.xaml
    /// </summary>
    public partial class CongratulationsWindow : MetroWindow
    {
        public string Message { get; set; } = string.Empty;

        public CongratulationsWindow()
        {
            InitializeComponent();
        }

        public CongratulationsWindow(string message)
        {
            this.Message = message;
            this.DataContext = this;
            InitializeComponent();
        }
    }
}
