using Assets.ArchiApp.Application.Entities;
using Assets.ArchiApp.Application.Managers;
using Assets.WM.Settings;
using UnityEngine;

namespace Assets.ArchiApp.Application.UI
{
    // TODO: Rename to ProjectSelectionBase...
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