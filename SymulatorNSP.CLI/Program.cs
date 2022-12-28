using SymulatorNSP.Core;
using SymulatorNSP.GUI.Core.ViewModels;

namespace SymulatorNSP.CLI
{
    internal class Program
    {
//        static void Main(string[] args)
//        {
//            var time = DateTime.Now;
//            Population population = new Population();
//#if DEBUG
//            var config = Person.GenerationConfiguration.GetDefaultGenerationConfiguration();
//            population.Generate(38000, config);
//#else
//            population.Generate(38036100, Person.GenerationConfiguration.GetDefaultGenerationConfiguration());
//#endif
//            Console.WriteLine("Generowanie zajęło: " + (DateTime.Now - time).TotalSeconds.ToString() + "sekund.");

//            time = DateTime.Now;
//            var plCount = population.People.Where((p) => { return p.Nationality == "Polska"; }).Count();
//            var endTime = DateTime.Now;
//            Console.WriteLine("Polskiej narodowości wygenerowano: " + plCount);
//            Console.WriteLine("Kwerenda trwała: " + (endTime - time).TotalSeconds.ToString() + "sekund.");
//        }

        static void Main(string[] args)
        {
            var time = DateTime.Now;
            var gus = new GUSViewModel();
#if DEBUG
            gus.model.Generate(38030);
#else
            gus.Generate(38036100);
#endif
            Console.WriteLine("Record generation time: " + (DateTime.Now - time).TotalSeconds.ToString() + " seconds.");

            time = DateTime.Now;
            var plCount = gus.QueryNationalityCount("polska",null,1);  
            var endTime = DateTime.Now;
            Console.WriteLine("There were : " + plCount);
            Console.WriteLine("Query execution time: " + (endTime - time).TotalSeconds.ToString() + " seconds.");
        }
    }
}