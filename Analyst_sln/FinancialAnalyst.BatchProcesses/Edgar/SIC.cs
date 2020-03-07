using Analyst.Domain.Edgar.Datasets;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Analyst.Domain.Edgar
{
    /// <summary>
    /// Standard Industrial Classification (SIC). 
    /// Four digit code assigned by the Commission as of the filing date, 
    /// indicating the registrant's type of business.
    /// </summary>
    [Serializable]
    [DataContract]
    public class SIC:IEdgarEntity
    {
        [Key]
        [DataMember]
        public int Id { get; set; }

        // https://docs.microsoft.com/es-es/ef/core/modeling/indexes
        // https://github.com/jsakamoto/EntityFrameworkCore.IndexAttribute
        //[Index(IsUnique =true)]
        [DataMember]
        public short Code { get; set; }

        [StringLength(5)]
        [DataMember]
        public string ADOffice { get; set; }

        [StringLength(100)]
        [DataMember]
        public string IndustryTitle { get; set; }

        public string Key
        {
            get
            {
                return Code.ToString();
            }
        }
    }
}
