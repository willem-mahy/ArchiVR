using UnityEngine;

namespace Assets.Scripts.WM
{
    public class POIManager : MonoBehaviour
    {
        static private POIManager s_instance = null;

        static public POIManager GetInstance()
        {
            return s_instance;
        }

        public CameraNavigation.CameraNavigation m_cameraNavigation = null;
                
        public int m_activePOIIndex = -1;

        //! The POI list GameObject.
        //  The POI list GameObject defines the list of currently available POI's.
        //  POI's are inactive Camera objects and have to be direct childs of this root GameObject.containing the  
        private GameObject m_poi = null;
        
        void Awake()
        {
            s_instance = this;

            if (m_poi == null)
            {
                m_poi = GameObject.Find("POI.Default");
            }
        }

        void Start()
        {
            // Activate first POI
            m_activePOIIndex = 0;

            SyncWithActivePOI();
        }

        public void ActivatePrevPOI()
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
        
        public void ActivateNextPOI()
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

            // Update (via camera navigation mode) camera location to newly activated POI location.
            var nm = m_cameraNavigation.GetActiveNavigationMode();

            if (null != nm)
            {
                var position = Vector3.zero;
                var rotation = Quaternion.identity;

                if (null != activePOI)
                {
                    position = activePOI.transform.position;
                    rotation = activePOI.transform.rotation;
                }

                nm.PositionCamera(position, rotation);
            }
        }
    }
 }