using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyst.Domain.Edgar.Indexes
{

    public abstract class IndexBase<T> 
    {
        public ushort Year { get; set; }
        public Quarter Quarter { get; set; }
        public string RelativeURL { get; set; }//TODO: no mappear


        /// <summary>
        /// It indicates if all the forms of this index are stored (and ready to query) or not
        /// </summary>
        public bool IsComplete { get; set; }

        public Dictionary<T, IndexEntry> Entries { get; set; }
    }
}
