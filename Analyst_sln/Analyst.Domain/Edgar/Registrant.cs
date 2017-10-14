using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyst.Domain.Edgar
{
    [Serializable]
    public abstract class Registrant
    {
        public int Id { get; set; }

        /// <summary>
        /// Central Index Key (CIK). 
        /// Ten digit number assigned by the Commission 
        /// to each registrant that submits filings.
        /// </summary>
        public string CIK { get; set; }

        public string Name { get; set; }

        
        public SIC SIC { get; set; }

        /// <summary>
        /// The ISO 3166-1 country of the registrant's business address.
        /// </summary>
        public string CountryBA { get; set; }

        /// <summary>
        /// The city of the registrant's business address.
        /// </summary>
        public string CityBA { get; set; }

        /// <summary>
        /// The country of incorporation for the registrant.
        /// </summary>
        public string CountryInc { get; set; }

        /// <summary>
        /// Employee Identification Number
        /// 9 digit identification number 
        /// assigned by the Internal Revenue Service 
        /// to business entities operating in the United States.
        /// </summary>
        public int EIN { get; set; }

        /// <summary>
        /// Filer status with the Commission at the time of submission:
        /// 1-LAF=Large Accelerated,
        /// 2-ACC=Accelerated,
        /// 3-SRA=Smaller Reporting Accelerated,
        /// 4-NON=Non-Accelerated,
        /// 5-SML=Smaller Reporting Filer,
        /// NULL=not assigned.
        /// </summary>
        public short AFS { get; set; }

        /// <summary>
        /// Well Known Seasoned Issuer (WKSI). 
        /// An issuer that meets specific Commission requirements at some point during a 60-day period preceding the date the issuer satisfies its obligation to update its shelf registration statement.
        /// </summary>
        public bool WKSI { get; set; }

        /// <summary>
        /// Fiscal Year End Date
        /// </summary>
        public DateTime FYE { get; set; }


    }
}
