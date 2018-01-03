using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.WM.UI.VR
{
    public class TimedGazeItem : MonoBehaviour
    {
        public Button m_button = null;

        private bool m_isGazing = false;
        private float m_timeGazed = 0;
        public float m_gazeTime = 1.5f;

        // Use this for initialization
        void Start()
        {
            if (null == m_button)
            {
                //throw new Exception();            
            }

            var vii = m_button.GetComponent<VRStandardAssets.Utils.VRInteractiveItem>();

            if (null == vii)
            {
                //throw new Exception();
            }

            m_isGazing = false;

            vii.OnOver += OnOver;
            vii.OnOut += OnOut;
        }

        // Update is called once per frame
        void Update()
        {
            if (m_isGazing)
            {
                m_timeGazed += Time.deltaTime;

                if (m_timeGazed >= m_gazeTime)
                {
                    m_timeGazed %= m_gazeTime;
                    m_button.onClick.Invoke();

                    ExecuteEvents.Execute<IPointerClickHandler>(m_button.GetComponent<GameObject>(), new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
                }
            }
        }

        //Handle the Over event
        private void OnOver()
        {
            if (!m_button.IsInteractable())
            {
                return;
            }

            m_isGazing = true;
        }

        //Handle the Out event
        private void OnOut()
        {
            m_timeGazed = 0;
            m_isGazing = false;
        }
    }
}
