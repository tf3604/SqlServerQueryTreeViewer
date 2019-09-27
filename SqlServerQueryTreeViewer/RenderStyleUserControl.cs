using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SqlServerQueryTreeViewer
{
    public partial class RenderStyleUserControl : UserControl
    {
        public RenderStyleUserControl()
        {
            InitializeComponent();
        }

        private void RenderStyleUserControl_Load(object sender, EventArgs e)
        {
            if (ViewerSettings.Clone.TreeRenderStyle == TreeRenderStyle.VerticalBalancedTree)
            {
                vertialBalancedTreeButton.Checked = true;
            }
            else
            {
                planStyleHorizontalButton.Checked = true;
            }

            hideLowLevelNodesCheckBox.Checked =
                ViewerSettings.Clone.HideLowValueLeafLevelNodes == null ? true : ViewerSettings.Clone.HideLowValueLeafLevelNodes.Value;
        }

        private void PlanStyleHorizontalButton_CheckedChanged(object sender, EventArgs e)
        {
            if (planStyleHorizontalButton.Checked)
            {
                ViewerSettings.Clone.TreeRenderStyle = TreeRenderStyle.PlanStyleHorizontalTree;
            }
        }

        private void VertialBalancedTreeButton_CheckedChanged(object sender, EventArgs e)
        {
            if (vertialBalancedTreeButton.Checked)
            {
                ViewerSettings.Clone.TreeRenderStyle = TreeRenderStyle.VerticalBalancedTree;
            }
        }

        private void HideLowLevelNodesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ViewerSettings.Clone.HideLowValueLeafLevelNodes = hideLowLevelNodesCheckBox.Checked;
        }
    }
}
