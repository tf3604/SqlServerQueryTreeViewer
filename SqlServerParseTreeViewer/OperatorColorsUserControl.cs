using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SqlServerParseTreeViewer
{
    public partial class OperatorColorsUserControl : UserControl
    {
        public OperatorColorsUserControl()
        {
            InitializeComponent();
        }

        private void CustomColorButton_Click(object sender, EventArgs e)
        {
            if (standardColorComboBox.Items.Count > 0)
            {
                using (ColorDialog dialog = new ColorDialog())
                {
                    dialog.AnyColor = true;
                    dialog.FullOpen = true;
                    dialog.Color = (Color)standardColorComboBox.Items[0];
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        standardColorComboBox.Items[0] = dialog.Color;
                        OperatorColor operatorColor = operatorsListView.SelectedItem as OperatorColor;
                        if (operatorColor != null)
                        {
                            operatorColor.DisplayColor = dialog.Color;
                        }
                    }
                }
            }
        }

        private void OperatorColorsUserControl_Load(object sender, EventArgs e)
        {
            operatorsListView.Items.Clear();
            List<OperatorColor> operatorColors = ViewerSettings.Clone.OperatorColors.ToList();
            operatorColors.Sort((a, b) => a.OperatorName.CompareTo(b.OperatorName));
            operatorColors.ForEach(c => operatorsListView.Items.Add(c));
        }

        private void OperatorsListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            OperatorColor operatorColor = operatorsListView.SelectedItem as OperatorColor;
            if (operatorColor != null)
            {
                standardColorComboBox.Items.Clear();
                standardColorComboBox.Items.Add(operatorColor.DisplayColor);

                Type colorType = typeof(Color);
                List<PropertyInfo> colorProperties = colorType.GetProperties(BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Public).ToList();
                colorProperties.Sort((a, b) => a.Name.CompareTo(b.Name));
                foreach (PropertyInfo property in colorProperties)
                {
                    Color color = (Color)property.GetMethod.Invoke(null, null);
                    standardColorComboBox.Items.Add(color);
                }

                standardColorComboBox.SelectedIndex = 0;
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            OperatorColor operatorColor = operatorsListView.SelectedItem as OperatorColor;
            if (operatorColor != null)
            {
                ViewerSettings.Clone.DeleteOperatorColor(operatorColor.OperatorName);
                operatorsListView.Items.Remove(operatorColor);
            }
        }

        private void StandardColorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            OperatorColor operatorColor = operatorsListView.SelectedItem as OperatorColor;
            if (operatorColor != null)
            {
                operatorColor.DisplayColor = (Color)standardColorComboBox.SelectedItem;
            }
        }
    }
}
