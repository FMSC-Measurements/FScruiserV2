﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using FSCruiser.Core.Models;
using System.ComponentModel;

namespace FSCruiser.Core
{
    public enum BackUpMethod { None = 0, LeaveUnit = 1, TimeInterval = 2 }

    [Serializable]
    public class ApplicationSettings : IApplicationSettings
    {
        public enum HotKeyAction
        { None = 0, AddTree, AddPlot, JumpTreeTally, ResequencePlotTrees, UnTally }

        protected const string APP_SETTINGS_PATH = "Settings.xml";

        public event EventHandler CruisersChanged;

        public event Action HotKeysChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        #region static properties

        static ApplicationSettings _instance;

        public static ApplicationSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    Initialize();//TODO remove auto initialization
                }
                return _instance;
            }
            set { _instance = value; }
        }

        public static string ApplicationSettingDirectory
        {
            get
            {
#if WindowsCE || NetCF
                return System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FScruiser"); ;

#else
                return System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FScruiser");
#endif
            }
        }

        public static string ApplicationSettingFilePath
        {
            get
            {
                var dir = ApplicationSettingDirectory;
                return System.IO.Path.Combine(dir, APP_SETTINGS_PATH);
            }
        }

        #endregion static properties

        string _backupDir;
        List<Cruiser> _cruisers;
        List<RecentProject> _recentProjects = new List<RecentProject>();

        public List<RecentProject> RecentProjects
        {
            get { return _recentProjects;  }
            set { _recentProjects = value; }
        }

        public ApplicationSettings()
        {

            EnablePageChangeSound = true;
            EnableTallySound = true;
            EnableAskEnterTreeData = true;

#if NetCF
            AddTreeKeyStr = string.Empty;
#else
            AddTreeKeyStr = "F3";
#endif

            AddPlotKeyStr = "Add"; //plus key
            ResequencePlotTreesKeyStr = string.Empty;
            UntallyKeyStr = string.Empty;
            JumpTreeTallyKeyStr = "Escape";
        }

        #region backup settings

        [XmlAttribute]
        public bool BackUpToCurrentDir { get; set; }

        [XmlAttribute]
        public string BackupDir
        {
            get { return _backupDir; }
            set
            {
                if (!String.IsNullOrEmpty(value) && System.IO.Directory.Exists(value))
                {
                    this._backupDir = value;
                }
                else
                {
                    this._backupDir = null;
                }
            }
        }

        [XmlAttribute]
        public BackUpMethod BackUpMethod { get; set; }

        #endregion backup settings

        bool _keepDeviceAwake = true;

        [XmlAttribute]
        public bool KeepDeviceAwake
        {
            get { return _keepDeviceAwake; }
            set
            {
                _keepDeviceAwake = value;
                NotifyPropertyChanged("KeepDeviceAwake");
            }
        }

        [XmlAttribute]
        public bool EnableCruiserPopup { get; set; }

        [XmlAttribute]
        public bool EnableAskEnterTreeData { get; set; }

        [XmlElement]
        public List<Cruiser> Cruisers
        {
            get
            {
                if (_cruisers == null)
                {
                    _cruisers = new List<Cruiser>();
                }
                return _cruisers;
            }
            set { _cruisers = value; }
        }

        [XmlElement]
        public float DataGridFontSize { get; set; }

        #region sound settings

        [XmlAttribute]
        public bool EnableTallySound { get; set; }

        [XmlAttribute]
        public bool EnablePageChangeSound { get; set; }

        #endregion sound settings

        #region hotkey settings

        Dictionary<string, HotKeyAction> _keyAction = new Dictionary<string, HotKeyAction>();
        Dictionary<HotKeyAction, string> _actionKey = new Dictionary<HotKeyAction, string>();

        void RegisterHotKey(string key, HotKeyAction action)
        {
            _actionKey.Remove(action);
            _keyAction.RemoveByValue(action);

            if (!string.IsNullOrEmpty(key) && key != "None")
            {
                _keyAction.Remove(key);
                _actionKey.RemoveByValue(key);

                _keyAction.Add(key, action);
                _actionKey.Add(action, key);
            }
        }

        [XmlAttribute]
        public string AddPlotKeyStr
        {
            get
            {
                if (_actionKey.ContainsKey(HotKeyAction.AddPlot))
                { return _actionKey[HotKeyAction.AddPlot]; }
                else
                { return null; }
            }
            set
            {
                RegisterHotKey(value, HotKeyAction.AddPlot);
            }
        }

        [XmlAttribute]
        public string AddTreeKeyStr
        {
            get
            {
                if (_actionKey.ContainsKey(HotKeyAction.AddTree))
                { return _actionKey[HotKeyAction.AddTree]; }
                else
                { return null; }
            }
            set
            {
                RegisterHotKey(value, HotKeyAction.AddTree);
            }
        }

        [XmlAttribute]
        public string JumpTreeTallyKeyStr
        {
            get
            {
                if (_actionKey.ContainsKey(HotKeyAction.JumpTreeTally))
                { return _actionKey[HotKeyAction.JumpTreeTally]; }
                else
                { return null; }
            }
            set
            {
                RegisterHotKey(value, HotKeyAction.JumpTreeTally);
            }
        }

        [XmlAttribute]
        public string ResequencePlotTreesKeyStr
        {
            get
            {
                if (_actionKey.ContainsKey(HotKeyAction.ResequencePlotTrees))
                { return _actionKey[HotKeyAction.ResequencePlotTrees]; }
                else
                { return null; }
            }
            set
            {
                RegisterHotKey(value, HotKeyAction.ResequencePlotTrees);
            }
        }

        [XmlAttribute]
        public string UntallyKeyStr
        {
            get
            {
                if (_actionKey.ContainsKey(HotKeyAction.UnTally))
                { return _actionKey[HotKeyAction.UnTally]; }
                else
                { return null; }
            }
            set
            {
                RegisterHotKey(value, HotKeyAction.UnTally);
            }
        }

        #endregion hotkey settings

        public static void Initialize()
        {
            if (File.Exists(ApplicationSettingFilePath))
            {
                var appSettingsPath = ApplicationSettingFilePath;

                _instance = Deserialize(appSettingsPath);
            }
            else
            {
                //previous versions stored app settings in the executing dir
                //so if no app setting file exists check old location

                var oldSettingsPath = System.IO.Path.Combine(GetExecutionDirectory()
                    , APP_SETTINGS_PATH);

                if (File.Exists(oldSettingsPath))
                {
                    _instance = ApplicationSettings.Deserialize(oldSettingsPath);
                    try
                    {
                        System.IO.File.Delete(oldSettingsPath);
                    }
                    catch { }
                }
                else
                {
                    _instance = new ApplicationSettings();
                }
            }
        }

        public static ApplicationSettings Deserialize(string path)
        {
            using (StreamReader reader = new StreamReader(path))
            {
                return Deserialize(reader);
            }
        }

        public static ApplicationSettings Deserialize(TextReader reader)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ApplicationSettings));
            return (ApplicationSettings)serializer.Deserialize(reader);
        }

        public void Save()
        {
            var dir = ApplicationSettingDirectory;

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            var path = ApplicationSettingFilePath;

            using (StreamWriter writer = new StreamWriter(path))
            {
                Serialize(writer, this);
            }
        }

        public static void Serialize(TextWriter writer, ApplicationSettings appSettings)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ApplicationSettings));
            serializer.Serialize(writer, appSettings);
        }

        static string GetExecutionDirectory()
        {
            string name = System.Reflection.Assembly
                .GetCallingAssembly()
                .GetName().CodeBase;

            //clean up path, in FF name is a URI
            if (name.StartsWith(@"file:///"))
            {
                name = name.Replace("file:///", string.Empty);
            }
            string dir = System.IO.Path.GetDirectoryName(name);
            return dir;
        }

        #region Cruisers

        //public CruiserVM[] GetCruiserList()
        //{
        //    if (this.Cruisers == null)
        //    {
        //        return new CruiserVM[0];
        //    }
        //    else
        //    {
        //        return this.Cruisers.ToArray();
        //    }

        //}

        public void AddCruiser(string initials)
        {
            if (this.Cruisers == null)
            {
                this.Cruisers = new List<Cruiser>();
            }
            this.Cruisers.Add(new Cruiser(initials));
        }

        public void RemoveCruiser(Cruiser cruiser)
        {
            this.Cruisers.Remove(cruiser);
        }

        public void NotifyCruisersChanged()
        {
            var cruisersChanged = CruisersChanged;
            if (cruisersChanged != null)
            { cruisersChanged(this, null); }
        }

        public void NotifyHotKeysChanged()
        {
            var hotKeyChanged = HotKeysChanged;
            if (hotKeyChanged != null)
            { hotKeyChanged(); }
        }

        private void NotifyPropertyChanged(string propName)
        {
            var propertyChanged = PropertyChanged;
            if (propertyChanged != null)
            {
                var eArgs = new PropertyChangedEventArgs(propName);
                propertyChanged(this, eArgs);
            }
        }

        #endregion Cruisers

        #region Recent Files

        public void AddRecentProject(RecentProject project)
        {
            if (_recentProjects == null)
                _recentProjects = new List<RecentProject>();

            if (_recentProjects.Contains(project))
                _recentProjects.Remove(project);

            _recentProjects.Insert(0, project);

            if (_recentProjects.Count > 5)
                _recentProjects.RemoveAt(5);
        }

        public void ClearRecentProjects()
        {
            if (RecentProjects != null)
            {
                _recentProjects.Clear();
            }
        }

        #endregion Recent Files
    }
}