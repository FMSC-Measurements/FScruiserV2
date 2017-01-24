using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using FSCruiser.Core.Models;
using System.Windows.Forms;
using System.ComponentModel;

namespace FSCruiser.Core
{
    [Serializable]
    public class ApplicationSettings 
    {
        #region static properties
        static KeysConverter _keyConverter = new KeysConverter();
        static ApplicationSettings _instance;

        public static ApplicationSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    Initialize();
                }
                return _instance;
            }
        }

        public static string ApplicationSettingDirectory
        {
            get
            {
#if NetCF
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
                return System.IO.Path.Combine(dir, Constants.APP_SETTINGS_PATH);
            }
        }

        public static IExceptionHandler ExceptionHandler { get; set; }

        #endregion static properties

        string _backupDir;
        List<Cruiser> _cruisers;

        public List<RecentProject> RecentProjects { get; set; }

        

        public ApplicationSettings()
        {
            RecentProjects = new List<RecentProject>();

            EnablePageChangeSound = true;
            EnableTallySound = true;

            
#if NetCF
            AddPlotKey = Keys.None;
#else
            AddPlotKey = Keys.F3;
#endif

            AddPlotKey = Keys.Add;
            ResequencePlotTreesKey = Keys.None;
            UntallyKey = Keys.None;
            JumpTreeTallyKey = Keys.Escape;

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
        #endregion 

        [XmlAttribute]
        public bool EnableCruiserPopup { get; set; }

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
        #endregion

        #region hotkey settings
        [XmlAttribute]
        public string AddPlotKeyStr
        {
            get { return _keyConverter.ConvertToString(AddPlotKey); }
            set
            {
                AddPlotKey = ParseKey(value, Keys.None);
            }
        }

        [XmlAttribute]
        public string AddTreeKeyStr
        {
            get { return _keyConverter.ConvertToString(AddTreeKey); }
            set
            {
                AddTreeKey = ParseKey(value, Keys.None);
            }
        }

        [XmlAttribute]
        public string JumpTreeTallyKeyStr
        {
            get { return _keyConverter.ConvertToString(JumpTreeTallyKey); }
            set
            {
                JumpTreeTallyKey = ParseKey(value, Keys.Escape);
            }
        }

        [XmlAttribute]
        public string ResequencePlotTreesKeyStr
        {
            get { return _keyConverter.ConvertToString(ResequencePlotTreesKey); }
            set
            {
                ResequencePlotTreesKey = ParseKey(value, Keys.None);
            }
        }

        [XmlAttribute]
        public string UntallyKeyStr
        {
            get { return _keyConverter.ConvertToString(UntallyKey); }
            set
            {
                UntallyKey = ParseKey(value, Keys.None);
            }
        }

        [XmlIgnore]
        public Keys AddPlotKey { get; set; }

        [XmlIgnore]
        public Keys AddTreeKey { get; set; }

        [XmlIgnore]
        public Keys ResequencePlotTreesKey { get; set; }

        [XmlIgnore]
        public Keys UntallyKey { get; set; }

        [XmlIgnore]
        public Keys JumpTreeTallyKey { get; set; }
        #endregion


        public static void Initialize()
        {
            try
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
                        , Constants.APP_SETTINGS_PATH);

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
            catch (Exception e)
            {
                ExceptionHandler.HandelEx(new UserFacingException("Fail to load application settings", e));
                _instance = new ApplicationSettings();
            }
        }

        public static ApplicationSettings Deserialize(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ApplicationSettings));
            using (StreamReader reader = new StreamReader(path))
            {
                return (ApplicationSettings)serializer.Deserialize(reader);
            }
        }

        static Keys ParseKey(String value, Keys defVal)
        {
            try
            {
                return (Keys)_keyConverter.ConvertFromString(value);
            }
            catch
            {
                return defVal;
            }
        }

        public static void Save()
        {
            if (_instance == null)
            {
                return;
            }
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ApplicationSettings));
                var dir = ApplicationSettingDirectory;

                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                var path = ApplicationSettingFilePath;

                using (StreamWriter writer = new StreamWriter(path))
                {
                    serializer.Serialize(writer, _instance);
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.HandelEx(new UserFacingException("Unabel to save user settings", e));
            }
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

        public event EventHandler CruisersChanged;

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
            if(cruisersChanged != null)
            { cruisersChanged(this, null); }
        }

        #endregion Cruisers

        #region Recent Files

        public void AddRecentProject(RecentProject project)
        {
            if (RecentProjects == null)
                RecentProjects = new List<RecentProject>();

            if (RecentProjects.Contains(project))
                RecentProjects.Remove(project);

            RecentProjects.Insert(0, project);

            if (RecentProjects.Count > 5)
                RecentProjects.RemoveAt(5);
        }

        public void ClearRecentProjects()
        {
            if (RecentProjects != null)
            {
                RecentProjects.Clear();
            }
        }

        #endregion Recent Files
    }
}