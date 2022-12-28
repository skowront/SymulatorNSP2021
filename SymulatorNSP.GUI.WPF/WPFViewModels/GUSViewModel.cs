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
using System.Reflection;
using System.Diagnostics;
using SymulatorNSP.GUI.Core.ViewModels;

namespace SymulatorNSP.GUI.WPF.WPFViewModels
{
    public class GUSViewModel : SymulatorNSP.GUI.Core.ViewModels.GUSViewModel
    {
        public RelayCommand GenerateCommand { get; set; }
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand LoadCommand { get; set; }
        public RelayCommand ToggleTransparencyModeCommand { get; set; }
        public RelayCommand LogPopulationSampleCommand { get; set; }
        public RelayCommand ClearLogsCommand { get; set; }
        public RelayCommand ExecuteNationalityQuery { get; set; }
        public RelayCommand SelectRandomPersonCommand { get; set; }
        public RelayCommand ShareResultCommand { get; set; }
        public RelayCommand OpenLeaderboardCommand { get; set; }

        public Action<SymulatorNSP.GUI.Core.ViewModels.QueryViewModel, GUSViewModel> OnQueryPerformed { get; set; } = new Action<SymulatorNSP.GUI.Core.ViewModels.QueryViewModel, GUSViewModel>((q, g) => { return; });
        public Action<SymulatorNSP.GUI.Core.ViewModels.QueryViewModel> OnResultSend { get; set; } = new Action<SymulatorNSP.GUI.Core.ViewModels.QueryViewModel>((q) => { return; });


        public string SelectedDistanceCalculator
        {
            get
            {
                return this.DistanceCalculator.Name;
            }
            set
            {
                this.DistanceCalculator = this.model.AvailableDistanceCalculators.Where((c) => { return c.Name == value; }).FirstOrDefault(this.model.AvailableDistanceCalculators.First());
            }
        }

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
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(AvailableGenerationConfigurations));
                this.OnPropertyChanged(nameof(AvailableNationalities));
            }
        }

        public string Version
        {
            get { return Assembly.GetEntryAssembly()?.GetName()?.Version?.ToString() ?? string.Empty; }
        }

        public string Copyright
        {
            get
            {
                var versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly()?.Location ?? string.Empty);
                return versionInfo.LegalCopyright ?? string.Empty;
            }
        }

        public GUSViewModel() : base()
        {
            this.GenerateCommand = new RelayCommand(async (o) =>
            {
                this.IsNotGenerating = false;
                await Task.Delay(TimeSpan.FromMilliseconds(10));
                await this.GenerateAsync(this.DesiredPopulationCount, 1000000,
                    new Progress<SymulatorNSP.Core.ProgressValue>((progress) =>
                    {
                        var progressVal = 0;
                        if (progress.StepReport == false)
                        {
                            if ((progress.Current) % (0.1 * progress.End) == 0 || progress.Current == progress.End)
                            {
                                progressVal = (int)(progress.Current / (progress.End - progress.Start) * 100);
                                this.AddLogEntry(String.Format(Resource.CreationProgressUpdate, progress.Current, progress.End));
                            }
                        }
                        else
                        {
                            progressVal = (int)(progress.Current / (progress.End - progress.Start) * 100);
                            this.AddLogEntry(String.Format(Resource.CreationProgressUpdate, progress.Current, progress.End));
                        }
                    }));
                this.IsNotGenerating = true;
                this.PopulationGenerated = true;
            });

            this.ClearLogsCommand = new RelayCommand((o) =>
            {
                this.ClearLogs();
            });

            this.ExecuteNationalityQuery = new RelayCommand((o) =>
            {
                this.ExecuteQueryNationalityCount();
                this.OnQueryPerformed?.Invoke(this.NationalityQuery, this);
                this.QueryExecuted = true;
            });

            this.SelectRandomPersonCommand = new RelayCommand((o) =>
            {
                this.SelectRandomPerson();
            });

            this.SaveCommand = new RelayCommand((o) =>
            {
                try
                {
                    var dialog = new SaveFileDialog();
                    dialog.Filter = "Json (*.json)|*.json|Txt (*.txt)|*.txt";
                    dialog.OverwritePrompt = true;
                    dialog.CreatePrompt = false;
                    if (dialog.ShowDialog() == true)
                    {
                        var filePath = dialog.FileName;

                        if (System.IO.File.Exists(filePath))
                        {
                            File.WriteAllText(filePath, JsonConvert.SerializeObject(this.model, Formatting.Indented));
                        }
                        else
                        {
                            File.AppendAllText(filePath, this.model.SerializePopulation());
                        }
                        if (filePath != string.Empty)
                        {
                            this.AddLogEntry(Resource.SaveSuccess);
                        }
                    }
                }
                catch (Exception)
                {
                    this.AddLogEntry(Resource.SaveFailed);
                }
            });

            this.LoadCommand = new RelayCommand((o) =>
            {
                try
                {
                    var dialog = new OpenFileDialog();
                    dialog.Filter = "Json (*.json)|*.json|Txt (*.txt)|*.txt";
                    if (dialog.ShowDialog() == true)
                    {
                        var filePath = dialog.FileName;

                        //Read the contents of the file into a stream
                        var fileStream = dialog.OpenFile();

                        using (StreamReader reader = new StreamReader(fileStream))
                        {
                            string content = reader.ReadToEnd();
                            this.model.DeserializePopulation(content);
                        }
                    }
                    this.AddLogEntry(Resource.LoadSuccess);
                }
                catch (Exception)
                {
                    this.AddLogEntry(Resource.LoadFailed);
                }
            });

            this.ToggleTransparencyModeCommand = new RelayCommand((o) =>
            {
                this.TransparencyMode = !this.TransparencyMode;
                if (TransparencyMode == true) this.AddLogEntry(Resource.TransparencyModeON);
                else this.AddLogEntry(Resource.TransparencyModeON);
            });

            this.LogPopulationSampleCommand = new RelayCommand((o) =>
            {
                this.LogPopulationSample();
            });
            this.ShareResultCommand = new RelayCommand((o) =>
            {
                this.OnResultSend?.Invoke(this.NationalityQuery);
            });

            this.OpenLeaderboardCommand = new RelayCommand((o) =>
            {
                string url = "http://symulatornsp2021.regios.org.pl//leaderboard";
                Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
            });
        }
    }
}
