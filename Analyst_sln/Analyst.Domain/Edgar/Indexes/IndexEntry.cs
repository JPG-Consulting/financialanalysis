using Analyst.Domain.Edgar.Datasets;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Analyst.Domain.Edgar.Indexes
{
    [Serializable]
    [DataContract]
    public class IndexEntry: IEdgarEntity
    {
        [Required]
        [DataMember]
        public int Id { get; set; }

        public string Key
        {
            get
            {
                //return DateFiled.Year.ToString() + DateFiled.Month.ToString() + DateFiled.Day.ToString() + FormType.Key + Company.CIK.ToString();
                return DateFiled.Year.ToString() + DateFiled.Month.ToString() + DateFiled.Day.ToString() + FormType.Key + CIK.ToString();
            }
        }

        [Required]
        [DataMember]
        public Registrant Company { get; set; }

        [NotMapped]
        public string CompanyName { get; set; }

        /// <summary>
        /// CIK == CompanyId as FK
        /// </summary>
        [Required]
        [DataMember]
        public int CIK { get; set; }

        [Required]
        [DataMember]
        public SECForm FormType { get; set; }
        public int FormTypeId { get; set; }

        [Required]
        [DataMember]
        public DateTime DateFiled { get; set; }
        
        [Required]
        [DataMember]
        public string RelativeURL { get; set; }

        [Required]
        public MasterIndex MasterIndex { get; set; }
        public int MasterIndexId { get; set; }

    }
}
