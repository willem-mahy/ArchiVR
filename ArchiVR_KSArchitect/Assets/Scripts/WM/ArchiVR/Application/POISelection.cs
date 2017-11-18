using System.Collections;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Assets.Scripts.WM.CameraControl.CameraNavigation.RotationControl;

namespace Assets.Scripts.WM
{
    /* A 'Point-Of-Interest' description.
     * Projects can be viewed by jumping from POI to POI, using the POI controls.
     */
    public class POI
    {
        public Vector3 m_position;
        public string m_name;
    }

    public class POISelection : MonoBehaviour
    {
        public Camera m_camera = null;

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

        Transform GetActivePOI()
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

            return m_poi.transform.GetChild(m_activePOIIndex);
        }

        void SyncWithActivePOI()
        {
            Debug.Log("SyncWithActivePOI()");

            var activePOI = GetActivePOI();

            // Update camera location to active POI location.
            m_camera.transform.position = (activePOI ? activePOI.transform.position : Vector3.zero);

            if (null == activePOI)
            {
                m_camera.transform.rotation = Quaternion.identity;
            }
            else
            {
                if (Application.isEditor)
                {
                    m_camera.transform.rotation = activePOI.transform.rotation;
                }
                else
                {
                    Quaternion cameraRotationFromGyro = RotationCOntrolGyro.GetRotationFromGyro();

                    GameObject temp = new GameObject();
                    temp.transform.Rotate(cameraRotationFromGyro.eulerAngles);
                    Vector3 forwardCameraFromGyro = temp.transform.forward;
                    forwardCameraFromGyro.y = 0;

                    Vector3 forwardPOI = activePOI.transform.forward;
                    forwardPOI.y = 0;

                    if ((forwardCameraFromGyro.sqrMagnitude != 0) &&
                            (forwardPOI.sqrMagnitude != 0))
                    {
                        forwardCameraFromGyro.Normalize();
                        forwardPOI.Normalize();

                        //m_POINameText.text = "fCam:" + forwardCameraFromGyro.ToString() + " fPOI:" + forwardPOI.ToString();

                        //TODO:
                        /*
                        CameraRotateByGyro.m_offsetRotY = (180.0f / Mathf.PI) * Mathf.Acos(Vector3.Dot(forwardPOI, forwardCameraFromGyro));

                        if (Vector3.Cross(forwardPOI, forwardCameraFromGyro).y > 0)
                        {
                            CameraRotateByGyro.m_offsetRotY = -CameraRotateByGyro.m_offsetRotY;
                        }
                        */
                    }
                }
            }

            // Update active POI name in UI.
            var activePOIName = activePOI ? activePOI.name : "No POI selected";
                        
            foreach (var text in m_POINameTextArray)
            {
                text.text = activePOIName;
            }
        }
    }
 }