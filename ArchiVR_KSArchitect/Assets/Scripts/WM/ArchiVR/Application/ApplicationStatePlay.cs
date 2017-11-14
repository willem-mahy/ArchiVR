using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using Assets.Scripts.WM.UI;
using UnityEngine.EventSystems;

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

        // Use this for initialization
        override protected void Start()
        {
            base.Start();

            if (s_firstTime)
            {
                s_firstTime = false;

                if (m_initialProjectSceneName.Length > 0)
                {
                    OpenProject(m_initialProjectSceneName);
                }
            }
        }

        // Update is called once per frame
        override protected void Update()
        {
            base.Update();
        }
    }
}
