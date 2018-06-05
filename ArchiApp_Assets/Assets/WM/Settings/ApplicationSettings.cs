using System;
using System.IO;
#if NON_UWP
using System.Runtime.Serialization.Formatters.Binary;
#endif

using UnityEngine;

namespace Assets.WM.Settings
{
    [Serializable()]
    public class ApplicationSettingsData
    {
        public StateSettings m_stateSettings = new StateSettings();

        public GraphicsSettings m_graphicSettings = new GraphicsSettings();

        public ControlSettings m_controlSettings = new ControlSettings();
    }

    public class ApplicationSettings : MonoBehaviour {

        static ApplicationSettings s_instance = null;

        public ApplicationSettingsData m_data = new ApplicationSettingsData();

        // Use this for initialization
        void Awake()
        {
            Debug.Log("ApplicationSettings.Awake()");

            if (null == s_instance)
            {
                DontDestroyOnLoad(gameObject);
                s_instance = this;
                Load();
            }
            else if (s_instance != this)
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            Debug.Log("ApplicationSettings.Start()");
        }

        // Update is called once per frame
        void Update()
        {
        }

        private string GetFilePath()
        {
            var filePath = Application.persistentDataPath + "/ApplicationSettings.dat";
            return filePath;
        }

        public void Save()
        {
            Debug.Log("ApplicationSettings.Save()");

            //Opens a file and serializes the object into it.
            var fp = GetFilePath();

#if NON_UWP
            var stream = File.Open(fp, FileMode.Create);
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, m_data);
            stream.Close();
#endif
        }

        public void Load()
        {
            Debug.Log("ApplicationSettings.Load(" + GetFilePath() + ")");

            // First see if there is a persistent file with ApplicationSettings present from an earlier application execution.
            var fp = GetFilePath();
            if (!File.Exists(fp))
            {
                // No file present yet -> Create it by saving the default initial Application Settings.
                Save();
                return; // Nothing to load.
            }

            // Then load the Application Settings from the file.
            try
            {
                DoLoad();
            }
            catch (Exception ex0)
            {
                Debug.LogWarning("Failed to load ApplicationSettings.  Re-saving and trying to laod again... " + ex0.Message);
                Save();

                try
                {
                    DoLoad();
                }
                catch(Exception ex1)
                {
                    Debug.LogError("Failed to load ApplicationSettings after re-saving in most recent format! " + ex1.Message);
                    throw ex1;
                }
            }

            // Then push the application settings onto the application.
            m_data.m_graphicSettings.Apply();
        }

        //! Might throw if the file format has changed.
        private void DoLoad()
        {
            var fp = GetFilePath();

#if NON_UWP
            //Opens file and deserializes the object from it.
            var stream = File.Open(fp, FileMode.Open);

            try
            {

                var formatter = new BinaryFormatter();
                m_data = (ApplicationSettingsData)formatter.Deserialize(stream);

                stream.Close();
            }
            catch (Exception e)
            {
                stream.Close();

                throw (e);
            }
#endif
        }

        public static ApplicationSettings GetInstance()
        {
            return s_instance;
        }

        public int GetQualityLevelIndex(string qualityLevelName)
        {
            int i = 0;
            foreach (var name in QualitySettings.names)
            {
                if (name == qualityLevelName)
                    return i;

                ++i;
            }

            return -1; // not found
        }

        public void SetNextGraphicSettingsQualityLevel()
        {
            int qualityLevel = (GetQualityLevelIndex(m_data.m_graphicSettings.m_qualityLevelName) + 1) % QualitySettings.names.Length;
            SetGraphicSettingsQualityLevel(qualityLevel);
        }

        public void SetGraphicSettingsQualityLevel(int qualityLevel)
        {
            Debug.Log("ApplicationSettings.SetGraphicSettingsQualityLevel()");

            if (qualityLevel < 0 || qualityLevel >= QualitySettings.names.Length)
            {
                Debug.Log("Trying to set an invalid quality level! (" + qualityLevel + ")");
            }

            var qualityLevelName = QualitySettings.names[qualityLevel];

            Debug.Log("Set quality level to " + qualityLevel + " (" + qualityLevelName + ")");

            m_data.m_graphicSettings.m_qualityLevelName = qualityLevelName;           

            QualitySettings.SetQualityLevel(qualityLevel);
        }
    }
}
