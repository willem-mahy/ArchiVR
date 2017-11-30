using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Assets.Scripts.WM.Settings;
using Assets.Scripts.WM.UI;
using Assets.Scripts.WM.Util;

namespace Assets.Scripts.WM.ArchiVR.Menu
{
    public class MenuLayer : MonoBehaviour
    {
        //! The button to close this menu.
        public Button m_exitButton = null;

        // Use this for initialization
        void Start()
        {
            m_exitButton.onClick.AddListener(ExitButton_OnClick);
        }

        // Update is called once per frame
        void Update()
        {
        }

        void ExitButton_OnClick()
        {
            Debug.Log("MenuLayer.ExitButton_OnClick()");
            UIManager.GetInstance().CloseMenu();
        }
    }
}