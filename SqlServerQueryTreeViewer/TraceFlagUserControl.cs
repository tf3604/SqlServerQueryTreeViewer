//  Copyright(c) 2016-2017 Brian Hansen.

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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SqlServerQueryTreeViewer
{
    public partial class TraceFlagUserControl : UserControl
    {
        private BindingSource _bindingSource = null;
        private BindingList<TraceFlag> _boundTraceFlags = null;
        private string _initialSignature;
        private bool _inChangeHandler = false;

        public TraceFlagUserControl()
        {
            InitializeComponent();
        }

        private void TraceFlagUserControl_Load(object sender, EventArgs e)
        {
            SyncTraceFlagsToDefault();
            ViewerSettings.Clone.TraceFlags.Sort((x, y) => x.Order.CompareTo(y.Order));

            if (_bindingSource == null)
            {
                _boundTraceFlags = new BindingList<TraceFlag>();
                ViewerSettings.Clone.TraceFlags.ForEach(tf => _boundTraceFlags.Add(tf));

                _bindingSource = new BindingSource();
                _bindingSource.DataSource = _boundTraceFlags;

                traceFlagGrid.AutoGenerateColumns = false;
                traceFlagGrid.Columns.Clear();
                traceFlagGrid.DataSource = _bindingSource;

                DataGridViewColumn column = new DataGridViewCheckBoxColumn();
                column.Name = "Enabled";
                column.DataPropertyName = "Enabled";
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                traceFlagGrid.Columns.Add(column);

                column = new DataGridViewTextBoxColumn();
                column.Name = "TF";
                column.DataPropertyName = "TraceFlagNumber";
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                column.ReadOnly = true;
                traceFlagGrid.Columns.Add(column);

                column = new DataGridViewTextBoxColumn();
                column.Name = "Description";
                column.DataPropertyName = "Description";
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                column.ReadOnly = true;
                traceFlagGrid.Columns.Add(column);
            }

            _initialSignature = ComputeListSignature();
            ParentForm.FormClosing += ParentForm_FormClosing;
        }

        private void SyncTraceFlagsToDefault()
        {
            if (ViewerSettings.Clone.TraceFlags == null)
            {
                ViewerSettings.Clone.TraceFlags = new List<TraceFlag>();
            }

            foreach (TraceFlag traceFlag in TraceFlag.DefaultTraceFlagList)
            {
                TraceFlag existingTraceFlag = ViewerSettings.Clone.TraceFlags.FirstOrDefault(tf => tf.TraceFlagNumber == traceFlag.TraceFlagNumber);
                if (existingTraceFlag != null)
                {
                    existingTraceFlag.Description = traceFlag.Description;
                }
                else
                {
                    ViewerSettings.Clone.TraceFlags.Add(traceFlag);
                }
            }

            // Delete any trace flag entries not in the default list.
            ViewerSettings.Clone.TraceFlags.Except(TraceFlag.DefaultTraceFlagList).ToList().ForEach(tf => ViewerSettings.Clone.TraceFlags.Remove(tf));
        }

        private void ParentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            string finalSignature = ComputeListSignature();
            ViewerSettings.Clone.TraceFlagListHasBeenEdited = finalSignature != _initialSignature;
        }

        private static string ComputeListSignature()
        {
            StringBuilder sb = new StringBuilder();
            if (ViewerSettings.Clone.TraceFlags == null)
            {
                return string.Empty;
            }
            List<TraceFlag> traceFlags = ViewerSettings.Clone.TraceFlags.ToList();
            traceFlags.Sort((x, y) => x.TraceFlagNumber.CompareTo(y.TraceFlagNumber));
            traceFlags.ForEach(tf => sb.Append(tf.TraceFlagNumber.ToString() + "=" + tf.Enabled.ToString() + ";"));
            return sb.ToString();
        }

        private void TraceFlagGrid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (_inChangeHandler == false)
            {
                try
                {
                    _inChangeHandler = true;
                    traceFlagGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);

                    int row = traceFlagGrid.CurrentCell.RowIndex;
                    DataGridViewCell cell = traceFlagGrid["TF", row];
                    int traceFlagNumber = (int)cell.Value;
                    bool enabled = (bool)traceFlagGrid.CurrentCell.Value;

                    TraceFlag traceFlag = _boundTraceFlags.FirstOrDefault(tf => tf.TraceFlagNumber == traceFlagNumber);

                    // Check that any parent trace flags are also enabled
                    if (enabled == true)
                    {
                        if (traceFlag.ParentTraceFlag != null)
                        {
                            TraceFlag parentTraceFlag = _boundTraceFlags.FirstOrDefault(tf => tf.TraceFlagNumber == traceFlag.ParentTraceFlag.Value);
                            if (parentTraceFlag != null)
                            {
                                if (parentTraceFlag.Enabled == false)
                                {
                                    parentTraceFlag.Enabled = true;
                                    _boundTraceFlags.ResetBindings();
                                }
                            }
                        }
                    }

                    // Check that any child trace flags are disabled
                    if (enabled == false)
                    {
                        List<TraceFlag> childTraceFlags = _boundTraceFlags.Where(tf => tf.ParentTraceFlag == traceFlag.TraceFlagNumber).ToList();
                        int itemsChanged = 0;
                        foreach (TraceFlag childTraceFlag in childTraceFlags)
                        {
                            if (childTraceFlag.Enabled == true)
                            {
                                childTraceFlag.Enabled = false;
                                itemsChanged++;
                            }
                        }
                        if (itemsChanged > 0)
                        {
                            _boundTraceFlags.ResetBindings();
                        }
                    }
                }
                finally
                {
                    _inChangeHandler = false;
                }
            }
        }
    }
}
