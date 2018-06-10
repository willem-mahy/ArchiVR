using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ArchiAR_ARCore_AR
{
    public class Project
    {
        public string m_name = null;
        public string m_scenePath = null;
    }

    public class ProjectManager : MonoBehaviour
    {
        public string m_projectScenesAssetFolderPath = "KS/Projects/Scenes/";

        private int m_activeProjectIndex = -1;

        private List<Project> m_projects = new List<Project>();

        // Use this for initialization
        void Start()
        {
            DynamicallyLoadProjects();
        }

        private void DynamicallyLoadProjects()
        {
            m_projects.Clear();

            for (int i = 0; i < 100; ++i) // Check for Project000 up to to Project100
            {
                var sceneName = "Project" + StringUtil.Get3Digit(i);

                var scenePath = m_projectScenesAssetFolderPath + sceneName;

                if (SceneUtility.GetBuildIndexByScenePath(scenePath) != -1)
                {
                    Project project = new Project();
                    project.m_name = sceneName;
                    project.m_scenePath = m_projectScenesAssetFolderPath + sceneName;

                    m_projects.Add(project);
                }
            }

            m_activeProjectIndex = (m_projects.Count == 0 ? -1 : 0);
        }

        public void NextProject()
        {
            m_activeProjectIndex = Math.Min(++m_activeProjectIndex, m_projects.Count - 1);
        }

        public void PreviousProject()
        {
            m_activeProjectIndex = Math.Max(--m_activeProjectIndex, 0);
        }

        public Project GetActiveProject()
        {
            if (m_activeProjectIndex == -1)
            {
                return null;
            }

            return m_projects[m_activeProjectIndex];
        }
    }
}
