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
    abstract public class SceneSelectionBase : MonoBehaviour
    {
        public ProjectManager m_projectManager = null;
                
        virtual public void Update()
        {
            //Debug.Log("SceneSelectionBase.Update()");        
            var s = ApplicationSettings.GetInstance().m_data.m_stateSettings;

            if (null == m_projectManager)
            {
                Debug.LogError("null == SceneSelectionBase.m_projectManager");
            }
            else
            {
                var activeProject = m_projectManager.GetProjectByName(s.m_activeProjectName);
                SetActiveProject(activeProject);
            }
        }
        
        abstract protected void SetActiveProject(Project project);           
    }
}