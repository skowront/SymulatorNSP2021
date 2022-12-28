using SymulatorNSP.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymulatorNSP.GUI.Core.ViewModels
{
    public class LeaderboardRecordViewModel : BaseViewModel
    {
        public LeaderboardRecord Model { get; set; }

        public string Nickname
        {
            get { return Model.Nickname; }
            set { Model.Nickname = value; this.OnPropertyChanged(); }
        }

        public int QueryCount
        {
            get { return Model.QueryCount; }
            set { Model.QueryCount = value; this.OnPropertyChanged(); }
        }

        public DateTime StartTime
        {
            get { return Model.StartTime; }
            set { this.Model.StartTime = value; this.OnPropertyChanged(); }
        }

        public DateTime EndTime
        {
            get { return this.Model.EndTime; }
            set { this.Model.EndTime = value; this.OnPropertyChanged(); }
        }

        public DateTime Timestamp
        {
            get { return this.Model.Timestamp; }
            set { this.Model.Timestamp = value; this.OnPropertyChanged(); }
        }

        public double Factor
        {
            get { return Model.Factor; }
            set { this.Model.Factor = value; this.OnPropertyChanged(); }
        }

        public int Threads
        {
            get { return this.Model.Threads; }
            set { this.Model.Threads = value; this.OnPropertyChanged(); }
        }

        public TimeSpan ExecutionTime
        {
            get { return this.Model.ExecutionTime(); }
        }

        public string Source
        {
            get
            {
                switch (this.Model.Source)
                {
                    case LeaderboardRecord.eSource.Browser:
                        return Resources.Resource.Browser;
                    case LeaderboardRecord.eSource.Windows:
                        return Resources.Resource.Windows;
                    default:
                        return Resources.Resource.Unknown;
                }
            }
            set
            {
                if (value == Resources.Resource.Browser) this.Model.Source = LeaderboardRecord.eSource.Browser;
                if (value == Resources.Resource.Windows) this.Model.Source = LeaderboardRecord.eSource.Windows;
                if (value == Resources.Resource.Unknown) this.Model.Source = LeaderboardRecord.eSource.Windows;
            }
        }

        public LeaderboardRecordViewModel()
        {
            this.Model = new LeaderboardRecord();
        }

        public LeaderboardRecordViewModel(LeaderboardRecord model)
        {
            if (model == null)
            {
                this.Model = new LeaderboardRecord();
            }
            else
            {
                this.Model = model;
            }
        }

    }
}
