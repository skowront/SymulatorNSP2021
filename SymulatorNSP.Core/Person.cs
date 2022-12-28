using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;

namespace SymulatorNSP.Core
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Religion { get; set; } = string.Empty;
        public string Nationality { get; set; } = string.Empty;
        //public string Ethnicity { get; set; } = string.Empty;
        public string Job { get; set; } = string.Empty;

        public void Generate(GenerationConfiguration generationConfiguration)
        {
            var gc = generationConfiguration;
            if (gc.Names.Count == 0 || gc.Surnames.Count == 0 || gc.Addresses.Count == 0 ||
                gc.Religions.Count == 0 || gc.Nationalities.Count == 0 || /*gc.Ethnicities.Count == 0 ||*/
                gc.Jobs.Count == 0) return;
            Random random = new Random();
            double n = 0.0;
            var r0 = 0.0;
            if (gc.Names.FirstOrDefault(new RandomItem<string>(string.Empty, 1.0)).Probability != 1.0)
            {
                n = random.NextDouble();
                foreach (var item in gc.Names) if (n >= r0 && n <= r0 + item.Probability) { this.Name = item.Value; break; } else if (r0 > 1.0) { throw new Exception(); } else { r0 += item.Probability; }
            }
            if (this.Name == string.Empty) this.Name = gc.Names.First().Value;

            if (gc.Surnames.FirstOrDefault(new RandomItem<string>(string.Empty, 1.0)).Probability != 1.0)
            {
                n = random.NextDouble();
                r0 = 0.0;
                foreach (var item in gc.Surnames) if (n >= r0 && n <= r0 + item.Probability) { this.Surname = item.Value; break; } else if (r0 > 1.0) { throw new Exception(); } else { r0 += item.Probability; }
            }
            if (this.Surname == string.Empty) this.Surname = gc.Surnames.First().Value;

            if (gc.Addresses.FirstOrDefault(new RandomItem<string>(string.Empty, 1.0)).Probability != 1.0)
            {
                n = random.NextDouble();
                r0 = 0.0;
                foreach (var item in gc.Addresses) if (n >= r0 && n <= r0 + item.Probability) { this.Address = item.Value; break; } else if (r0 > 1.0) { throw new Exception(); } else { r0 += item.Probability; }
            }
            if (this.Address == string.Empty) this.Address = gc.Addresses.First().Value;

            if (gc.Religions.FirstOrDefault(new RandomItem<string>(string.Empty, 1.0)).Probability != 1.0)
            {
                n = random.NextDouble();
                r0 = 0.0;
                foreach (var item in gc.Religions) if (n >= r0 && n <= r0 + item.Probability) { this.Religion = item.Value; break; } else if (r0 > 1.0) { throw new Exception(); } else { r0 += item.Probability; }
            }
            if (this.Religion == string.Empty) this.Religion = gc.Religions.First().Value;

            if (gc.Nationalities.FirstOrDefault(new RandomItem<string>(string.Empty, 1.0)).Probability != 1.0)
            {
                n = random.NextDouble();
                r0 = 0.0;
                foreach (var item in gc.Nationalities) if (n >= r0 && n <= r0 + item.Probability) { this.Nationality = item.Value; break; } else if (r0 > 1.0) { throw new Exception(); } else { r0 += item.Probability; }
                if (this.Nationality == string.Empty) this.Nationality = gc.Nationalities.First().Value;
                n = random.NextDouble();
                if (n < gc.NationalityErrorRate)
                {
                    int impact = random.Next(0, gc.NationalityErrorRateMaxImpact);
                    List<int> indexesToChange = new List<int>();
                    for (int i = 0; i < impact; i++)
                    {
                        indexesToChange.Add(random.Next(0, this.Nationality.Length));
                    }
                    foreach (var index in indexesToChange)
                    {
                        var changeOrSwap = 0;
                        if (this.Nationality.Length > 2)
                            changeOrSwap = random.Next(0, 2);
                        switch (changeOrSwap)
                        {
                            case 0:
                                if (index == this.Nationality.Length)
                                {
                                    this.Nationality += " ";
                                }
                                StringBuilder sb = new StringBuilder(this.Nationality);
                                sb[index] = (char)((int)random.NextInt64('a', 'z'));
                                this.Nationality = sb.ToString();
                                break;
                            case 1:
                                StringBuilder sb2 = new StringBuilder(this.Nationality);
                                var temp = sb2[0];
                                sb2[0] = sb2[1];
                                sb2[1] = temp;
                                this.Nationality = sb2.ToString();
                                break;
                        }
                    }
                }
            }
            if (this.Nationality == string.Empty) this.Nationality = gc.Nationalities.First().Value;

            //if (gc.Ethnicities.FirstOrDefault(new RandomItem<string>(string.Empty, 1.0)).Probability != 1.0)
            //{
            //    n = random.NextDouble();
            //    r0 = 0.0;
            //    foreach (var item in gc.Ethnicities) if (n >= r0 && n <= r0 + item.Probability) { this.Ethnicity = item.Value; break; } else if (r0 > 1.0) { throw new Exception(); } else { r0 += item.Probability; }
            //    if (this.Ethnicity == string.Empty) this.Ethnicity = gc.Ethnicities.First().Value;
            //    n = random.NextDouble();
            //    if (n < gc.EthnicityErrorRate)
            //    {
            //        int impact = random.Next(0, gc.EthnicityErrorRateMaxImpact);
            //        List<int> indexesToChange = new List<int>();
            //        for (int i = 0; i < impact; i++)
            //        {
            //            indexesToChange.Add(random.Next(0, this.Ethnicity.Length));
            //        }
            //        foreach (var index in indexesToChange)
            //        {

            //            var changeOrSwap = 0;
            //            if (this.Ethnicity.Length > 2)
            //                changeOrSwap = random.Next(0, 2);
            //            switch (changeOrSwap)
            //            {
            //                case 0:
            //                    if (index == this.Ethnicity.Length)
            //                    {
            //                        this.Ethnicity += " ";
            //                    }
            //                    StringBuilder sb = new StringBuilder(this.Ethnicity);
            //                    sb[index] = (char)((int)random.NextInt64('a', 'z'));
            //                    this.Ethnicity = sb.ToString();
            //                    break;
            //                case 1:
            //                    StringBuilder sb2 = new StringBuilder(this.Ethnicity);
            //                    var temp = sb2[0];
            //                    sb2[0] = sb2[1];
            //                    sb2[1] = temp;
            //                    this.Ethnicity = sb2.ToString();
            //                    break;
            //            }
            //        }
            //    }
            //}
            //if (this.Ethnicity == string.Empty) this.Ethnicity = gc.Ethnicities.First().Value;

            if (gc.Jobs.FirstOrDefault(new RandomItem<string>(string.Empty, 1.0)).Probability != 1.0)
            {
                n = random.NextDouble();
                r0 = 0.0;
                foreach (var item in gc.Jobs) if (n >= r0 && n <= r0 + item.Probability) { this.Job = item.Value; break; } else if (r0 > 1.0) { throw new Exception(); } else { r0 += item.Probability; }
            }
            if (this.Job == string.Empty) this.Job = gc.Jobs.First().Value;
        }

        public async Task GenerateAsync(GenerationConfiguration generationConfiguration)
        {
            await Task.Run(() => { this.Generate(generationConfiguration); });
            return;
        }

        public class GenerationConfiguration
        {
            public List<RandomItem<string>> Names { get; set; } = new List<RandomItem<string>>();
            public List<RandomItem<string>> Surnames { get; set; } = new List<RandomItem<string>>();
            public List<RandomItem<string>> Addresses { get; set; } = new List<RandomItem<string>>();
            public List<RandomItem<string>> Religions { get; set; } = new List<RandomItem<string>>();
            public List<RandomItem<string>> Nationalities { get; set; } = new List<RandomItem<string>>();
            public double NationalityErrorRate { get; set; } = 0.0;
            public int NationalityErrorRateMaxImpact { get; set; } = 0;
            //public List<RandomItem<string>> Ethnicities { get; set; } = new List<RandomItem<string>>();
            //public double EthnicityErrorRate { get; set; } = 0.0;
            //public int EthnicityErrorRateMaxImpact { get; set; } = 0;
            public List<RandomItem<string>> Jobs { get; set; } = new List<RandomItem<string>>();

            #region Defaults
            public static Dictionary<string, GenerationConfiguration> Configurations
            { 
                get
                {
                    return new Dictionary<string, GenerationConfiguration>() {
                    { Resources.Resource.OptimizedGenerationConfiguration, GetOptimizedGenerationConfiguration() },
                    { Resources.Resource.SimpleGenerationConfiguration, GetSimpleGenerationConfiguration() },
                    { Resources.Resource.ComplexGenerationConfiguration, GetComplexGenerationConfiguration() } };
                }
            }

            public static Dictionary<string, string> ConfigurationDescriptions
            {
                get
                {
                    return new Dictionary<string, string>() {
                    { Resources.Resource.OptimizedGenerationConfiguration, Resources.Resource.OptimizedGenerationConfigurationDescription },
                    { Resources.Resource.SimpleGenerationConfiguration, Resources.Resource.SimpleGenerationConfigurationDescription },
                    { Resources.Resource.ComplexGenerationConfiguration, Resources.Resource.ComplexGenerationConfigurationDescription } };
                }
            }

            public static GenerationConfiguration GetComplexGenerationConfiguration()
            {
                var gc = new GenerationConfiguration();
                #region Names
                gc.Names.Add(new RandomItem<string>("Jan", 0.05));
                gc.Names.Add(new RandomItem<string>("Anna", 0.05));
                gc.Names.Add(new RandomItem<string>("Richard", 0.05));
                gc.Names.Add(new RandomItem<string>("Maciej", 0.05));
                gc.Names.Add(new RandomItem<string>("Mariusz", 0.05));
                gc.Names.Add(new RandomItem<string>("Aneta", 0.05));
                gc.Names.Add(new RandomItem<string>("Celina", 0.05));
                gc.Names.Add(new RandomItem<string>("Krzysztof", 0.05));
                gc.Names.Add(new RandomItem<string>("Helena", 0.05));
                gc.Names.Add(new RandomItem<string>("Maria", 0.05));
                gc.Names.Add(new RandomItem<string>("Eugeniusz", 0.05));
                gc.Names.Add(new RandomItem<string>("Aleksandra", 0.05));
                gc.Names.Add(new RandomItem<string>("Aleksander", 0.05));
                gc.Names.Add(new RandomItem<string>("Franciszek", 0.05));
                gc.Names.Add(new RandomItem<string>("Antoni", 0.05));
                gc.Names.Add(new RandomItem<string>("Stefania", 0.05));
                gc.Names.Add(new RandomItem<string>("Michalina", 0.05));
                gc.Names.Add(new RandomItem<string>("Feliks", 0.05));
                gc.Names.Add(new RandomItem<string>("Gabriela", 0.05));
                gc.Names.Add(new RandomItem<string>("Jadwiga", 0.05));
                #endregion
                #region Surnames
                gc.Surnames.Add(new RandomItem<string>("Smith", 0.05));
                gc.Surnames.Add(new RandomItem<string>("Rice", 0.05));
                gc.Surnames.Add(new RandomItem<string>("Baker", 0.05));
                gc.Surnames.Add(new RandomItem<string>("Grant", 0.05));
                gc.Surnames.Add(new RandomItem<string>("Miller", 0.05));
                gc.Surnames.Add(new RandomItem<string>("Mccoy", 0.05));
                gc.Surnames.Add(new RandomItem<string>("Solis", 0.05));
                gc.Surnames.Add(new RandomItem<string>("Garza", 0.05));
                gc.Surnames.Add(new RandomItem<string>("Day", 0.05));
                gc.Surnames.Add(new RandomItem<string>("Santos", 0.05));
                gc.Surnames.Add(new RandomItem<string>("Arnold", 0.05));
                gc.Surnames.Add(new RandomItem<string>("Perry", 0.05));
                gc.Surnames.Add(new RandomItem<string>("Klein", 0.05));
                gc.Surnames.Add(new RandomItem<string>("Wallace", 0.05));
                gc.Surnames.Add(new RandomItem<string>("Hancock", 0.05));
                gc.Surnames.Add(new RandomItem<string>("Bailey", 0.05));
                gc.Surnames.Add(new RandomItem<string>("Frazier", 0.05));
                gc.Surnames.Add(new RandomItem<string>("Lawson", 0.05));
                gc.Surnames.Add(new RandomItem<string>("Thomson", 0.05));
                gc.Surnames.Add(new RandomItem<string>("Houghton", 0.05));
                #endregion
                #region Addresses
                gc.Addresses.Add(new RandomItem<string>("2941 Alfred Drive,Harrisburg", 0.05));
                gc.Addresses.Add(new RandomItem<string>("1907 Northwest Boulevard, Jersey City", 0.05));
                gc.Addresses.Add(new RandomItem<string>("261 Marshville Road, Poughkeepsie", 0.05));
                gc.Addresses.Add(new RandomItem<string>("1740 Musgrave Street, East Point", 0.05));
                gc.Addresses.Add(new RandomItem<string>("2001 Parkway Street, Palm Springs", 0.05));
                gc.Addresses.Add(new RandomItem<string>("3382 Primrose Lane, Richland Center", 0.05));
                gc.Addresses.Add(new RandomItem<string>("3754 Redbud Drive, New York", 0.05));
                gc.Addresses.Add(new RandomItem<string>("4189 Shadowmar Drive, Metairie", 0.05));
                gc.Addresses.Add(new RandomItem<string>("4209 Coventry Court, Biloxi", 0.05));
                gc.Addresses.Add(new RandomItem<string>("3083 Harron Drive, Baltimore", 0.05));
                gc.Addresses.Add(new RandomItem<string>("648 Buckhannan Avenue, Syracuse", 0.05));
                gc.Addresses.Add(new RandomItem<string>("75 Penn Street, Stlouis ", 0.05));
                gc.Addresses.Add(new RandomItem<string>("3860 White Avenue, Corpus Christi", 0.05));
                gc.Addresses.Add(new RandomItem<string>("4341 Tipple Road, Philadelphia", 0.05));
                gc.Addresses.Add(new RandomItem<string>("3797 Bagwell Avenue, Weekiwachee Spgs.", 0.05));
                gc.Addresses.Add(new RandomItem<string>("4256 Michael Street, Houston", 0.05));
                gc.Addresses.Add(new RandomItem<string>("623 Carter Street, Granite City", 0.05));
                gc.Addresses.Add(new RandomItem<string>("1773 Medical Center Drive, Sarasota", 0.05));
                gc.Addresses.Add(new RandomItem<string>("3771 Wilkinson Court, Fort Myers", 0.05));
                gc.Addresses.Add(new RandomItem<string>("4883 Stiles Street, Pittsburgh", 0.05));
                #endregion
                #region Religions
                gc.Religions.Add(new RandomItem<string>("Catholic", 0.5));
                gc.Religions.Add(new RandomItem<string>("Protestant", 0.1));
                gc.Religions.Add(new RandomItem<string>("Jewish", 0.1));
                gc.Religions.Add(new RandomItem<string>("Pastafarian", 0.1));
                gc.Religions.Add(new RandomItem<string>("Hindu", 0.1));
                gc.Religions.Add(new RandomItem<string>("Confucianism", 0.05));
                #endregion
                #region Nationalities 
                //Based on actual "early" data from NSP 2021 
                const double PolishNationalityProbability = NSP2021EarlyResults.PolishOnly / NSP2021EarlyResults.TotalPopulation;
                const double numberOfSampleNationalMinorities = 5.0;
                const double GermanNationalityProbability = ((NSP2021EarlyResults.PolishOrNonPolish + NSP2021EarlyResults.Unrecognized + NSP2021EarlyResults.NonPolishOnly) / numberOfSampleNationalMinorities) / NSP2021EarlyResults.TotalPopulation;
                const double SilesianNationalityProbability = ((NSP2021EarlyResults.PolishOrNonPolish + NSP2021EarlyResults.Unrecognized + NSP2021EarlyResults.NonPolishOnly) / numberOfSampleNationalMinorities) / NSP2021EarlyResults.TotalPopulation;
                const double PomeranianNationalityProbability = ((NSP2021EarlyResults.PolishOrNonPolish + NSP2021EarlyResults.Unrecognized + NSP2021EarlyResults.NonPolishOnly) / numberOfSampleNationalMinorities) / NSP2021EarlyResults.TotalPopulation;
                const double LithuanianNationalityProbability = ((NSP2021EarlyResults.PolishOrNonPolish + NSP2021EarlyResults.Unrecognized + NSP2021EarlyResults.NonPolishOnly) / numberOfSampleNationalMinorities) / NSP2021EarlyResults.TotalPopulation;
                const double UkrainianNationalityProbability = ((NSP2021EarlyResults.PolishOrNonPolish + NSP2021EarlyResults.Unrecognized + NSP2021EarlyResults.NonPolishOnly) / numberOfSampleNationalMinorities) / NSP2021EarlyResults.TotalPopulation;
                gc.Nationalities.Add(new RandomItem<string>("polska", PolishNationalityProbability));
                gc.Nationalities.Add(new RandomItem<string>("niemiecka", GermanNationalityProbability));
                gc.Nationalities.Add(new RandomItem<string>("śląska", SilesianNationalityProbability));
                gc.Nationalities.Add(new RandomItem<string>("pomorska", PomeranianNationalityProbability));
                gc.Nationalities.Add(new RandomItem<string>("litewska", LithuanianNationalityProbability));
                gc.Nationalities.Add(new RandomItem<string>("ukraińska", UkrainianNationalityProbability));
                gc.NationalityErrorRate = NSP2021EarlyResults.Unrecognized / NSP2021EarlyResults.TotalPopulation;
                gc.NationalityErrorRateMaxImpact = 3;
                #endregion
                #region Ethnicities 
                ////Based on actual "early" data from NSP 2021 
                //const double PolishEthnicityProbability = (NSP2021EarlyResults.PolishOnly + NSP2021EarlyResults.PolishOrNonPolish) / NSP2021EarlyResults.TotalPopulation;
                //const double numberOfSampleEthnicMinorities = 5.0;
                //const double GermanEthnicityProbability = ((NSP2021EarlyResults.PolishOrNonPolish + NSP2021EarlyResults.Unrecognized + NSP2021EarlyResults.NonPolishOnly) / numberOfSampleEthnicMinorities) / NSP2021EarlyResults.TotalPopulation;
                //const double SilesianEthnicityProbability = ((NSP2021EarlyResults.PolishOrNonPolish + NSP2021EarlyResults.Unrecognized + NSP2021EarlyResults.NonPolishOnly) / numberOfSampleEthnicMinorities) / NSP2021EarlyResults.TotalPopulation;
                //const double PomeranianEthnicityProbability = ((NSP2021EarlyResults.PolishOrNonPolish + NSP2021EarlyResults.Unrecognized + NSP2021EarlyResults.NonPolishOnly) / numberOfSampleEthnicMinorities) / NSP2021EarlyResults.TotalPopulation;
                //const double LithuanianEthnicityProbability = ((NSP2021EarlyResults.PolishOrNonPolish + NSP2021EarlyResults.Unrecognized + NSP2021EarlyResults.NonPolishOnly) / numberOfSampleEthnicMinorities) / NSP2021EarlyResults.TotalPopulation;
                //const double UkrainianEthnicityProbability = ((NSP2021EarlyResults.PolishOrNonPolish + NSP2021EarlyResults.Unrecognized + NSP2021EarlyResults.NonPolishOnly) / numberOfSampleEthnicMinorities) / NSP2021EarlyResults.TotalPopulation;
                //gc.Ethnicities.Add(new RandomItem<string>("polska", PolishEthnicityProbability));
                //gc.Ethnicities.Add(new RandomItem<string>("niemiecka", GermanEthnicityProbability));
                //gc.Ethnicities.Add(new RandomItem<string>("śląska", SilesianEthnicityProbability));
                //gc.Ethnicities.Add(new RandomItem<string>("pomorska", PomeranianEthnicityProbability));
                //gc.Ethnicities.Add(new RandomItem<string>("litewska", LithuanianEthnicityProbability));
                //gc.Ethnicities.Add(new RandomItem<string>("ukraińska", UkrainianEthnicityProbability));
                //gc.EthnicityErrorRate = NSP2021EarlyResults.Unrecognized / NSP2021EarlyResults.TotalPopulation;
                //gc.EthnicityErrorRateMaxImpact = 3;
                #endregion
                #region Jobs 
                gc.Jobs.Add(new RandomItem<string>("Clerk", 0.2));
                gc.Jobs.Add(new RandomItem<string>("Gastronomy worker", 0.2));
                gc.Jobs.Add(new RandomItem<string>("Merchant", 0.2));
                gc.Jobs.Add(new RandomItem<string>("Doctor", 0.2));
                gc.Jobs.Add(new RandomItem<string>("Entrepreneur", 0.2));
                #endregion
                return gc;
            }

            public static GenerationConfiguration GetSimpleGenerationConfiguration()
            {
                var gc = new GenerationConfiguration();
                #region Names
                gc.Names.Add(new RandomItem<string>("J", 0.5));
                gc.Names.Add(new RandomItem<string>("R", 0.5));
                #endregion
                #region Surnames
                gc.Surnames.Add(new RandomItem<string>("Smith", 0.5));
                gc.Surnames.Add(new RandomItem<string>("Rice", 0.5));
                #endregion
                #region Addresses
                gc.Addresses.Add(new RandomItem<string>("A", 0.5));
                gc.Addresses.Add(new RandomItem<string>("B", 0.5));
                #endregion
                #region Religions
                gc.Religions.Add(new RandomItem<string>("Catholic", 0.5));
                gc.Religions.Add(new RandomItem<string>("Protestant", 0.5));
                #endregion
                #region Nationalities 
                //Based on actual "early" data from NSP 2021 
                const double PolishNationalityProbability = NSP2021EarlyResults.PolishOnly / NSP2021EarlyResults.TotalPopulation;
                const double numberOfSampleNationalMinorities = 5.0;
                const double GermanNationalityProbability = ((NSP2021EarlyResults.PolishOrNonPolish + NSP2021EarlyResults.Unrecognized + NSP2021EarlyResults.NonPolishOnly) / numberOfSampleNationalMinorities) / NSP2021EarlyResults.TotalPopulation;
                const double SilesianNationalityProbability = ((NSP2021EarlyResults.PolishOrNonPolish + NSP2021EarlyResults.Unrecognized + NSP2021EarlyResults.NonPolishOnly) / numberOfSampleNationalMinorities) / NSP2021EarlyResults.TotalPopulation;
                const double PomeranianNationalityProbability = ((NSP2021EarlyResults.PolishOrNonPolish + NSP2021EarlyResults.Unrecognized + NSP2021EarlyResults.NonPolishOnly) / numberOfSampleNationalMinorities) / NSP2021EarlyResults.TotalPopulation;
                const double LithuanianNationalityProbability = ((NSP2021EarlyResults.PolishOrNonPolish + NSP2021EarlyResults.Unrecognized + NSP2021EarlyResults.NonPolishOnly) / numberOfSampleNationalMinorities) / NSP2021EarlyResults.TotalPopulation;
                const double UkrainianNationalityProbability = ((NSP2021EarlyResults.PolishOrNonPolish + NSP2021EarlyResults.Unrecognized + NSP2021EarlyResults.NonPolishOnly) / numberOfSampleNationalMinorities) / NSP2021EarlyResults.TotalPopulation;
                gc.Nationalities.Add(new RandomItem<string>("polska", PolishNationalityProbability));
                gc.Nationalities.Add(new RandomItem<string>("niemiecka", GermanNationalityProbability));
                gc.Nationalities.Add(new RandomItem<string>("śląska", SilesianNationalityProbability));
                gc.Nationalities.Add(new RandomItem<string>("pomorska", PomeranianNationalityProbability));
                gc.Nationalities.Add(new RandomItem<string>("litewska", LithuanianNationalityProbability));
                gc.Nationalities.Add(new RandomItem<string>("ukraińska", UkrainianNationalityProbability));
                gc.NationalityErrorRate = NSP2021EarlyResults.Unrecognized / NSP2021EarlyResults.TotalPopulation;
                gc.NationalityErrorRateMaxImpact = 3;
                #endregion
                #region Ethnicities 
                //Based on actual "early" data from NSP 2021 
                //const double PolishEthnicityProbability = (NSP2021EarlyResults.PolishOnly + NSP2021EarlyResults.PolishOrNonPolish) / NSP2021EarlyResults.TotalPopulation;
                //const double numberOfSampleEthnicMinorities = 5.0;
                //const double GermanEthnicityProbability = ((NSP2021EarlyResults.PolishOrNonPolish + NSP2021EarlyResults.Unrecognized + NSP2021EarlyResults.NonPolishOnly) / numberOfSampleEthnicMinorities) / NSP2021EarlyResults.TotalPopulation;
                //const double SilesianEthnicityProbability = ((NSP2021EarlyResults.PolishOrNonPolish + NSP2021EarlyResults.Unrecognized + NSP2021EarlyResults.NonPolishOnly) / numberOfSampleEthnicMinorities) / NSP2021EarlyResults.TotalPopulation;
                //const double PomeranianEthnicityProbability = ((NSP2021EarlyResults.PolishOrNonPolish + NSP2021EarlyResults.Unrecognized + NSP2021EarlyResults.NonPolishOnly) / numberOfSampleEthnicMinorities) / NSP2021EarlyResults.TotalPopulation;
                //const double LithuanianEthnicityProbability = ((NSP2021EarlyResults.PolishOrNonPolish + NSP2021EarlyResults.Unrecognized + NSP2021EarlyResults.NonPolishOnly) / numberOfSampleEthnicMinorities) / NSP2021EarlyResults.TotalPopulation;
                //const double UkrainianEthnicityProbability = ((NSP2021EarlyResults.PolishOrNonPolish + NSP2021EarlyResults.Unrecognized + NSP2021EarlyResults.NonPolishOnly) / numberOfSampleEthnicMinorities) / NSP2021EarlyResults.TotalPopulation;
                //gc.Ethnicities.Add(new RandomItem<string>("polska", PolishEthnicityProbability));
                //gc.Ethnicities.Add(new RandomItem<string>("niemiecka", GermanEthnicityProbability));
                //gc.Ethnicities.Add(new RandomItem<string>("śląska", SilesianEthnicityProbability));
                //gc.Ethnicities.Add(new RandomItem<string>("pomorska", PomeranianEthnicityProbability));
                //gc.Ethnicities.Add(new RandomItem<string>("litewska", LithuanianEthnicityProbability));
                //gc.Ethnicities.Add(new RandomItem<string>("ukraińska", UkrainianEthnicityProbability));
                //gc.EthnicityErrorRate = NSP2021EarlyResults.Unrecognized / NSP2021EarlyResults.TotalPopulation;
                //gc.EthnicityErrorRateMaxImpact = 3;
                #endregion
                #region Jobs 
                gc.Jobs.Add(new RandomItem<string>("Job0", 0.5));
                gc.Jobs.Add(new RandomItem<string>("Job1", 0.5));
                #endregion
                return gc;
            }

            public static GenerationConfiguration GetOptimizedGenerationConfiguration()
            {
                var gc = new GenerationConfiguration();
                #region Names
                gc.Names.Add(new RandomItem<string>("", 1));
                #endregion
                #region Surnames
                gc.Surnames.Add(new RandomItem<string>("", 1));
                #endregion
                #region Addresses
                gc.Addresses.Add(new RandomItem<string>("", 1));
                #endregion
                #region Religions
                gc.Religions.Add(new RandomItem<string>("", 1));
                #endregion
                #region Nationalities 
                //Based on actual "early" data from NSP 2021 
                const double PolishNationalityProbability = NSP2021EarlyResults.PolishOnly / NSP2021EarlyResults.TotalPopulation;
                const double numberOfSampleNationalMinorities = 5.0;
                const double GermanNationalityProbability = ((NSP2021EarlyResults.PolishOrNonPolish + NSP2021EarlyResults.Unrecognized + NSP2021EarlyResults.NonPolishOnly) / numberOfSampleNationalMinorities) / NSP2021EarlyResults.TotalPopulation;
                const double SilesianNationalityProbability = ((NSP2021EarlyResults.PolishOrNonPolish + NSP2021EarlyResults.Unrecognized + NSP2021EarlyResults.NonPolishOnly) / numberOfSampleNationalMinorities) / NSP2021EarlyResults.TotalPopulation;
                const double PomeranianNationalityProbability = ((NSP2021EarlyResults.PolishOrNonPolish + NSP2021EarlyResults.Unrecognized + NSP2021EarlyResults.NonPolishOnly) / numberOfSampleNationalMinorities) / NSP2021EarlyResults.TotalPopulation;
                const double LithuanianNationalityProbability = ((NSP2021EarlyResults.PolishOrNonPolish + NSP2021EarlyResults.Unrecognized + NSP2021EarlyResults.NonPolishOnly) / numberOfSampleNationalMinorities) / NSP2021EarlyResults.TotalPopulation;
                const double UkrainianNationalityProbability = ((NSP2021EarlyResults.PolishOrNonPolish + NSP2021EarlyResults.Unrecognized + NSP2021EarlyResults.NonPolishOnly) / numberOfSampleNationalMinorities) / NSP2021EarlyResults.TotalPopulation;
                gc.Nationalities.Add(new RandomItem<string>("polska", PolishNationalityProbability));
                gc.Nationalities.Add(new RandomItem<string>("niemiecka", GermanNationalityProbability));
                gc.Nationalities.Add(new RandomItem<string>("śląska", SilesianNationalityProbability));
                gc.Nationalities.Add(new RandomItem<string>("pomorska", PomeranianNationalityProbability));
                gc.Nationalities.Add(new RandomItem<string>("litewska", LithuanianNationalityProbability));
                gc.Nationalities.Add(new RandomItem<string>("ukraińska", UkrainianNationalityProbability));
                gc.NationalityErrorRate = NSP2021EarlyResults.Unrecognized / NSP2021EarlyResults.TotalPopulation;
                gc.NationalityErrorRateMaxImpact = 0;
                #endregion
                #region Ethnicities 
                //Based on actual "early" data from NSP 2021 
                //const double PolishEthnicityProbability = (NSP2021EarlyResults.PolishOnly + NSP2021EarlyResults.PolishOrNonPolish) / NSP2021EarlyResults.TotalPopulation;
                //const double numberOfSampleEthnicMinorities = 5.0;
                //const double GermanEthnicityProbability = ((NSP2021EarlyResults.PolishOrNonPolish + NSP2021EarlyResults.Unrecognized + NSP2021EarlyResults.NonPolishOnly) / numberOfSampleEthnicMinorities) / NSP2021EarlyResults.TotalPopulation;
                //const double SilesianEthnicityProbability = ((NSP2021EarlyResults.PolishOrNonPolish + NSP2021EarlyResults.Unrecognized + NSP2021EarlyResults.NonPolishOnly) / numberOfSampleEthnicMinorities) / NSP2021EarlyResults.TotalPopulation;
                //const double PomeranianEthnicityProbability = ((NSP2021EarlyResults.PolishOrNonPolish + NSP2021EarlyResults.Unrecognized + NSP2021EarlyResults.NonPolishOnly) / numberOfSampleEthnicMinorities) / NSP2021EarlyResults.TotalPopulation;
                //const double LithuanianEthnicityProbability = ((NSP2021EarlyResults.PolishOrNonPolish + NSP2021EarlyResults.Unrecognized + NSP2021EarlyResults.NonPolishOnly) / numberOfSampleEthnicMinorities) / NSP2021EarlyResults.TotalPopulation;
                //const double UkrainianEthnicityProbability = ((NSP2021EarlyResults.PolishOrNonPolish + NSP2021EarlyResults.Unrecognized + NSP2021EarlyResults.NonPolishOnly) / numberOfSampleEthnicMinorities) / NSP2021EarlyResults.TotalPopulation;
                //gc.Ethnicities.Add(new RandomItem<string>("polska", PolishEthnicityProbability));
                //gc.Ethnicities.Add(new RandomItem<string>("niemiecka", GermanEthnicityProbability));
                //gc.Ethnicities.Add(new RandomItem<string>("śląska", SilesianEthnicityProbability));
                //gc.Ethnicities.Add(new RandomItem<string>("pomorska", PomeranianEthnicityProbability));
                //gc.Ethnicities.Add(new RandomItem<string>("litewska", LithuanianEthnicityProbability));
                //gc.Ethnicities.Add(new RandomItem<string>("ukraińska", UkrainianEthnicityProbability));
                //gc.EthnicityErrorRate = NSP2021EarlyResults.Unrecognized / NSP2021EarlyResults.TotalPopulation;
                //gc.EthnicityErrorRateMaxImpact = 0;
                #endregion
                #region Jobs 
                gc.Jobs.Add(new RandomItem<string>("", 1));
                #endregion
                return gc;
            }
            #endregion

            public static class NSP2021EarlyResults
            {
                public const double TotalPopulation = 38.0361; //in milions
                public const double PolishOnly = 33.0418; //in milions
                public const double PolishOrNonPolish = 0.8558; //in milions
                public const double NonPolishOnly = 0.3910; //in milions
                public const double Unrecognized = 3.7476; //in milions
            }
        }
    }
}