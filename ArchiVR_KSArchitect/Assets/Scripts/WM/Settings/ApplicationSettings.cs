using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization;
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

        static ApplicationSettings s_applicationSettings = null;

        public ApplicationSettingsData m_data = new ApplicationSettingsData();

        // Use this for initialization
        void Awake()
        {
            if (null == s_applicationSettings)
            {
                DontDestroyOnLoad(gameObject);
                s_applicationSettings = this;
            }
            else if (s_applicationSettings != this)
            {
                Destroy(gameObject);
            }
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
            Debug.Log("ApplicationSettings.Load()");

            if (!File.Exists(GetFilePath()))
            {
                return; // Nothing to load.
            }

            //Opens file and deserializes the object from it.
            var stream = File.Open(GetFilePath(), FileMode.Open);
            var formatter = new BinaryFormatter();

            m_data = (ApplicationSettingsData)formatter.Deserialize(stream);

            stream.Close();
        }
    }
}
