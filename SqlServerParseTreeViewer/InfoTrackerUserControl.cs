using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SqlServerParseTreeViewer
{
    public partial class InfoTrackerUserControl : UserControl
    {
        public InfoTrackerUserControl()
        {
            InitializeComponent();
        }

        private void InfoTrackerUserControl_Load(object sender, EventArgs e)
        {
            optimizerInfoCheckBox.Checked = ViewerSettings.Instance.TrackOptimizerInfo;
            transformationStatsCheckBox.Checked = ViewerSettings.Instance.TrackTransformationStats;
        }

        private void OptimizerInfoCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ViewerSettings.Clone.TrackOptimizerInfo = optimizerInfoCheckBox.Checked;
        }

        private void TransformationStatsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ViewerSettings.Clone.TrackTransformationStats = transformationStatsCheckBox.Checked;
        }
    }
}
