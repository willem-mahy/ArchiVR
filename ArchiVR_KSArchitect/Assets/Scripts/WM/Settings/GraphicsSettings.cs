using UnityEngine;

namespace Assets.Scripts.WM.Settings
{
    [System.Serializable()]
    public class GraphicsSettings
    {
        public bool m_enableDynamicGrass = true;

        public bool m_showFPS = false;

        public string m_qualityLevelName = null;

        public void Apply()
        {
            ApplyQualityLevel();
        }

        private void ApplyQualityLevel()
        {
            Debug.Log("GraphicsSettings.ApplyQualityLevel()");

            int qualityLevel = 0;
            foreach (var name in QualitySettings.names)
            {
                if (name == m_qualityLevelName)
                {
                    Debug.Log("Applying stored QualityLevel " + qualityLevel + " (" + m_qualityLevelName + ") from GraphicSettings.");
                    QualitySettings.SetQualityLevel(qualityLevel);
                    return;
                }
                ++qualityLevel;
            }

            // Application does not support a quality level with the quality level name that is stored in the GraphicSettings.
            // So update the quality level name in the GraphicSettings to the name of the current active quality level of the application instead.
            Debug.LogWarning("Application does not support QualityLevel stored in GraphicsSettings (" + m_qualityLevelName + "): updating QualityLevel in GraphicsSettings to QualityLevel currently active in Application (" + QualitySettings.names[QualitySettings.GetQualityLevel()] + ")!");
            m_qualityLevelName = QualitySettings.names[QualitySettings.GetQualityLevel()];
        }
    }
}
