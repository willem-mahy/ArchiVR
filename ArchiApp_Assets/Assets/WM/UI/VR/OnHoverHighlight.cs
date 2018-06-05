using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.WM.UI.VR
{
    public class OnHoverHighlight : MonoBehaviour
    {
        public float m_highlightScaleFactor = 1.2f;

        // Use this for initialization
        void Start()
        {
            var ii = gameObject.GetComponent<VRStandardAssets.Utils.VRInteractiveItem>();

            if (ii)
            {
                ii.OnOver += HandleOver;
                ii.OnOut += HandleOut;
            }
        }

        // Update is called once per frame
        void Update()
        {
        }

        //Handle the Over event
        private void HandleOver()
        {
            Debug.Log("HandleOver");
            gameObject.transform.localScale = m_highlightScaleFactor * Vector3.one;
            gameObject.GetComponentInChildren<Button>().Select();
        }

        //Handle the Out event
        private void HandleOut()
        {
            Debug.Log("HandleOut");
            gameObject.transform.localScale = Vector3.one;
            //gameObject.GetComponentInChildren<Button>().Deselect();
        }
    }
}