using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.WM.UI.VR {
    public class VRButton : MonoBehaviour {

        public VRStandardAssets.Utils.VRInteractiveItem m_InteractiveItem = null;

        // Use this for initialization
        void Start () {
            m_InteractiveItem.OnOver += HandleOver;
            m_InteractiveItem.OnOut += HandleOut;
            m_InteractiveItem.OnClick += HandleClick;
            m_InteractiveItem.OnDoubleClick += HandleDoubleClick;
        }
	
	    // Update is called once per frame
	    void Update () {
		
	    }

        private void Awake()
        {        
        }


        private void OnEnable()
        {        
        }

        private void OnDisable()
        {
            m_InteractiveItem.OnOver -= HandleOver;
            m_InteractiveItem.OnOut -= HandleOut;
            m_InteractiveItem.OnClick -= HandleClick;
            m_InteractiveItem.OnDoubleClick -= HandleDoubleClick;
        }


        //Handle the Over event
        private void HandleOver()
        {
            Debug.Log("HandleOver");
        }


        //Handle the Out event
        private void HandleOut()
        {
            Debug.Log("HandleOut");
        }


        //Handle the Click event
        private void HandleClick()
        {
            Debug.Log("HandleClick");
        }


        //Handle the DoubleClick event
        private void HandleDoubleClick()
        {
            Debug.Log("HandleDoubleClick");
        }
    }
}
