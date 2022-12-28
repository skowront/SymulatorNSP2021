using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymulatorNSP.Client.Shared.Services.Interfaces
{
    public interface IPageNavigable
    {
        public int PagesCount { get; }
        public int CurrentPage { get; set; }
        public int GetPageSize();
        public Task GoToFirst();
        public Task GoToLast();
        public Task GoToNext();
        public Task GoToPrevious();
        public Task GoToPageNumber(int pageNumber);
    }
}
