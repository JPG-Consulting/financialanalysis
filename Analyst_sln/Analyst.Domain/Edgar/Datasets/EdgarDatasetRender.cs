using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyst.Domain.Edgar.Datasets
{
    /// <summary>
    /// The REN data set summarizes for each filing the data provided by filers 
    /// about each presentation group as defined in EDGAR Filer manual volume II section 6.24. 
    /// The source for the table is the "as filed" XBRL filer submissions as processed by the renderer.
    /// Note that in rare cases the number of reports may differ depending on the version of the EDGAR Renderer used.
    /// The numbering provided in this table is the numbering resulting as processed 
    /// by the most current version of the EDGAR Renderer as of the date of data set publication; 
    /// for example, if the data for the 1st quarter of 2010 is not published on the Commission web site until 2016, 
    /// the rendering will be the EDGAR Renderer version current in 2016.
    /// </summary>
    public class EdgarDatasetRender:IEdgarDatasetFile
    {
        public static readonly string FILE_NAME = "ren.tsv";

        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Represents the report grouping. 
        /// The numeric value refers to the "R file" as computed by the renderer and posted on the EDGAR website.  
        /// Note that in some situations the numbers skip.
        /// </summary>
        [Index("IX_SubmissionId_Report",IsUnique = true,Order =2)]
        [Required]
        public int Report { get; set; }

        /// <summary>
        /// The type of interactive data file rendered on the EDGAR website, H = .htm file, X = .xml file.
        /// </summary>
        [Required]
        public char RenderFile { get; set; }

        /// <summary>
        /// If available, one of the menu categories as computed by the renderer: 
        /// C=Cover, S=Statements, N=Notes, P=Policies, T=Tables, D=Details, O=Other, and U=Uncategorized.
        /// </summary>
        [StringLength(20)]
        public string MenuCategory { get; set; }

        /// <summary>
        /// The portion of the long name used in the renderer menu.
        /// </summary>
        [Required]
        [StringLength(255)]
        public string ShortName { get; set; }


        /// <summary>
        /// The space-normalized text of the XBRL link "definition" element content.
        /// </summary>
        [StringLength(300)]
        public string LongName{ get; set; }

        /// <summary>
        /// The XBRL "roleuri" of the role.
        /// </summary>
        [Required]
        [StringLength(255)]
        public string RoleURI { get; set; }

        /// <summary>
        /// The XBRL roleuri of a role for which this role has a matching shortname prefix and a higher level menu category, as computed by the renderer.
        /// </summary>
        [StringLength(255)]
        public string ParentRoleURI { get; set; }

        /// <summary>
        /// The value of the report field for the role where roleuri equals this parentroleuri.
        /// </summary>
        public int? ParentReport { get; set; }

        /// <summary>
        /// The highest ancestor report reachable by following parentreport relationships.A note (menucat = N) is its own ultimate parent.
        /// </summary>
        public int? UltimateParentReport { get; set; }


        public int LineNumber { get; set; }

        public string Key
        {
            get
            {
                return Submission.ADSH + Report.ToString();
            }
        }
        
        public int DatasetId { get; set; }

        [Index("IX_SubmissionId_Report", IsUnique = true, Order = 1)]
        public int SubmissionId { get; set; }
        [Required]
        public EdgarDatasetSubmission Submission { get; set; }

        [Required]
        public virtual EdgarDataset Dataset { get; set; }

        public ICollection<EdgarDatasetPresentation> Presentations { get; set; }
        
    }
}
