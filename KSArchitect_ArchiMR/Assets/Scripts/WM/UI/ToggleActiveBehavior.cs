//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.EventSystems;

//// Toggles active state of a list of target GameObjects, upon following triggers:
//// - clicking a designated button
//// - tapping on the screen where there are no UI controls.
//namespace Assets.Scripts.WM
//{
//    public class ToggleActiveBehavior : MonoBehaviour
//    {
        

//        public bool m_active = true;
//        public List<GameObject> m_targetGameObjectArray = new List<GameObject>();

//        // Use this for initialization
//        void Start()
//        {
//        }

        

//        // Update is called once per frame
//        void Update()
//        {
//            HandleTouches();            
//        }        

//        public void TogglActiveState()
//        {
//            SetActiveState(!m_active);
//        }

//        public void SetActiveState(bool active)
//        {
//            Debug.Log("ToggleActiveBehavior.SetActive(" + active + ")");

//            m_active = active;

//            foreach (var target in m_targetGameObjectArray)
//            {
//                target.SetActive(m_active);
//            }
//        }
//    }
//}