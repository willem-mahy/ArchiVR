using Assets.Scripts.WM.UI;
using Assets.Scripts.WM.Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.WM
{
    public class SceneSelection : MonoBehaviour
    {
        public InputSwipe m_inputSwipe = null;

        public Button m_prevProjectButton = null;
        public Button m_nextProjectButton = null;

        public List<Text> m_selectedProjectNameTextArray = new List<Text>();
        public Text m_textStatus = null;

        public List<Button> m_mainMenuButtons = new List<Button>();
        public Button m_exitButton = null;
        public Image m_projectPreview = null;
        public Button m_goButton = null;

        public int m_activeProjectIndex = -1;
        public List<String> m_projectNames = new List<String>();
        public List<Sprite> m_projectImages = new List<Sprite>();

        private void DynamicallyLoadProjects()
        {
            m_projectNames.Clear();
            m_projectImages.Clear();

            //var numScenes = SceneManager.sceneCountInBuildSettings;

            for (int i = 0; i < 100; ++i) // Check for Project000 up to to Project100
            {
                var sceneName = "Project" + StringUtil.Get3Digit(i);

                var scenePath = "Assets/Scenes/" + sceneName + ".unity";

                if (SceneUtility.GetBuildIndexByScenePath(scenePath) != -1)
                {
                    m_projectNames.Add(sceneName);

                    var spriteProjectPreviewPath = "ProjectPreview/" + sceneName;
                    var spriteProjectPreview = (Sprite)Resources.Load(spriteProjectPreviewPath, typeof(Sprite));
                    m_projectImages.Add(spriteProjectPreview);
                }
            }
        }

        public void OnSwipe(object sender, SwipeEventArgs e)
        {
            m_textStatus.text = "OnSwipe(" + e.dir + ")";

            switch (e.dir)
            {
                case SwipeDirection.up:
                    SetActiveProject(--m_activeProjectIndex);
                    break;
                case SwipeDirection.down:
                    SetActiveProject(++m_activeProjectIndex);
                    break;
                case SwipeDirection.left:
                    SetActiveProject(++m_activeProjectIndex);
                    break;
                case SwipeDirection.right:
                    SetActiveProject(--m_activeProjectIndex);
                    break;
            }
        }

        // Use this for initialization
        void Start()
        {
            DynamicallyLoadProjects();

            if (m_prevProjectButton)
            {
                Button btn = m_prevProjectButton.GetComponent<Button>();
                btn.onClick.AddListener(PrevProjectButtonOnClick);
            }

            if (m_nextProjectButton)
            {
                Button btn = m_nextProjectButton.GetComponent<Button>();
                btn.onClick.AddListener(NextProjectButtonOnClick);
            }

            foreach (var button in m_mainMenuButtons)
            {
                if (null == button)
                {
                    continue;
                }

                button.onClick.AddListener(MainMenuButtonOnClick);
            }

            if (m_exitButton)
            {
                Button btn = m_exitButton.GetComponent<Button>();
                btn.onClick.AddListener(ExitButtonOnClick);
            }

            if (m_goButton)
            {
                Button btn = m_goButton.GetComponent<Button>();
                btn.onClick.AddListener(GoButtonOnClick);
            }

            SetActiveProject(0);

            // Do this at the very end:
            // this makes sure m_selectedProjectName text is not overwritten.
            if (m_inputSwipe)
            {
                //m_textStatus.text = "D:m_inputSwipe!=null";
                m_inputSwipe.SwipeEvent += this.OnSwipe;
            }
        }

        // Update is called once per frame
        void Update()
        {
            //m_selectedProjectName.text = "WMSceneSelection.Update();";

            if (m_exitButton != null && Input.GetKey("escape"))
            {
                Quit();
            }
        }

        void MainMenuButtonOnClick()
        {
            Debug.Log("Main Menu Button clicked.");
            LoadMainMenu();
        }

        public void LoadMainMenu()
        {
            if (ViewProject.IsActiveViewModeVR())
            {
                SceneManager.LoadScene("MainMenu_VR");
            }
            else
            {
                SceneManager.LoadScene("MainMenu");
            }
        }

        // Validate whether OK to activate given index (true), or not (false).
        bool IsValidProjectIndex(int index)
        {
            if (index < 0)
            {
                return false;
            }

            if (index >= m_projectNames.Count)
            {
                return false;
            }

            return true; // All checks passed, OK to activate index!
        }

        // Verify that the given index, is OK to be activated.
        // If OK, return the original index.
        // If not: try to derive a valid index, and return that.
        // If fail to derive a valid index, return -1 as sentinel.
        int MakeValidProjectIndexCycle(int index)
        {
            if (m_projectNames.Count == 0)
            {
                return -1;
            }

            if (index < 0)
            {
                return m_projectNames.Count - 1;
            }

            if (index >= m_projectNames.Count)
            {
                return 0;
            }

            // All checks passed: return the original index, which is OK to activate.
            return index;
        }

        void SetActiveProject(int index)
        {
            m_activeProjectIndex = MakeValidProjectIndexCycle(index);

            if (IsValidProjectIndex(m_activeProjectIndex)) // Could not make it a valid projectIndex
            {
                SetSelectedProjectPreview(m_projectImages[m_activeProjectIndex]);
                SetSelectedProjectName(m_projectNames[m_activeProjectIndex]);
            }
            else
            {
                SetSelectedProjectPreview(null);
                SetSelectedProjectName("No project selected");
            }
        }

        private void SetSelectedProjectPreview(Sprite spriteProjectPreview)
        {
            if (!m_projectPreview)
            {
                return;
            }

            m_projectPreview.sprite = spriteProjectPreview;
        }

        private void SetSelectedProjectName(String text)
        {
            foreach (var selectedProjectNameText in m_selectedProjectNameTextArray)
            {
                selectedProjectNameText.text = text;
            }
        }

        void PrevProjectButtonOnClick()
        {
            Debug.Log("Previous Project button clicked.");

            var newActiveProjectIndex = (m_projectNames.Count == 0) ? -1 : --m_activeProjectIndex;

            SetActiveProject(newActiveProjectIndex);
        }

        void NextProjectButtonOnClick()
        {
            Debug.Log("Next Project button clicked.");

            var newActiveProjectIndex = (m_projectNames.Count == 0) ? -1 : ++m_activeProjectIndex;

            SetActiveProject(newActiveProjectIndex);
        }

        void ExitButtonOnClick()
        {
            Debug.Log("Exit button clicked.");
            Quit();
        }

        private void Quit()
        {
            m_textStatus.text = "Exiting... ";

            Application.Quit();
        }

        void GoButtonOnClick()
        {
            Debug.Log("Go Project button clicked.");
            if ((m_activeProjectIndex < 0) || (m_activeProjectIndex > m_projectNames.Count))
            {
                m_textStatus.text = "m_activeProjectIndex=" + m_activeProjectIndex;
                return;
            }
            else
                m_textStatus.text = "Opening...";

            var sceneName = m_projectNames[m_activeProjectIndex];
            SceneManager.LoadScene(sceneName);
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene("ViewProject", LoadSceneMode.Additive);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode arg1)
        {
            var sceneName = m_projectNames[m_activeProjectIndex];

            var sp = SceneManager.GetSceneByName(sceneName);

            if (!sp.IsValid())
                return;

            if (!sp.isLoaded)
                return;

            var svp = SceneManager.GetSceneByName("ViewProject");

            if (!svp.IsValid())
                return;

            if (svp.isLoaded == false)
                return;

            var textProjectName = GameObject.Find("TextProjectName");

            if (textProjectName)
            {
                var text = textProjectName.GetComponent<Text>();

                if (text)
                {
                    text.text = m_projectNames[m_activeProjectIndex];
                }
            }

            var gameObjects = sp.GetRootGameObjects();

            var gameObjectWorld = gameObjects[0];

            if (gameObjectWorld)
            {
                SceneManager.MoveGameObjectToScene(gameObjectWorld, svp);
                SceneManager.SetActiveScene(svp);
                SceneManager.UnloadSceneAsync(sp);
            }
        }
    }
}