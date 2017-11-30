using UnityEngine;
using UnityEngine.SceneManagement;

using Assets.Scripts.WM.UI;
using Assets.Scripts.WM.Settings;

namespace Assets.Scripts.WM.ArchiVR.Application
{
    public class ApplicationStatePlay : ApplicationState
    {
        // For debugging purposes: allows to start up in 'Play' state,
        // with the project designated in 'm_initialProjectSceneName'.
        private static bool s_firstTime = true;

        // For debugging purposes: allows to start up in 'Play' state,
        // with the project designated in 'm_initialProjectSceneName'.
        public string m_initialProjectSceneName = "";

        override protected string GetName()
        {
            return "Play";
        }

        // Use this for initialization
        override protected void Start()
        {
            base.Start();

            // For debugging purposes: enables to open an initial project (defined by 'm_initialProjectSceneName') when starting the application in 'Play' mode.
            if (s_firstTime)
            {
                s_firstTime = false;

                if (m_initialProjectSceneName.Length > 0)
                {
                    OpenProject(m_initialProjectSceneName);
                }
            }

            GameObject.Find("Time").GetComponent<TimeBehavior>().m_time = 60 * 60 * 12;

            GetComponent<POISelection>().ActivateNextPOI();
        }

        // Update is called once per frame
        override protected void Update()
        {
            base.Update();

            // If user presses 'p', Write the current camera location as POI.
            if (Input.GetKeyDown("p"))
            {
                WritePOI();
            }

            // If user presses 'c', toggle Construction Lighting Mode.
            if (Input.GetKeyDown("c"))
            {
                GetComponent<ConstructionLighting>().ActivateNextLightingMode();
            }

            // If user presses 'Q', toggle Graphics Quality Mode.
            if (Input.GetKeyDown("q"))
            {
                ApplicationSettings.GetInstance().SetNextGraphicSettingsQualityLevel();
            }
        }

        public void HomeButton_OnClick()
        {
            Debug.Log("ApplicationStatePlay.HomeButton_OnClick()");
            OpenHomeMenu();
        }

        public void MenuTimeButton_OnClick()
        {
            Debug.Log("ApplicationStatePlay.MenuTimeButton_OnClick()");

            //UIManager.GetInstance().OpenMenu("MenuTime");
            UIManager.GetInstance().OpenMenu(GameObject.Find("MenuTime").GetComponent<Assets.Scripts.WM.UI.Menu>());
        }

        public void MenuLayerButton_OnClick()
        {
            Debug.Log("ApplicationStatePlay.MenuLayerButton_OnClick()");

            //UIManager.GetInstance().OpenMenu("MenuLayer");
            UIManager.GetInstance().OpenMenu(GameObject.Find("MenuLayer").GetComponent<Assets.Scripts.WM.UI.Menu>());
        }

        public void OpenHomeMenu()
        {
            SceneManager.LoadScene("Home");
        }
        
        public void WritePOI()
        {
            var camera = Camera.main;
            var name = GetComponent<POISelection>().GetActivePOI().name;
            var position = camera.transform.position.ToString();
            var rotation = camera.transform.rotation.eulerAngles.ToString();

            var text =
                System.Environment.NewLine +
                "POI" +
                " Name: " + name +
                " Pos:" + position +
                " Rot:" + rotation;

            var projectName = ApplicationSettings.GetInstance().m_data.m_stateSettings.m_activeProjectName;

            var filePath = UnityEngine.Application.persistentDataPath + "\\poi_" + projectName + ".txt";
            System.IO.File.AppendAllText(filePath, text);
        }
    }
}
