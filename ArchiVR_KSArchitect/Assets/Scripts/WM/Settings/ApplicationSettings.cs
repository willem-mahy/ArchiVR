using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Assets.Scripts.WM
{
    [System.Serializable()]
    public class ApplicationSettingsData
    {
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
            var stream = File.Open(GetFilePath(), FileMode.Create);
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, m_data);
            stream.Close();
        }

        public void Load()
        {
            Debug.Log("ApplicationSettings.Load(" + GetFilePath() + ")");

            // First load the application settings.
            if (!File.Exists(GetFilePath()))
            {
                Save();
                return; // Nothing to load.
            }

            //Opens file and deserializes the object from it.
            var stream = File.Open(GetFilePath(), FileMode.Open);
            var formatter = new BinaryFormatter();

            m_data = (ApplicationSettingsData)formatter.Deserialize(stream);

            stream.Close();

            // Then push the application settings onto the application.
            m_data.m_graphicSettings.Apply();            
        }

        public static ApplicationSettings GetInstance()
        {
            return s_instance;
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
