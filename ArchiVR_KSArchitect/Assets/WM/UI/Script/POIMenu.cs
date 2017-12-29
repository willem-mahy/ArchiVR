
using Assets.Scripts.WM;
using Assets.Scripts.WM.CameraNavigation;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.WM.UI.Script
{
    public class POIMenu : MonoBehaviour
    {
        // Button to activate the previous POI.
        public Button m_activatePreviousPOIButton = null;

        // Button to activate the next POI.
        public Button m_activateNextPOIButton = null;

        // Text that displays the name of the active POI.
        public Text m_activePOINameText = null;

        void Start()
        {
            if (m_activatePreviousPOIButton)
            {
                var buttonComponent = m_activatePreviousPOIButton.GetComponent<Button>();
                buttonComponent.onClick.AddListener(PrevButton_OnClick);
            }

            if (m_activateNextPOIButton)
            {
                var buttonComponent = m_activateNextPOIButton.GetComponent<Button>();
                buttonComponent.onClick.AddListener(NextButton_OnClick);
            }
        }

        private void Update()
        {
            if (m_activePOINameText)
            {
                var activePOI = POIManager.GetInstance().GetActivePOI();
                var activePOIName = (activePOI ? activePOI.name : "No POI active.");

                m_activePOINameText.text = activePOIName;
            }
        }

        void PrevButton_OnClick()
        {
            Debug.Log("PrevButton_OnClick()");

            POIManager.GetInstance().ActivatePrevPOI();
        }

        void NextButton_OnClick()
        {
            Debug.Log("NextButton_OnClick()");

            POIManager.GetInstance().ActivateNextPOI();
        }        
    }
 }