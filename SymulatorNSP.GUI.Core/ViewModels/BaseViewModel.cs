using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SymulatorNSP.GUI.Core.ViewModels
{
    /// <summary>
    /// Base usefull ViewModel implementation
    /// </summary>
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public bool Initialized { get; set; }

        public virtual Task InitializeAsync()
        {
            Initialized = true;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Default ctor
        /// </summary>
        public BaseViewModel()
        {
        }

        //Notify Property Changed Interface
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
