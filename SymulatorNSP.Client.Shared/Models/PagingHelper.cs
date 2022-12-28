using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymulatorNSP.Client.Shared.Models
{
    public class PagingHelper
    {
        List<Page> Pages { get; } = new List<Page>();

        public int PageCount
        {
            get { return Pages.Count; }
        }

        public PagingInfo PagingInfo { get; }   

        public PagingHelper(PagingInfo pagingInfo) 
        {
            this.PagingInfo = pagingInfo;
            var toDivide = pagingInfo.RecordsCount;
            var offset = 0;
            while(toDivide!=0)
            {
                if (toDivide >= this.PagingInfo.PageSize)
                {
                    var page = new Page() { Begin = offset, End = offset + this.PagingInfo.PageSize};
                    offset = offset + this.PagingInfo.PageSize;
                    toDivide = toDivide - this.PagingInfo.PageSize;
                    this.Pages.Add(page);
                }
                else
                {
                    var page = new Page() { Begin = offset, End = offset + toDivide };
                    offset = offset + toDivide;
                    toDivide = toDivide - toDivide;
                    this.Pages.Add(page);
                }
            }
        }

        public Page GetPage(int i)
        {
            return this.Pages[i];
        }
    }
}
