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
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using bkh.ParseTreeLib;

namespace SqlServerQueryTreeViewer
{
    [DataContract]
    class ViewerSettings
    {
        private const int _maxRecentSqlServers = 20;
        private const int _maxRecentQueries = 10;
        private static string _xmlFileName = GetXmlFileName();
        private static string _tempFileName = Path.Combine(
            Path.GetDirectoryName(_xmlFileName),
            Path.GetFileNameWithoutExtension(_xmlFileName) + ".tmp");
        private static bool _userSettingsOverwriteDecline = false;

        private static ViewerSettings _instance;
        private static object _instanceLocker = new object();

        private static ViewerSettings _clone;
        private static object _cloneLocker = new object();

        [DataMember]
        private List<string> _mostRecentSqlServers;

        [DataMember]
        private List<OperatorColor> _operatorColors;

        [DataMember]
        private List<SubmittedQueryInfo> _recentQueries;

        private ViewerSettings()
        {
            _mostRecentSqlServers = new List<string>();
            _operatorColors = new List<OperatorColor>();
            _recentQueries = new List<SubmittedQueryInfo>();
        }

        public static ViewerSettings Instance
        {
            get
            {
                lock(_instanceLocker)
                {
                    if (_instance == null)
                    {
                        _instance = Load();
                    }
                    return _instance;
                }
            }
        }

        public static ViewerSettings Clone
        {
            get
            {
                lock(_cloneLocker)
                {
                    if (_clone == null)
                    {
                        _clone = GetClone(Instance);
                    }
                    return _clone;
                }
            }
        }

        [DataMember]
        public bool UserAgreesToDisclaimer
        {
            get;
            set;
        }

        [DataMember]
        public string CurrentQuery
        {
            get;
            set;
        }

        [DataMember]
        public string CurrentQueryRtf
        {
            get;
            set;
        }

        [DataMember]
        public string CurrentFileName
        {
            get;
            set;
        }

        [DataMember]
        public List<TraceFlag> TraceFlags
        {
            get;
            set;
        }

        public bool TraceFlagListHasBeenEdited
        {
            get;
            set;
        }

        public ReadOnlyCollection<string> MostRecentSqlServers
        {
            get
            {
                return new ReadOnlyCollection<string>(_mostRecentSqlServers);
            }
        }

        public ReadOnlyCollection<SubmittedQueryInfo> RecentQueries
        {
            get
            {
                return new ReadOnlyCollection<SubmittedQueryInfo>(_recentQueries);
            }
        }
        
        public ReadOnlyCollection<OperatorColor> OperatorColors
        {
            get
            {
                return new ReadOnlyCollection<OperatorColor>(_operatorColors);
            }
        }

        [DataMember]
        public bool TrackOptimizerInfo
        {
            get;
            set;
        }

        [DataMember]
        public bool TrackTransformationStats
        {
            get;
            set;
        }

        [DataMember]
        public TreeRenderStyle TreeRenderStyle
        {
            get;
            set;
        }

        [DataMember]
        public bool? HideLowValueLeafLevelNodes
        {
            get;
            set;
        }

        public void Save()
        {
            if (_userSettingsOverwriteDecline == true)
            {
                return;
            }

            using (FileStream stream = new FileStream(_tempFileName, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(ViewerSettings));
                serializer.WriteObject(stream, this);
            }
            if (File.Exists(_xmlFileName))
            {
                File.Delete(_xmlFileName);
            }
            File.Move(_tempFileName, _xmlFileName);
        }

        public void AddMostRecentSqlServer(string serverName)
        {
            if (string.IsNullOrEmpty(serverName))
            {
                throw new ArgumentNullException(nameof(serverName));
            }

            // If already in the list, move it to the top
            if (_mostRecentSqlServers.Contains(serverName))
            {
                _mostRecentSqlServers.Remove(serverName);
                _mostRecentSqlServers.Insert(0, serverName);
            }
            else
            {
                _mostRecentSqlServers.Insert(0, serverName);
            }

            if (_mostRecentSqlServers.Count > _maxRecentSqlServers)
            {
                _mostRecentSqlServers = _mostRecentSqlServers.Take(_maxRecentSqlServers).ToList();
            }
        }

        public void AddOperatorColor(string operatorName, Color color)
        {
            if (string.IsNullOrEmpty(operatorName))
            {
                throw new ArgumentNullException(nameof(operatorName));
            }

            OperatorColor operatorColor = _operatorColors.FirstOrDefault(o => o.OperatorName == operatorName);
            if (operatorColor == null)
            {
                operatorColor = new OperatorColor();
                operatorColor.OperatorName = operatorName;
                operatorColor.DisplayColor = color;

                _operatorColors.Add(operatorColor);
            }
            else
            {
                operatorColor.DisplayColor = color;
            }
        }

