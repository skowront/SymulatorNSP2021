using SymulatorNSP.GUI.WPF;
using SymulatorNSP.Startup.Config;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SymulatorNSP.Startup
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, IBaseProxyUriProvider
    {
        public string GetBaseProxyURI()
        {
            return ProxyConfig.ProxyBaseURI;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-EN");
            CultureInfo.DefaultThreadCurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-EN");
        }
    }
}
