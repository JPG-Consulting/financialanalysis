using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Analyst.Domain.Edgar;
using PagedList;

namespace Analyst.Web.Models
{
    public class AskEdgarModel
    {
        public string Title { get; internal set; }
        public IPagedList<Registrant> Registrants { get; set; }
        public Registrant SelectedRegistrant { get; set; }
        public IPagedList<SECForm> Filings { get; set; }
        public int PageCount { get; set; }
        public int PageNumber { get; set; }
        public int Total { get; set; }
    }
}