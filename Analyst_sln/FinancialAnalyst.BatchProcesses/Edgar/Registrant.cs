using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyst.Domain.Edgar
{
    [Serializable]
    public class Registrant
    {

        /// <summary>
        /// Possible filing status values.
        /// </summary>
        public enum FilerStatusValue
        {
            LargeAccelerated,
            Accelerated,
            SmallerReportingAccelerated,
            NonAccelerated,
            SmallerReportingFiler,
            NotAssigned
        }

        /// <summary>
        /// Possible fiscal period focus values.
        /// </summary>
        public enum FiscalPeriodFocusValue
        {
            FiscalYear,
            FirstQuarter,
            SecondQuarter,
            ThirdQuarter,
            FourthQuarter,
            FirstHalf,
            SecondHalf,
            NineMonths,
            FirstTrimester,
            SecondTrimester,
            ThirdTrimester,
            EightMonths,
            CalendarYear,
            Unknown
        }

        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Central Index Key (CIK). 
        /// Ten digit number assigned by the Commission 
        /// to each registrant that submits filings.
        /// </summary>
        // https://docs.microsoft.com/es-es/ef/core/modeling/indexes
        // https://github.com/jsakamoto/EntityFrameworkCore.IndexAttribute
        //[Index(IsUnique = true)]
        public int CIK { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        
        public SIC SIC { get; set; }

        /// <summary>
        /// The ISO 3166-1 country of the registrant's business address.
        /// </summary>
        [StringLength(3)]
        public string CountryBA { get; set; }

        /// <summary>
        /// The city of the registrant's business address.
        /// </summary>
        [StringLength(100)]
        public string CityBA { get; set; }

        /// <summary>
        /// The country of incorporation for the registrant.
        /// </summary>
        [StringLength(3)]
        public string CountryInc { get; set; }

        /// <summary>
        /// Employee Identification Number (EIN)
        /// 9 digit identification number 
        /// assigned by the Internal Revenue Service 
        /// to business entities operating in the United States.
        /// </summary>
        public int? EmployeeIdentificationNumber { get; set; }

        /// <summary>
        /// Filer status with the Commission at the time of submission:
        /// 1-LAF=Large Accelerated,
        /// 2-ACC=Accelerated,
        /// 3-SRA=Smaller Reporting Accelerated,
        /// 4-NON=Non-Accelerated,
        /// 5-SML=Smaller Reporting Filer,
        /// NULL=not assigned.
        /// Field name in the file: AFS
        /// </summary>
        [StringLength(5)]
        public string FilerStatus { get; set; }


        public FilerStatusValue GetFilerStatus()
        {
            string stringValue = FilerStatus;
            FilerStatusValue status = FilerStatusValue.NotAssigned;
            switch (stringValue)
            {
                case "1-LAF":
                    status = FilerStatusValue.LargeAccelerated;
                    break;
                case "2-ACC":
                    status = FilerStatusValue.Accelerated;
                    break;
                case "3-SRA":
                    status = FilerStatusValue.SmallerReportingAccelerated;
                    break;
                case "4-NON":
                    status = FilerStatusValue.NonAccelerated;
                    break;
                case "5-SML":
                    status = FilerStatusValue.SmallerReportingFiler;
                    break;
                default:
                    break;
            }
            return status;
        }

        /// <summary>
        /// Well Known Seasoned Issuer (WKSI). 
        /// An issuer that meets specific Commission requirements at some point during a 60-day period preceding the date the issuer satisfies its obligation to update its shelf registration statement.
        /// </summary>
        public bool? WellKnownSeasonedIssuer { get; set; }

        /// <summary>
        /// Fiscal Year End Date
        /// ALPHANUMERIC (mmdd)
        /// Field name in the file: FYE
        /// </summary>
        public short? FiscalYearEndDate { get; set; }


    }
}
