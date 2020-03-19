using FinancialAnalyst.Common.Entities.Assets;
using FinancialAnalyst.Common.Entities.RequestResponse;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinancialAnalyst.UI.Windows.ChildForms
{
    public partial class AssetDetailForm : Form
    {
        private AssetDetailForm()
        {
            InitializeComponent();
            optionsChainUserControl1.Dock = DockStyle.Fill;
            assetDetailUserControl1.Dock = DockStyle.Fill;
            splitContainerMain.Dock = DockStyle.Fill;
        }

        public AssetDetailForm(APIResponse<Stock> response):this()
        {
            assetDetailUserControl1.Show(response);
            optionsChainUserControl1.Show(response);
        }

    }
}
