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
    public class Project
    {
        public string m_name = "";
        public Sprite m_image = null;
    }

    public class ProjectManager : MonoBehaviour
    {
        public List<Project> GetProjects()
        {
            return m_projects;
        }

        public static string GetProjectSceneFolderPath()
        {
            return "Assets/Scenes/Project/";
        }

        public static string GetProjectScenePath(string projectName)
        {
            return GetProjectSceneFolderPath() + projectName + ".unity";
        }

        public List<Project> m_projects = new List<Project>();

        private void DynamicallyLoadProjects()
        {
            m_projects.Clear();
            
            for (int i = 0; i < 100; ++i) // Check for Project000 up to to Project100
            {
                var sceneName = "Project" + StringUtil.Get3Digit(i);

                var scenePath = GetProjectScenePath(sceneName);

                if (SceneUtility.GetBuildIndexByScenePath(scenePath) != -1)
                {
                    Project project = new Project();
                    project.m_name = sceneName;

                    var spriteProjectPreviewPath = "ProjectPreview/" + sceneName;
                    var spriteProjectPreview = (Sprite)Resources.Load(spriteProjectPreviewPath, typeof(Sprite));

                    if (null == spriteProjectPreview)
                    {
                        Debug.LogWarning("Project preview Sprite for project '" + sceneName + "' not found!");
                    }

                    project.m_image = spriteProjectPreview;

                    m_projects.Add(project);
                }
            }
        }

        public int GetProjectIndexByName(string projectName)
        {
            if (projectName == null || projectName == "")
            {
                return -1;
            }

            int index = 0;
            foreach (var project in m_projects)
            {
                if (project.m_name == projectName)
                {
                    return index;
                }
                ++index;
            }

            Debug.LogWarning("Unknown project! (" + projectName + ")");
            return -1;
        }

        public Project GetProjectByName(string projectName)
        {
            int projectIndex = GetProjectIndexByName(projectName);

            return (IsValidProjectIndex(projectIndex) ? m_projects[projectIndex] : null);
        }

        public int GetActiveProjectIndex()
        {
            return GetProjectIndexByName(ApplicationSettings.GetInstance().m_data.m_stateSettings.m_activeProjectName);
        }

        public void ActivateNextProject()
        {
            SetActiveProject(GetActiveProjectIndex() + 1);
        }

        public void ActivatePreviousProject()
        {
            SetActiveProject(GetActiveProjectIndex() - 1);
        }

        public void Awake()
        {
            Debug.Log("ProjectManager.Awake()");

            DynamicallyLoadProjects();
        }

        public void Start()
        {
            Debug.Log("ProjectManager.Start()");

            if (null == GetProjectByName(ApplicationSettings.GetInstance().m_data.m_stateSettings.m_activeProjectName))
            {
                // The active project in the Application settings is unknown.
                if (m_projects.Count > 0)
                {
                    SetActiveProject(0);
                }
                else
                {
                    // There are no projects to activate.
                    ApplicationSettings.GetInstance().m_data.m_stateSettings.m_activeProjectName = null;
                }
            }
        }

        // Validate whether OK to activate given index (true), or not (false).
        bool IsValidProjectIndex(int index)
        {
            if (index < 0)
            {
                return false;
            }

            if (index >= m_projects.Count)
            {
                return false;
            }

            return true; // All checks passed, we have a valid project index!
        }

        // Verify that the given index, is OK to be activated.
        // If OK, return the original index.
        // If not: try to derive a valid index, and return that.
        // If fail to derive a valid index, return -1 as sentinel.
        int MakeValidProjectIndexCycle(int index)
        {
            if (m_projects.Count == 0)
            {
                return -1;
            }

            if (index < 0)
            {
                return m_projects.Count - 1;
            }

            if (index >= m_projects.Count)
            {
                return 0;
            }

            // All checks passed: return the original index, which is OK to activate.
            return index;
        }

        void SetActiveProjectByName(String projectName)
        {
            int index = GetProjectIndexByName(projectName);

            SetActiveProject(index);
        }

        void SetActiveProject(int index)
        {
            Debug.Log("SetActiveProject(" + index + ")");

            int projectIndex = MakeValidProjectIndexCycle(index);

            var s = ApplicationSettings.GetInstance().m_data.m_stateSettings;

            if (IsValidProjectIndex(projectIndex))
            {
                s.m_activeProjectName = m_projects[projectIndex].m_name;
            }
            else
            {
                // Could not make it a valid projectIndex                
                s.m_activeProjectName = "";
            }

            ToastMessageManager.GetInstance().AddToast("Welcome to " + m_projects[projectIndex].m_name);
        }
    }
}