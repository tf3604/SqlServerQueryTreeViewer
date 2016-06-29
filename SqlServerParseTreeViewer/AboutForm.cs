using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SqlServerParseTreeViewer
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            IEnumerable<Attribute> attributes = assembly.GetCustomAttributes();
            if (attributes != null)
            {
                AssemblyCopyrightAttribute copyrightAttribute = attributes.FirstOrDefault(a => a is AssemblyCopyrightAttribute) as AssemblyCopyrightAttribute;
                if (copyrightAttribute != null)
                {
                    copyrightLabel.Text = copyrightAttribute.Copyright;
                }

                AssemblyCompanyAttribute companyAttribute = attributes.FirstOrDefault(a => a is AssemblyCompanyAttribute) as AssemblyCompanyAttribute;
                if (companyAttribute != null)
                {
                    string company = companyAttribute.Company;
                    if (company.StartsWith("http") == false)
                    {
                        company = "http://" + company;
                    }
                    companyLinkLabel.Text = company;
                }
            }

            versionLabel.Text = assembly.GetName().Version.ToString();

            ComponentResourceManager disclaimerResources = new ComponentResourceManager(typeof(DisclaimerForm));
            disclaimerLabel.Text = disclaimerResources.GetString("disclaimerLabel.Text");

            if (assembly.GetName().Version.Major == 0)
            {
                disclaimerLabel.Text += Environment.NewLine + Environment.NewLine +
                    "This is BETA software, and any feature is subject to change at any time!";
            }

            ComponentResourceManager viewerResources = new ComponentResourceManager(typeof(ViewerForm));
            Icon icon = (Icon)(viewerResources.GetObject("$this.Icon"));
            iconPictureBox.Image = icon.ToBitmap();
        }

        private void CompanyLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(companyLinkLabel.Text);
        }
    }
}
