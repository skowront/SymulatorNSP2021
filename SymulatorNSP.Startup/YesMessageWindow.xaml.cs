using MahApps.Metro.Controls;
using SymulatorNSP.GUI.Core.ViewModels;
using SymulatorNSP.GUI.WPF.Commands;
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
    /// Interaction logic for YesMessageWindow.xaml
    /// </summary>
    public partial class YesMessageWindow : MetroWindow
    {


        public YesMessageWindow()
        {
            var vm = new YesMessageWindowViewModel(string.Empty, string.Empty);
            vm.ConfirmCommand = new RelayCommand((_) => {
                this.Close(); 
            });
            this.DataContext = vm;
            InitializeComponent();
        }

        public YesMessageWindow(YesMessageWindowViewModel vm)
        {
            vm.ConfirmCommand = new RelayCommand((_) => {
                this.Close();
            });
            this.DataContext = vm;
            this.DataContext = vm;
            InitializeComponent();
        }

        public class YesMessageWindowViewModel:BaseViewModel
        {
            private string title = string.Empty;
            public string Title
            {
                get { return this.title; }
                set { this.title = value; this.OnPropertyChanged(); }
            }

            private string message = string.Empty;
            public string Message
            {
                get { return this.message; }
                set { this.message = value; this.OnPropertyChanged(); }
            }

            public RelayCommand ConfirmCommand { get; set; } = new RelayCommand((_) => 
            {
                return; 
            });

            public YesMessageWindowViewModel(string Title, string Message)
            {
                this.title = Title;
                this.Message = Message;
            }
        }
    }
}
