using SymulatorNSP.Core;
using SymulatorNSP.GUI.Core.Resources;
using SymulatorNSP.GUI.WPF.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SymulatorNSP.Client.Shared.Services.Implementations;
using System.ComponentModel.DataAnnotations;
using System.Windows.Automation;
using System.Runtime.Versioning;

namespace SymulatorNSP.GUI.WPF.WPFViewModels
{
    public class LeaderboardRecordViewModel :SymulatorNSP.GUI.Core.ViewModels.LeaderboardRecordViewModel
    {
        public RelayCommand CancelCommand { get; set; } = new RelayCommand((_) => { });
        public RelayCommand ShareCommand { get; set; } = new RelayCommand((_) => { });

        public Action<string, string> MessageCallback = new Action<string, string>((t, m) => { });

        private string key = string.Empty;
        public string Key
        {
            get { return this.key; }
            set { this.key = value; this.OnPropertyChanged(); }
        }

        public string CongratulationsText
        {
            get { return SymulatorNSP.GUI.Core.ViewModels.GUSViewModel.GetComparisonString(this.Model) ?? Resource.Error; }
        }

        public LeaderboardRecordViewModel()
        {
            this.Model.Source = LeaderboardRecord.eSource.Windows;
        }

        public LeaderboardRecordViewModel(LeaderboardRecord model, Action<string, string> MessageCallback) : base(model)
        {
            this.MessageCallback = MessageCallback;
            this.Model.Source = LeaderboardRecord.eSource.Windows;
            this.ShareCommand = new RelayCommand(async (_) =>
            {
                try
                {
                    if (this.Key == string.Empty || this.Key == null)
                    {
                        this.MessageCallback?.Invoke(Resource.Error, SymulatorNSP.Core.Resources.Resource.eChangeResultKeyEmptyExplanation);
                        return;
                    }
                    if (this.QueryCount < 1000000)
                    {
                        this.MessageCallback?.Invoke(Resource.Error, SymulatorNSP.Core.Resources.Resource.eChangeResultTooFewRecordsExplanation);
                        return;
                    }

                    var proxyHttpClient = new HttpClient() { BaseAddress = new Uri(((IBaseProxyUriProvider)Application.Current).GetBaseProxyURI()) };
                    var httpClient = new HttpClient();
                    var leaderboardService = new LeaderboardService(httpClient, new ConfigService(proxyHttpClient));

                    var result = await leaderboardService.AddEntry(new Tuple<LeaderboardRecord, string>(this.Model, this.Key));
                    switch (result)
                    {
                        case LeaderboardRecord.eChangeResult.Success:
                            this.MessageCallback?.Invoke(Resource.Success, SymulatorNSP.Core.Resources.Resource.eChangeResultSuccessExplanation);
                            break;
                        case LeaderboardRecord.eChangeResult.WorseFactor:
                            this.MessageCallback?.Invoke(Resource.Error, SymulatorNSP.Core.Resources.Resource.eChangeResultWorseFactorExplanation);
                            break;
                        case LeaderboardRecord.eChangeResult.TooFewRecords:
                            this.MessageCallback?.Invoke(Resource.Error, SymulatorNSP.Core.Resources.Resource.eChangeResultTooFewRecordsExplanation);
                            break;
                        case LeaderboardRecord.eChangeResult.KeyEmpty:
                            this.MessageCallback?.Invoke(Resource.Error, SymulatorNSP.Core.Resources.Resource.eChangeResultKeyEmptyExplanation);
                            break;
                        case LeaderboardRecord.eChangeResult.WrongKey:
                            this.MessageCallback?.Invoke(Resource.Error, SymulatorNSP.Core.Resources.Resource.eChangeResultWrongKeyExplanation);
                            break;
                        default:
                            this.MessageCallback?.Invoke(Resource.Error, Resource.Unknown);
                            break;
                    }
                }
                catch (Exception)
                {

                    throw;
                }

            });
        }
    }
}
