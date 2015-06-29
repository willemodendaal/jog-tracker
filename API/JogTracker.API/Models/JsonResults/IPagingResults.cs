using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JogTracker.Api.Models.JsonResults
{
    public class PagingResults
    {
        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public int TotalResults { get; private set; }
        public object Items { get; private set; }

        public PagingResults(int pageIndex, int pageSize, int totalResults, object items)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalResults = totalResults;
            Items = items;
        }
    }
}