//  Copyright(c) 2016-2019 Brian Hansen.

//  Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
//  documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
//  the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
//  and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
    
//  The above copyright notice and this permission notice shall be included in all copies or substantial portions 
//  of the Software.
    
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//  TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//  THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//  CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//  DEALINGS IN THE SOFTWARE.

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

namespace SqlServerQueryTreeViewer
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

            disclaimerLabel.Text += Environment.NewLine + Environment.NewLine +
                SqlServerQueryTreeViewerResources.MITLicense;

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
