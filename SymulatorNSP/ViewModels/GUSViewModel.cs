using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using SymulatorNSP.GUI.Core.Resources;
using SymulatorNSP.GUI.Core.ViewModels;
using System.IO;
using System.Runtime.CompilerServices;

namespace SymulatorNSP.ViewModels
{
    public class GUSViewModel:SymulatorNSP.GUI.Core.ViewModels.GUSViewModel
    {
        private bool isGenerating = false;
        public bool IsGenerating 
        { 
            get 
            { 
                return isGenerating; }
            set 
            {
                this.isGenerating = value; this.OnPropertyChanged();
            }
        }


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

        public async Task LoadPopulation(InputFileChangeEventArgs file)
        {
            try
            {
                using (StreamReader reader = new StreamReader(file.File.OpenReadStream()))
                {
                    this.model.DeserializePopulation(await reader.ReadToEndAsync());
                }
                this.AddLogEntry(Resource.LoadSuccess);
            }
            catch (Exception ex)
            {
                this.AddLogEntry(Resource.LoadFailed);
            }
        }
    }
}
