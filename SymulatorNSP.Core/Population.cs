using Newtonsoft.Json;
using System.Collections.ObjectModel;


namespace SymulatorNSP.Core
{
    public class Population
    {
        public Collection<Person> People { get; set; } = new Collection<Person>();

        public Population()
        {

        }

        public void Generate(int populationSize, Person.GenerationConfiguration generationConfiguration, Action<ProgressValue>? OnProgressChanged = null)
        {
            this.People.Clear();
            int progress = 0;
            for (int i = 0; i < populationSize; i++)
            {
                var person = new Person();
                person.Generate(generationConfiguration);
                person.Id = i;
                this.People.Add(person);

                progress++;
                OnProgressChanged?.Invoke(new ProgressValue() { Start = 0, Current = progress+1, End = populationSize });
            }
        }

        async public Task GenerateAsync(int populationSize, Person.GenerationConfiguration generationConfiguration, int step, IProgress<ProgressValue>? OnProgressChanged = null)
        {
            this.People.Clear();
            int progress = 1;
            var tasks = new List<Task>();
            //for (int i = 0; i < 100; i++)
            //{
            //    await Task.Delay(100);
            //    OnProgressChanged.Invoke(new Progress() { Start = 0, Current = i, End = 100 });
            //}
            for (int i = 0; i < populationSize; i++)
            {
                var person = new Person();
                Task t = Task.Run(async () => { await person.GenerateAsync(generationConfiguration); });
                tasks.Add(t);
                person.Id = i;
                this.People.Add(person);
                if(populationSize> step)
                {
                    if(progress% step == 0 || progress == populationSize)
                    {
                        await Task.Delay(TimeSpan.FromMilliseconds(1));
                        OnProgressChanged?.Report(new ProgressValue() { Start = 0, Current = progress, End = populationSize, StepReport=true });
                    }
                }
                else
                {
                    OnProgressChanged?.Report(new ProgressValue() { Start = 0, Current = progress, End = populationSize });
                }
                progress++;
            }
            await Task.WhenAll(tasks);
            return;
        }

        public static string Pack(Population population)
        {
            return JsonConvert.SerializeObject(population);
        }

        public static Population Unpack(string json)
        {
            var population = new Population();
            JsonConvert.PopulateObject(json, population);
            return population;
        }
    }
}
