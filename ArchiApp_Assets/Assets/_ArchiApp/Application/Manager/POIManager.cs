using Assets.Scripts.WM.Settings;
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
        
        //! The POI Collection GameObject.
        //  The POI Collection GameObject defines the list of currently available POI's.
        //  POI's are inactive Camera objects and have to be direct childs of this root GameObject.containing the  
        private GameObject m_poiCollection = null;

        //! The index of the currently active POI in the POI collection.
        public int m_activePOIIndex = -1;

        void Awake()
        {
            s_instance = this;
        }

        void Start()
        {
        }

        public void SetPOICollection(GameObject collection)
        {
            m_poiCollection = collection;

            // If there are POI's in the collection, activate the first POI.
            SetActivePOI((GetNumPOIs() > 0) ? 0 : -1);
        }

        private void SetActivePOI(int index)
        {
            m_activePOIIndex = index;
            SyncWithActivePOI();
        }

        public void ActivatePrevPOI()
        {
            Debug.Log("ActivatePrevPOI()");

            var numPOIs = GetNumPOIs();

            if (numPOIs == 0)
            {
                SetActivePOI(-1);
            }
            else
            {
                SetActivePOI((m_activePOIIndex == 0) ? 
                    numPOIs - 1 : 
                    m_activePOIIndex - 1);
            }
        }
        
        public void ActivateNextPOI()
        {
            Debug.Log("ActivateNextPOI()");

            var numPOIs = GetNumPOIs();

            SetActivePOI(numPOIs > 0 ? (++m_activePOIIndex % numPOIs) : -1);
        }

        public int GetNumPOIs()
        {
            if (m_poiCollection == null)
            {
                return 0;
            }

            return m_poiCollection.transform.childCount;
        }

        //! Returns active POI.  Returns null if no POI active.
        public GameObject GetActivePOI()
        {
            if (null == m_poiCollection)
            {
                return null;
            }

            if (m_activePOIIndex < 0)
            {
                return null;
            }

            if (m_activePOIIndex >= m_poiCollection.transform.childCount)
            {
                return null;
            }

            return m_poiCollection.transform.GetChild(m_activePOIIndex).gameObject;
        }

        void SyncWithActivePOI()
        {
            Debug.Log("SyncWithActivePOI()");

            var activePOI = GetActivePOI();

            var tmm = WM.UI.ToastMessageManager.GetInstance();

            if (tmm)
            {
                tmm.RetireAllToasts();
                tmm.AddToast(activePOI ? activePOI.name : "");
            }

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

        public void WriteCurrentCameraLocationToFilePOI()
        {
            var camera = Camera.main;
            var name = GetActivePOI().name;
            var position = camera.transform.position.ToString();
            var rotation = camera.transform.rotation.eulerAngles.ToString();

            var text =
                System.Environment.NewLine +
                "POI" +
                " Name: " + name +
                " Pos:" + position +
                " Rot:" + rotation;

            var appSettings = ApplicationSettings.GetInstance();

            var projectName = appSettings.m_data.m_stateSettings.m_activeProjectName;

            var filePath = UnityEngine.Application.persistentDataPath + "\\poi_" + projectName + ".txt";
            System.IO.File.AppendAllText(filePath, text);
        }
    }
 }