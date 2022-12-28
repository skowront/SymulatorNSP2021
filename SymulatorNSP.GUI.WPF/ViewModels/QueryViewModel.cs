using SymulatorNSP.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymulatorNSP.GUI.Core.ViewModels
{
    public class QueryViewModel:BaseViewModel
    {
        private Query model;

        public string Value
        {
            get { return model.Value; }
            set { model.Value = value; this.OnPropertyChanged(); }
        }

        public string Result
        {
            get { return model.Result; }
            set { model.Result = value; this.OnPropertyChanged(); }
        }


        public int QueriedRecordsCount
        {
            get { return model.QueriedRecordsCount; }
            set { model.QueriedRecordsCount = value; this.OnPropertyChanged(); }
        }

        public DateTime StartTime
        {
            get { return model.StartTime; }
            set { model.StartTime = value; this.OnPropertyChanged(); }
        }
        public DateTime EndTime
        {
            get { return model.EndTime; }
            set { model.EndTime = value; this.OnPropertyChanged(); }
        }
        public TimeSpan ExecutionTime
        {
            get { return model.ExecutionTime; }
            set { model.ExecutionTime = value; this.OnPropertyChanged(); }
        }

        public QueryViewModel(Query? model = null)
        {
            this.model = model ?? new Query();
        }
    }
}
