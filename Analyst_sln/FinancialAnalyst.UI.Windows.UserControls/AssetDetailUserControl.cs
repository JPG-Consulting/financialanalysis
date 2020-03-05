using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FinancialAnalyst.Common.Entities.Assets;
using FinancialAnalyst.Common.Entities.RequestResponse;

namespace FinancialAnalyst.UI.Windows.UserControls
{
    public partial class AssetDetailUserControl : UserControl
    {
        public AssetDetailUserControl()
        {
            InitializeComponent();
        }

        public void Show(APIResponse<Stock> response)
        {
            if(response.Ok)
            {
                Stock s = response.Content;
                labelName.ForeColor = this.ForeColor;
                labelName.Text = s.CompanyName;
                textBoxDescription.Text = s.Description;
            }
            else
            {
                labelName.Text = response.ErrorMessage;
                labelName.ForeColor = Color.Red;
            }
        }
    }
}
