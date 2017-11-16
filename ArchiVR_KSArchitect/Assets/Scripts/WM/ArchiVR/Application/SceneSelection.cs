using Assets.Scripts.WM.UI;
using Assets.Scripts.WM.Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Assets.Scripts.WM.ArchiVR.Application;
using Assets.Scripts.WM.Settings;

namespace Assets.Scripts.WM
{
    public class SceneSelection : MonoBehaviour
    {
        public InputSwipe m_inputSwipe = null;

        public Button m_prevProjectButton = null;
        public Button m_nextProjectButton = null;

        public List<Text> m_selectedProjectNameTextArray = new List<Text>();
        public Text m_textStatus = null;        
        
        public Image m_projectPreview = null;
        public Button m_goButton = null;

        public int m_activeProjectIndex = -1;
        public List<String> m_projectNames = new List<String>();
        public List<Sprite> m_projectImages = new List<Sprite>();

        private void DynamicallyLoadProjects()
        {
            m_projectNames.Clear();
            m_projectImages.Clear();

            for (int i = 0; i < 100; ++i) // Check for Project000 up to to Project100
            {
                var sceneName = "Project" + StringUtil.Get3Digit(i);

                var scenePath = "Assets/Scenes/" + sceneName + ".unity";

                if (SceneUtility.GetBuildIndexByScenePath(scenePath) != -1)
                {
                    m_projectNames.Add(sceneName);

                    var spriteProjectPreviewPath = "ProjectPreview/" + sceneName;
                    var spriteProjectPreview = (Sprite)Resources.Load(spriteProjectPreviewPath, typeof(Sprite));

                    if (null == spriteProjectPreview)
                    {
                        Debug.LogWarning("Project preview Sprite for project '" + sceneName + "' not found!");
                    }

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
            Debug.Log("SceneSelection.Start()");

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

            if (m_goButton)
            {
                Button btn = m_goButton.GetComponent<Button>();
                btn.onClick.AddListener(GoButtonOnClick);
            }

            // Do this at the very end:
            // this makes sure m_selectedProjectName text is not overwritten.
            if (m_inputSwipe)
            {
                //m_textStatus.text = "D:m_inputSwipe!=null";
                m_inputSwipe.SwipeEvent += this.OnSwipe;
            }

            var activeProjectName = ApplicationSettings.GetInstance().m_data.m_stateSettings.m_activeProjectName;
            SetActiveProjectByName(activeProjectName);
        }

        // Update is called once per frame
        void Update()
        {
            //Debug.Log("SceneSelection.Update()");            
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

        void SetActiveProjectByName(String projectNameToActivate)
        {
            for (int projectIndex = 0; projectIndex < m_projectNames.Count; ++ projectIndex)
            {
                var projectName = m_projectNames[projectIndex];

                if (projectName == projectNameToActivate)
                {
                    SetActiveProject(projectIndex);
                    return;
                }
            }

            SetActiveProject(0);
        }

        void SetActiveProject(int index)
        {
            m_activeProjectIndex = MakeValidProjectIndexCycle(index);

            var s = ApplicationSettings.GetInstance().m_data.m_stateSettings;

            if (IsValidProjectIndex(m_activeProjectIndex)) // Could not make it a valid projectIndex
            {
                SetSelectedProjectPreview(m_projectImages[m_activeProjectIndex]);
                SetSelectedProjectName(m_projectNames[m_activeProjectIndex]);
                s.m_activeProjectName = m_projectNames[m_activeProjectIndex]; 
            }
            else
            {
                SetSelectedProjectPreview(null);
                SetSelectedProjectName("No project selected");
                s.m_activeProjectName = "";
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

        void GoButtonOnClick()
        {
            Debug.Log("GoButtonOnClick()");

            if ((m_activeProjectIndex < 0) || (m_activeProjectIndex > m_projectNames.Count))
            {
                m_textStatus.text = "m_activeProjectIndex=" + m_activeProjectIndex;
                return;
            }
            else
                m_textStatus.text = "Opening...";

            var sceneName = m_projectNames[m_activeProjectIndex];

            ApplicationState.OpenProject(sceneName);
        }        
    }
}