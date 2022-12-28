using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SymulatorNSP.Core;
using SymulatorNSP.GUI.Core.Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace SymulatorNSP.GUI.Core.ViewModels
{
    public class GUSViewModel : BaseViewModel
    {
        public GUS model = new();

        private int desiredPopulationCount = 0;

        private int virtualCount = 0;
        public int VirtualCount
        {
            get { return this.virtualCount; }
            set { this.virtualCount = value; this.OnPropertyChanged(); }
        }

        public static int AvailableThreads
        {
            get
            {
                return Environment.ProcessorCount*4;
            }
        }

        public static List<int> AvailableThreadsList
        {
            get
            {
                List<int> list = new();
                for (int i = 1; i <= AvailableThreads; i++)
                {
                    list.Add(i);
                }
                return list;
            }
        }

        public int requestedThreads = 1;
        public int RequestedThreads
        {
            get { return this.requestedThreads; }
            set { this.requestedThreads = value; this.OnPropertyChanged(); }
        }

        private bool transparencyMode = false;
        public bool TransparencyMode
        {
            get { return this.transparencyMode; }
            set { this.transparencyMode = value; this.OnPropertyChanged(); }
        }

        [Range(0, 100000000, ErrorMessageResourceName = nameof(DesiredPopulationCount) + "RangeError", ErrorMessageResourceType = typeof(Resource))]
        public int DesiredPopulationCount
        {
            get { return desiredPopulationCount; }
            set { this.desiredPopulationCount = value; this.OnPropertyChanged(); }
        }

        private string log = string.Empty;
        public string Log
        {
            get { return this.log; }
            set { this.log = value; this.OnPropertyChanged(); }
        }

        private double populationSampleLogCount = 0.0f;
        [Range(0, 100)]
        public double PopulationSampleLogCount
        {
            get { return populationSampleLogCount; }
            set { populationSampleLogCount = value; this.OnPropertyChanged(); }
        }

        public double NationalityErrorRate
        {
            get { return this.model.GenerationConfiguration.NationalityErrorRate; }
            set { this.model.GenerationConfiguration.NationalityErrorRate = value; this.OnPropertyChanged(); this.OnPropertyChanged(nameof(GenerationConfiguration)); }
        }
        public int NationalityMaxImpact
        {
            get { return this.model.GenerationConfiguration.NationalityErrorRateMaxImpact; }
            set { this.model.GenerationConfiguration.NationalityErrorRateMaxImpact = value; this.OnPropertyChanged(); this.OnPropertyChanged(nameof(GenerationConfiguration)); }
        }

        public DistanceCalculator DistanceCalculator
        {
            get { return this.model.DistanceCalculator; }
            set { this.model.DistanceCalculator = value; this.OnPropertyChanged(); }
        }

        private bool populationGenerated = false;
        public bool PopulationGenerated
        {
            get { return populationGenerated; }
            set { populationGenerated = value; this.OnPropertyChanged(); }
        }

        private bool queryExecuted = false;
        public bool QueryExecuted
        {
            get { return queryExecuted; }
            set { queryExecuted = value; this.OnPropertyChanged(); }
        }

        public List<string> AvailableDistanceCalculators
        {
            get { return model.AvailableDistanceCalculators.Select((p) => { return p.Name; }).ToList(); }
        }

        public List<string> AvailableGenerationConfigurations
        {
            get { return model.AvailableGenerationConfigurations; }
        }

        public string GenerationConfiguration
        {
            get { return this.model.GenerationConfigurationString; }
            set { try { this.model.GenerationConfigurationString = value; this.OnPropertyChanged();
                    this.OnPropertyChanged(nameof(NationalityErrorRate));
                    //this.OnPropertyChanged(nameof(EthnicityErrorRate));
                    this.OnPropertyChanged(nameof(NationalityMaxImpact));
                    //this.OnPropertyChanged(nameof(EthnicityMaxImpact));
                    this.OnPropertyChanged(nameof(AvailableNationalities));
                } catch (Exception ex) { this.OnException?.Invoke(ex); } }
        }

        public List<string> AvailableNationalities
        {
            get
            {
                var l = this.model.GenerationConfiguration.Nationalities.Select((s) => { return s.Value; }).ToList();
                l.Add(SymulatorNSP.Core.Resources.Resource.Unrecognized);
                return l;
            }
        }

        private string randomPerson = string.Empty;
        public string RandomPerson
        {
            get { return randomPerson; }
            set { this.randomPerson = value; this.OnPropertyChanged(); }
        }

        private string selectedGenerationConfigurationDescription = Person.GenerationConfiguration.ConfigurationDescriptions[Person.GenerationConfiguration.Configurations.First().Key];
        public string SelectedGenerationConfigurationDescription
        {
            get
            {
                return selectedGenerationConfigurationDescription;
            }
            set
            {
                this.selectedGenerationConfigurationDescription = value;
                this.OnPropertyChanged();
            }
        }

        private string selectedGenerationConfiguration = Person.GenerationConfiguration.Configurations.First().Key;
        public string SelectedGenerationConfiguration
        {
            get { return selectedGenerationConfiguration; }
            set
            {
                selectedGenerationConfiguration = value;
                this.GenerationConfiguration = JsonConvert.SerializeObject(Person.GenerationConfiguration.Configurations[value]);
                this.SelectedGenerationConfigurationDescription = Person.GenerationConfiguration.ConfigurationDescriptions[value];
                this.OnPropertyChanged();
            }
        }

        private bool isNotGenerating = true;
        public bool IsNotGenerating
        {
            get { return this.isNotGenerating; }
            set { this.isNotGenerating = value; this.OnPropertyChanged(); }
        }

        private QueryViewModel nationalityQuery = new();
        public QueryViewModel NationalityQuery
        {
            get { return this.nationalityQuery; }
            set { this.nationalityQuery = value; this.OnPropertyChanged(); }
        }
        public Action<FittingResult> FitCallback
        {
            get
            {
                return new Action<FittingResult>((o) =>
                {
                    if (o.Success == true)
                    {
                        this.AddLogEntry(string.Format(Resource.FitSuccess, o.Original, o.Fit, o.Distance, o.Algorithm));
                    }
                    else
                    {
                        var s = Resource.FitFailure;
                        this.AddLogEntry(string.Format(Resource.FitFailure, o.Original, o.Fit, o.Distance, o.Algorithm));
                    }
                });
            }
        }

        public Action<Exception>? OnException { get; set; } = null; 

        public GUSViewModel(GUS? model = null)
        {
            this.model = model ?? new GUS();
            this.NationalityQuery.Value = this.AvailableNationalities.FirstOrDefault(string.Empty);
        }

        public void Generate(int count, Action<ProgressValue>? OnProgressChanged = null)
        {
            this.model.Generate(count, OnProgressChanged);
        }

        public async Task GenerateAsync(int count, int step, IProgress<ProgressValue>? OnProgressChanged = null)
        {
            await this.model.GenerateAsync(count, step, OnProgressChanged);
        }

        public int QueryNationalityCount(string queriedValueInput, Action<FittingResult>? fitCallback, int requestedThreads)
        {
            return this.model.QueryNationalityCount(queriedValueInput, fitCallback, requestedThreads);
        }

        public void ExecuteQueryNationalityCount()
        {
            this.NationalityQuery.StartTime = DateTime.Now;
            this.NationalityQuery.Result = this.QueryNationalityCount(this.NationalityQuery.Value.ToLower(), this.TransparencyMode ? this.FitCallback : null, requestedThreads).ToString() + " | " + this.model.Population.People.Count.ToString();
            this.NationalityQuery.EndTime = DateTime.Now;
            this.NationalityQuery.QueriedRecordsCount = this.model.Population.People.Count;
            this.NationalityQuery.ExecutionTime = this.NationalityQuery.EndTime - this.NationalityQuery.StartTime;
        }
        public virtual void AddLogEntry(string value)
        {
            if (this.Log == string.Empty)
            {
                this.Log = DateTime.Now + ": " + value;
            }
            else
            {
                this.Log = DateTime.Now + ": " + value + "\n" + this.Log;
            }
        }

        public void ClearLogs()
        {
            this.Log = string.Empty;
        }

        public string SerializePopulation()
        {
            return this.model.SerializePopulation();
        }

        public void DeserializePopulation(string serializedPopulation)
        {
            this.model.DeserializePopulation(serializedPopulation);
        }

        public void LogPopulationSample()
        {
            var count = this.PopulationSampleLogCount;
            if (count > 1000)
            {
                this.AddLogEntry(Resource.QueryLimitThousandSamples);
                return;
            }
            for (int i = 0; i < count && i < this.model.Population.People.Count; i++)
            {
                this.AddLogEntry(JsonConvert.SerializeObject(this.model.Population.People[i]));
            }
        }

        public void SelectRandomPerson()
        {
            if (this.model.Population.People.Count > 0)
            {
                var rand = new Random();
                var id = rand.Next(0, this.model.Population.People.Count-1);
                var person = this.model.Population.People.ElementAt(id);
                this.RandomPerson = string.Format(Resource.PersonStringified, person.Name, person.Surname, person.Address, person.Religion, person.Nationality, /*person.Ethnicity,*/ person.Job);
            }
            else
            {
                this.AddLogEntry(Resource.GeneratePopulationFirst);
            }
        }

        public Person? GetRandomPerson()
        {
            if (this.model.Population.People.Count > 0)
            {
                var rand = new Random();
                var id = rand.Next(0, this.model.Population.People.Count - 1);
                return this.model.Population.People.ElementAt(id);
            }
            else
            {
                this.AddLogEntry(Resource.GeneratePopulationFirst);
                return null;
            }
        }

        public async Task<int> VirtualCountingNationality(string queryValue, int expectedNumberOfRecords, Action<int> callback, Action<FittingResult>? fittingCallback=null)
        {
            this.IsNotGenerating = false;
            var todo = expectedNumberOfRecords;
            var count = 0;
            var iterations = 0;
            var queriedValue = queryValue;
            var options = this.model.GenerationConfiguration.Nationalities.Select((s) => { return s.Value; }).ToList();

            this.NationalityQuery.StartTime = DateTime.Now;

            while (todo - this.model.Population.People.Count > 0)
            {
                count += this.model.Population.People.Take(todo).Where((r) => { return queryValue.ToLower() == this.model.TryFit(r.Nationality, options, "??", fittingCallback).ToLower(); }).Count();
                todo -= this.model.Population.People.Count;
                if(this.model.Population.People.Count<10000)
                {
                    if(iterations==1000)
                    {
                        iterations = 0;
                        callback?.Invoke(todo);
                        await Task.Delay(TimeSpan.FromMilliseconds(1));
                    }
                }
                else
                {
                    callback?.Invoke(todo);
                    await Task.Delay(TimeSpan.FromMilliseconds(1));
                }
                iterations++;
            }
            count += this.model.Population.People.Take(todo).Where((r) => { return queryValue.ToLower() == this.model.TryFit(r.Nationality, options, "??", null).ToLower(); }).Count();
            this.NationalityQuery.EndTime = DateTime.Now;
            this.NationalityQuery.QueriedRecordsCount = expectedNumberOfRecords;
            this.NationalityQuery.ExecutionTime = this.NationalityQuery.EndTime - this.NationalityQuery.StartTime;
            this.IsNotGenerating = true;   
            return count;
        }

        public static string? GetComparisonString(QueryViewModel query)
        {
            double popcount = query.QueriedRecordsCount;
            var yourTime = query.ExecutionTime;
            var GUScountingTime = GUS.GUSRealCountingTime();
            var timesFaster = GUS.CalculateTimesFasterFactor(query.ExecutionTime, popcount);
            if (timesFaster == null)
                return null;
            return string.Format(Resource.RealLifeComparisonStringCotent, popcount, yourTime.TotalSeconds, GUScountingTime.TotalSeconds, timesFaster);
        }

        public static string? GetComparisonString(LeaderboardRecord leaderboardRecord)
        {
            double popcount = leaderboardRecord.QueryCount;
            var yourTime = leaderboardRecord.ExecutionTime();
            var GUScountingTime = GUS.GUSRealCountingTime();
            var timesFaster = GUS.CalculateTimesFasterFactor(leaderboardRecord.ExecutionTime(), popcount);
            if (timesFaster == null)
                return null;
            return string.Format(Resource.RealLifeComparisonStringCotent, popcount, yourTime.TotalSeconds, GUScountingTime.TotalSeconds, timesFaster);
        }
    }
}
