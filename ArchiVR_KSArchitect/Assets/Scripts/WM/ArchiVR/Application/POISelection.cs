using System.Collections;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Assets.Scripts.WM.CameraNavigation.RotationControl;

namespace Assets.Scripts.WM
{
    public class POISelection : MonoBehaviour
    {
        public CameraNavigation.CameraNavigation m_cameraNavigation = null;

        public List<Button> m_PrevButtonArray = new List<Button>();
        public List<Button> m_NextButtonArray = new List<Button>();
        public List<Text> m_POINameTextArray = new List<Text>();

        public int m_activePOIIndex = -1;

        public GameObject m_poi = null;

        void Start()
        {
            foreach (var button in m_PrevButtonArray)
            {
                var buttonComponent = button.GetComponent<Button>();
                buttonComponent.onClick.AddListener(PrevButton_OnClick);
            }

            foreach (var button in m_NextButtonArray)
            {
                var buttonComponent = button.GetComponent<Button>();
                buttonComponent.onClick.AddListener(NextButton_OnClick);
            }

            if (m_poi == null)
            {
                m_poi = GameObject.Find("POI.Default");
            }

            // Activate first POI
            m_activePOIIndex = 0;

            SyncWithActivePOI();
        }

        void PrevButton_OnClick()
        {
            Debug.Log("PrevButton_OnClick()");

            ActivatePrevPOI();
        }

        void ActivatePrevPOI()
        {
            Debug.Log("ActivatePrevPOI()");

            if (m_poi == null)
                m_poi = GameObject.Find("POI.Default");

            if (m_poi == null || m_poi.transform.childCount == 0)
            {
                m_activePOIIndex = -1;
            }
            else
            {
                --m_activePOIIndex;

                // Cycle from end.
                if (m_activePOIIndex < 0)
                    m_activePOIIndex = m_poi.transform.childCount - 1;
            }

            SyncWithActivePOI();
        }

        void NextButton_OnClick()
        {
            Debug.Log("NextButton_OnClick()");

            ActivateNextPOI();
        }

        void ActivateNextPOI()
        {
            Debug.Log("ActivateNextPOI()");

            if (m_poi == null)
            {
                m_poi = GameObject.Find("POI.Default");
            }

            if (m_poi == null || m_poi.transform.childCount == 0)
            {
                m_activePOIIndex = -1;
            }
            else
            {
                m_activePOIIndex = (++m_activePOIIndex % m_poi.transform.childCount);
            }

            SyncWithActivePOI();
        }

        public GameObject GetActivePOI()
        {
            if (null == m_poi)
            {
                return null;
            }

            if (m_activePOIIndex < 0)
            {
                return null;
            }

            if (m_activePOIIndex >= m_poi.transform.childCount)
            {
                return null;
            }

            return m_poi.transform.GetChild(m_activePOIIndex).gameObject;
        }

        void SyncWithActivePOI()
        {
            Debug.Log("SyncWithActivePOI()");

            var activePOI = GetActivePOI();

            // Update camera location to active POI location.
            var nm = m_cameraNavigation.GetActiveNavigationMode();

            if (null == nm)
                return;
            
            var position = Vector3.zero;
            var rotation = Quaternion.identity;

            if (null != activePOI)
            {
                position = activePOI.transform.position;
                rotation = activePOI.transform.rotation;
            }

            nm.PositionCamera(position, rotation);

            // Update active POI name in UI.
            var activePOIName = activePOI ? activePOI.name : "No POI selected";
                        
            foreach (var text in m_POINameTextArray)
            {
                text.text = activePOIName;
            }
        }
    }
 }