        public void DeleteOperatorColor(string operatorName)
        {
            if (string.IsNullOrEmpty(operatorName))
            {
                throw new ArgumentNullException(nameof(operatorName));
            }

            OperatorColor operatorColor;
            while ((operatorColor = _operatorColors.FirstOrDefault(o => o.OperatorName == operatorName)) != null)
            {
                _operatorColors.Remove(operatorColor);
            }
        }

        public Color? GetOperatorColor(string operatorName)
        {
            if (string.IsNullOrEmpty(operatorName))
            {
                throw new ArgumentNullException(nameof(operatorName));
            }

            OperatorColor operatorColor = _operatorColors.FirstOrDefault(o => o.OperatorName == operatorName);
            return operatorColor?.DisplayColor;
        }

        public void AddRecentQuery(string queryText, string rtf)
        {
            if (string.IsNullOrEmpty(queryText) == false)
            {
                SubmittedQueryInfo match = null;
                while ((match = _recentQueries.FirstOrDefault(q => q.QueryText == queryText)) != null)
                {
                    _recentQueries.Remove(match);
                }

                _recentQueries.Add(new SubmittedQueryInfo(queryText, rtf));
                while (_recentQueries.Count > _maxRecentQueries)
                {
                    _recentQueries.RemoveAt(0);
                }
            }
        }

        public static void CancelClone()
        {
            lock(_cloneLocker)
            {
                _clone = null;
            }
        }

        public static void PromoteClone()
        {
            lock(_instanceLocker)
            {
                _instance = Clone;
                lock (_cloneLocker)
                {
                    _clone = null;
                }
            }
        }

        private static ViewerSettings GetClone(ViewerSettings instance)
        {
            lock(_instanceLocker)
            {
                return Utilities.CreateClone(_instance);
            }
        }

        [OnDeserialized]
        private void PostDeserialize(StreamingContext context)
        {
            SetDefaultValues();
        }

        private void SetDefaultValues()
        {
            if (_operatorColors == null)
            {
                _operatorColors = new List<OperatorColor>();
            }

            if (_recentQueries == null)
            {
                _recentQueries = new List<SubmittedQueryInfo>();
            }

            if (TreeRenderStyle == TreeRenderStyle.NotSpecified)
            {
                Version version = Assembly.GetExecutingAssembly().GetName().Version;
                if (version.Major == 0 && version.Minor < 7)
                {
                    TreeRenderStyle = TreeRenderStyle.VerticalBalancedTree;
                }
                else
                {
                    TreeRenderStyle = TreeRenderStyle.PlanStyleHorizontalTree;
                }
            }

            if (HideLowValueLeafLevelNodes == null)
            {
                HideLowValueLeafLevelNodes = true;
            }
        }

        private static ViewerSettings Load()
        {
            if (File.Exists(_xmlFileName) == false ||
                _userSettingsOverwriteDecline == true)
            {
                ViewerSettings settings = new ViewerSettings();
                settings.SetDefaultValues();
                return settings;
            }

            try
            {
                using (FileStream stream = new FileStream(_xmlFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    DataContractSerializer serializer = new DataContractSerializer(typeof(ViewerSettings));
                    ViewerSettings settings = serializer.ReadObject(stream) as ViewerSettings;
                    return settings;
                }
            }
            catch (Exception ex)
            {
                string lineSpacing = Environment.NewLine + Environment.NewLine;
                string message = string.Format(
                    "The system was not able to load the current settings.  The specific error is: {0}." + lineSpacing +
                        "Do you want to revert to default settings?" + lineSpacing +
                        "Select Yes if you want to revert to the default settings.  You will likely lose any custom settings if you choose this option." + lineSpacing +
                        "Select No to end the program and manually correct the problem.",
                    ex.Message);
                DialogResult result = MessageBox.Show(
                    message,
                    "Unable to load settings",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2);
                if (result == DialogResult.No)
                {
                    _userSettingsOverwriteDecline = true;
                    Application.Exit();
                }

                ViewerSettings settings = new ViewerSettings();
                settings.SetDefaultValues();
                return settings;
            }
        }

        private static string GetXmlFileName()
        {
            string path = Properties.Settings.Default.SettingsXmlFileName;
            if (Path.GetDirectoryName(path) == string.Empty)
            {
                return Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    path);
            }
            return path;
        }
    }
}
