using Analyst.Domain.Edgar.Datasets;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyst.Domain.Edgar.Indexes
{
    public class IndexEntry: IEdgarEntity
    {
        public int Id { get; set; }

        public string Key
        {
            get
            {
                return DateFiled.Year.ToString() + DateFiled.Month.ToString() + DateFiled.Day.ToString() + FormType.Key + Company.CIK.ToString();
            }
        }

        public Company Company { get; set; }

        public SECForm FormType { get; set; }

        public DateTime DateFiled { get; set; }
            
        public string FileName { get; set; }


        public int MasterIndexId { get; set; }

        [Required]
        public MasterFullIndex MasterIndex { get; set; }

        public string RelativeURL { get; set; }

        public int CIK { get; set; }//TODO: no mappear
    }
}
