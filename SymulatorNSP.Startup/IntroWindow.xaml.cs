using MahApps.Metro.Controls;
using SymulatorNSP.GUI.Core.FamFamFamFlags;
using SymulatorNSP.GUI.WPF.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
    /// Interaction logic for IntroWindow.xaml
    /// </summary>
    public partial class IntroWindow : MetroWindow
    {
        public string language = "en-EN";
        public string Language
        {
            get
            {
                return this.language;
            }
            set
            {
                this.language = value;
            }
        }

        public CountryData CountryLanguage
        {
            get
            {
                var country = CountryData.AllCountries.Where((c) => { return c.Iso2.ToUpper() == this.Language.ToUpper(); }).FirstOrDefault(
                    CountryData.AllCountries.Where((c) => { return c.Iso2.ToUpper() == "GB"; }).First());
                return country;
            }

            set
            {
                this.Language = value.Iso2;
                WPFLocalizeExtension.Engine.LocalizeDictionary.Instance.SetCurrentThreadCulture = true;
                WPFLocalizeExtension.Engine.LocalizeDictionary.Instance.Culture = System.Globalization.CultureInfo.CreateSpecificCulture(this.Language);
            }
        }

        public string Version
        {
            get { return Assembly.GetExecutingAssembly()?.GetName()?.Version?.ToString() ?? string.Empty; }
        }

        public string Copyright
        {
            get 
            {
                var versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly()?.Location?? string.Empty);
                return versionInfo?.LegalCopyright?? string.Empty;
            }
        }

        public RelayCommand SimpleRunCommand { get; set; } = new RelayCommand(async (o) => { var window = new SimpleWindow(); window.Show(); });
        public RelayCommand AdvancedRunCommand { get; set; } = new RelayCommand(async (o) => { var window = new MainWindow(); window.Show(); });

        public IntroWindow()
        {
            this.DataContext = this;
            InitializeComponent();
        }

        private void GithubButton_Click(object sender, RoutedEventArgs e)
        {
            var destinationurl = "https://www.github.com/skowront/symulatornsp";
            var sInfo = new System.Diagnostics.ProcessStartInfo(destinationurl)
            {
                UseShellExecute = true,
            };
            System.Diagnostics.Process.Start(sInfo);
        }
    }
}
