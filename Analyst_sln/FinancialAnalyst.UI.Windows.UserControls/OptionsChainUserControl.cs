using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FinancialAnalyst.Common.Entities.RequestResponse;
using FinancialAnalyst.Common.Entities.Assets;

namespace FinancialAnalyst.UI.Windows.UserControls
{
    public partial class OptionsChainUserControl : UserControl
    {
        public OptionsChainUserControl()
        {
            InitializeComponent();
        }

        public void Show(APIResponse<Stock> response)
        {
            MessageBox.Show("Pending to show option chain");
        }
    }
}
