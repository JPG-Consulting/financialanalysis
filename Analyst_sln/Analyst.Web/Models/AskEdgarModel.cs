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
        public IPagedList<Registrant> Registrants { get; internal set; }
        public int PageCount { get; set; }
        public int PageNumber { get; set; }
    }
}