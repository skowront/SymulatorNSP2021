using Newtonsoft.Json;

namespace SymulatorNSP.Core
{
    public class GUS
    {
        public const int DefaultCacheSize = 100000;
        public Population Population { get; set; } = new Population();
        public int StringDifferenceLimit { get; set; } = 3;
        public DistanceCalculator DistanceCalculator { get; set; } = new DistanceCalculator();
        public Person.GenerationConfiguration GenerationConfiguration { get; set; } = Person.GenerationConfiguration.GetOptimizedGenerationConfiguration();
        public string GenerationConfigurationString
        {
            get
            {
                return JsonConvert.SerializeObject(this.GenerationConfiguration, Formatting.Indented);
            }
            set
            {
                try
                {
                    Person.GenerationConfiguration? generationConfiguration = JsonConvert.DeserializeObject<Person.GenerationConfiguration>(value ?? "", new JsonSerializerSettings()
                    {
                        Formatting = Formatting.Indented,
                    });
                    this.GenerationConfiguration = generationConfiguration ?? throw new Exception();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public List<string> AvailableGenerationConfigurations
        {
            get
            {
                return Person.GenerationConfiguration.Configurations.Keys.ToList();
            }
        }

        public List<DistanceCalculator> AvailableDistanceCalculators { get; set; } = new List<DistanceCalculator>();

        public GUS()
        {
            this.AvailableDistanceCalculators.Add(new DistanceCalculator());
            this.AvailableDistanceCalculators.Add(new LevensteinDistanceCalculator());
            this.AvailableDistanceCalculators.Add(new HammingDistanceCalculator());
        }

        public void Generate(int count, Action<ProgressValue>? OnProgressChanged = null)
        {
            this.Population.Generate(count, this.GenerationConfiguration , OnProgressChanged);
        }

        public async Task GenerateAsync(int count, int step, IProgress<ProgressValue>? OnProgressChanged = null)
        {
            await this.Population.GenerateAsync(count, this.GenerationConfiguration, step, OnProgressChanged);
        }

        public int QueryNationalityCount(string queriedValueInput, Action<FittingResult>? fitCallback, int requestedThreads)
        {
            var queriedValue = queriedValueInput;
            var options = this.GenerationConfiguration.Nationalities.Select((s) => { return s.Value; }).ToList();
            if (requestedThreads == 1)
            {
                var count = this.Population.People.Where((p) => { return queriedValueInput == this.TryFit(p.Nationality, options, Resources.Resource.Unrecognized, fitCallback); }).Count();
                return count;
            }
            else
            {
                return this.Population.People.AsParallel().WithDegreeOfParallelism(requestedThreads).Where((p) => { return queriedValueInput == this.TryFit(p.Nationality, options, Resources.Resource.Unrecognized, fitCallback); }).Count();
                var counts = new List<int>();
                var threads = new List<Thread>();
                var sections = GUS.DivideIndexes(this.Population.People.Count, requestedThreads);
                for(int i = 0; i < sections.Count; i++)
                {
                    counts.Add(0);
                }
                for(int i = 0; i < sections.Count; i++)
                {
                    var thread = new Thread((iterator) =>
                    {
                        var j = (int)(iterator??0);
                        counts[j] += this.Population.People.Take(new Range(sections[j].Item1, sections[j].Item2))
                        .Where((p) => { return queriedValueInput == this.TryFit(p.Nationality, options, Resources.Resource.Unrecognized, fitCallback); }).Count();
                    });
                    threads.Add(thread);
                }
                for(int i = 0; i < sections.Count; i++)
                {
                    threads[i].Start(i);
                }
                foreach(var thread in threads)
                {
                    thread.Join();
                }
                return counts.Sum();
            }
        }

        //public int QueryEthnicityCount(string queriedValueInput, Action<FittingResult> fitCallback, int requestedThreads)
        //{
        //    var queriedValue = queriedValueInput;
        //    var options = this.generationConfiguration.Ethnicities.Select((s) => { return s.Value; }).ToList();
        //    if (requestedThreads == 1)
        //    {
        //        var count = this.Population.People.Where((p) => { return queriedValueInput == this.TryFit(p.Ethnicity, options, Resources.Resource.Unrecognized, fitCallback); }).Count();
        //        return count;
        //    }
        //    else
        //    {
        //        var counts = new List<int>();
        //        var threads = new List<Thread>();
        //        var sections = this.DivideIndexes(this.Population.People.Count, requestedThreads);
        //        for (int i = 0; i < sections.Count; i++)
        //        {
        //            counts.Add(0);
        //        }
        //        for (int i = 0; i < sections.Count; i++)
        //        {
        //            var thread = new Thread((iterator) =>
        //            {
        //                var j = (int)iterator;
        //                counts[j] += this.Population.People.Take(new Range(sections[j].Item1, sections[j].Item2))
        //                .Where((p) => { return queriedValueInput == this.TryFit(p.Ethnicity, options, Resources.Resource.Unrecognized, fitCallback); }).Count();
        //            });
        //            threads.Add(thread);
        //        }
        //        for (int i = 0; i < sections.Count; i++)
        //        {
        //            threads[i].Start(i);
        //        }
        //        foreach (var thread in threads)
        //        {
        //            thread.Join();
        //        }
        //        return counts.Sum();
        //    }
        //}

        //public int QueryNationalityOrEthnicityCount(string queriedValueInput, Action<FittingResult> fitCallback, int requestedThreads)
        //{
        //    var queriedValue = queriedValueInput;
        //    var options = this.generationConfiguration.Nationalities.Select((s) => { return s.Value; }).ToList();
        //    if (requestedThreads == 1)
        //    {
        //        options = options.Concat(this.generationConfiguration.Ethnicities.Select((s) => { return s.Value; }).ToList()).ToList();
        //        var count = this.Population.People.Where((p) => { return queriedValueInput == this.TryFit(p.Nationality, options, Resources.Resource.Unrecognized, fitCallback) || queriedValueInput == this.TryFit(p.Ethnicity, options, Resources.Resource.Unrecognized, fitCallback); }).Count();
        //        return count;
        //    }
        //    else
        //    {
        //        var counts = new List<int>();
        //        var threads = new List<Thread>();
        //        var sections = this.DivideIndexes(this.Population.People.Count, requestedThreads);
        //        for (int i = 0; i < sections.Count; i++)
        //        {
        //            counts.Add(0);
        //        }
        //        for (int i = 0; i < sections.Count; i++)
        //        {
        //            var thread = new Thread((iterator) =>
        //            {
        //                var j = (int)iterator;
        //                counts[j] += this.Population.People.Take(new Range(sections[j].Item1, sections[j].Item2))
        //                .Where((p) => { return queriedValueInput == this.TryFit(p.Nationality, options, Resources.Resource.Unrecognized, fitCallback) || queriedValueInput == this.TryFit(p.Ethnicity, options, Resources.Resource.Unrecognized, fitCallback); }).Count();
        //            });
        //            threads.Add(thread);
        //        }
        //        for (int i = 0; i < sections.Count; i++)
        //        {
        //            threads[i].Start(i);
        //        }
        //        foreach (var thread in threads)
        //        {
        //            thread.Join();
        //        }
        //        return counts.Sum();
        //    }
        //}

        private static List<Tuple<int, int>> DivideIndexes(int count, int sections)
        {
            List<Tuple<int,int>> indexes = new();
            var sectionLength = count / sections;
            for (int i = 0; i < sections; i++)
            {
                indexes.Add(new Tuple<int,int>(i * sectionLength, ((i+1) * sectionLength)));
            }
            var dividedSum = indexes[indexes.IndexOf(indexes.Last())].Item2;
            indexes[indexes.IndexOf(indexes.Last())] = new Tuple<int,int>(indexes[indexes.IndexOf(indexes.Last())].Item1, indexes[indexes.IndexOf(indexes.Last())].Item2 + Math.Abs(count - dividedSum));
            return indexes;
        }

        public string TryFit(string entry, List<string> options, string unknownOption, Action<FittingResult>? fitCallback)
        {
            foreach (var option in options)
            {
                if (entry.ToLower() == option)
                {
                    return option;
                }
            }

            int minDistance = int.MaxValue;
            int minIndex = -1;
            int bestDistance = int.MaxValue;
            int bestIndex = 0;
            for (int i = 0; i < options.Count; i++)
            {
                var distance = this.DistanceCalculator.CalculateDistance(entry.ToLower(), options[i]);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    minIndex = i;
                }
                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    bestIndex = i;
                }
            }
            if (minDistance < StringDifferenceLimit)
            {
                fitCallback?.Invoke(new FittingResult() { Algorithm = this.DistanceCalculator.Name, Distance = minDistance, Original = entry, Fit = options[minIndex], Success = true });
                return options[minIndex];
            }
            if (bestDistance != int.MaxValue)
                fitCallback?.Invoke(new FittingResult() { Algorithm = this.DistanceCalculator.Name, Distance = minDistance, Original = entry, Fit = options[bestIndex], Success = false });
            return unknownOption;
        }

        public string SerializePopulation()
        {
            return JsonConvert.SerializeObject(this.Population, Formatting.Indented);
        }

        public void DeserializePopulation(string serialized)
        {
            JsonConvert.PopulateObject(serialized, this.Population);
        }

        public static double? CalculateTimesFasterFactor(TimeSpan ExecutionTime, double population)
        {
            const double totalGUSPopulation = 38036100;
            if (ExecutionTime.TotalMilliseconds == 0 || totalGUSPopulation == 0)
                return null;
            var GUScountingTime = GUSRealCountingTime();
            return (GUScountingTime.TotalMilliseconds / ExecutionTime.TotalMilliseconds) * (population / totalGUSPopulation);
        }

        public static TimeSpan GUSRealCountingTime()
        {
            return (new DateTime(2022, 11, 30)) - (new DateTime(2021, 09, 30));
        }
    }
}
