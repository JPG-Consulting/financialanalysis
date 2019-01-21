using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyst.Domain.Edgar.Indexes
{
    /// <summary>
    /// Four types of indexes are available. 
    /// The company, form, and master indexes contain the same information sorted differently.
    /// * company — sorted by company name
    /// * form — sorted by form type
    /// * master — sorted by CIK number
    /// * XBRL — list of submissions containing XBRL financial files, sorted by CIK number; these include Voluntary Filer Program submissions
    /// 
    /// The key is the CIK number
    /// </summary>
    public class MasterFullIndex:IndexBase<int>
    {


    }
}
