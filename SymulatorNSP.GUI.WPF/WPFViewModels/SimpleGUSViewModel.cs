using SymulatorNSP.GUI.WPF.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics.PerformanceData;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.IO;
using Newtonsoft.Json;
using SymulatorNSP.GUI.WPF;
using SymulatorNSP.Core;
using System.Dynamic;
using SymulatorNSP.GUI.Core.Resources;
using SymulatorNSP.GUI.Core.FamFamFamFlags;
using System.Collections;
using System.Windows.Controls;
using SymulatorNSP.GUI.Core.ViewModels;

namespace SymulatorNSP.GUI.WPF.WPFViewModels
{
    public class SimpleGUSViewModel: GUSViewModel
    {
        public SimpleGUSViewModel():base()
        {
            this.DesiredPopulationCount = GUS.DefaultCacheSize;
            this.VirtualCount = GUS.DefaultCacheSize;
            this.ExecuteNationalityQuery = new RelayCommand(async (o) =>
            {
                this.NationalityQuery.Result = (await this.VirtualCountingNationality(NationalityQuery.Value.ToLower(), this.VirtualCount, null)).ToString() + " | " + this.VirtualCount;
                this.OnQueryPerformed?.Invoke(this.NationalityQuery, this);
                this.QueryExecuted = true;
            });
        }
        public SimpleGUSViewModel(int virtualizedCount):base()
        {
            this.DesiredPopulationCount = GUS.DefaultCacheSize;
            this.VirtualCount = virtualizedCount;
            this.ExecuteNationalityQuery = new RelayCommand(async (o) =>
            {
                this.NationalityQuery.Result = (await this.VirtualCountingNationality(NationalityQuery.Value.ToLower(),this.VirtualCount,null)).ToString() + " | " + this.VirtualCount;
                this.OnQueryPerformed?.Invoke(this.NationalityQuery, this);
                this.QueryExecuted = true;
            });
        }
    }
}
