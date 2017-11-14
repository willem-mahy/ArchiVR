using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.WM.ArchiVR.Application
{
    public class ApplicationStateHome : ApplicationState
    {
        // Use this for initialization
        override protected void Start()
        {
            Debug.Log("ApplicationStateHome.Start()");
            base.Start();

            ApplicationSettings.GetInstance().Load();                     
        }

        // Update is called once per frame
        override protected void Update()
        {
            base.Update();

            // If user presses 'Esc', quit the application.
            if (Input.GetKey("escape"))
            {
                QuitApplication();
            }
        }        
    }
}
