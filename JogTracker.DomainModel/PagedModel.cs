using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JogTracker.DomainModel
{
    /// <summary>
    /// Contains a collection of domainModel items, as well as paging information.
    /// Returned when request was made with paging information. Typically used on front end to determine
    /// how many page links to render.
    /// </summary>
    /// <typeparam name="TDomainModel"></typeparam>
    public class PagedModel<TDomainModel>
    {
        public ICollection<TDomainModel> Items { get; private set; }
        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public int TotalResults { get; private set; }

        public PagedModel(int pageIndex, int pageSize, int totalResults, ICollection<TDomainModel> items)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalResults = totalResults;
            Items = items;
        }
    }
}
