using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace FinancialAnalyst.Common.Entities.EdgarSEC
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
