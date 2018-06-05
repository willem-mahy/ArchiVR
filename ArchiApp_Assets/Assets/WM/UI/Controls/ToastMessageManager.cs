using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.WM.UI
{
    class ToastMessageManager : MonoBehaviour
    {
        public static ToastMessageManager s_instance = null;

        public static ToastMessageManager GetInstance()
        {
            return s_instance;
        }

        // Prefab Gameobject (UI Text) for a toast message.
        public GameObject m_toastMessagePrefab = null;

        // The UI container (Panel) that contains all toast messages.
        public GameObject m_toastPanel = null;

        // List of existing Toast messages.
        private List<ToastMessage> m_messages = new List<ToastMessage>();

        public void Start()
        {
            s_instance = this;
        }

        public void OnDestroy()
        {
            if (s_instance == this)
            {
                s_instance = null;
            }
        }

        public void OnEnable()
        {
        }

        public void OnDisable()
        {
        }

        public void Update()
        {
            // Update toast messages.  Some of them might die here.
            foreach (var message  in m_messages)
            {
                message.Update();
            }

            // Kill all but last X messages.
            while (m_messages.Count > 5)
            {
                m_messages[0].m_speed *= 2;
                m_messages[0].die();
            }

            // Remove the dead (recently deceased) toasts.
            // Because we release the toasts as last, we transition them into no-existence here.
            for (int i = 0;  i < m_messages.Count; ++i)
            {
                var message = m_messages[i];

                if (message && message.m_state.ToastShouldBeRemoved() == true)
                {
                    m_messages.RemoveAt(i);
                    --i;
                }
            }            

            // Relayout the toasts that are still alive.
            float posY = 0 ;
            foreach (var message in m_messages)
            {
                var text = message.GetComponent<Text>();

                if (null != text)
                {
                    if (false)
                    {
                        // Reposition current toast to its correct position.
                        var pos = text.rectTransform.localPosition;
                        pos.y = posY;
                        //text.rectTransform.localPosition = pos;
                    }
                    else
                    {
                        //// Set the rect transform top offset for the toast message UI control.
                        var rectTransform = message.GetComponent<RectTransform>();

                        if (null != rectTransform)
                        {
                            // rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, rectTransform.offsetMax.y - yOffset);
                            var rpos = rectTransform.localPosition;
                            rpos.y = posY;
                            rectTransform.localPosition = rpos;

                            // WM: TODO1: make scale or  height of toast decreae when fading out...
                            var s = rectTransform.localScale;
                            s.y = 
                                text.color.a / 255.0f;
                        }
                    }

                    // Then adjust position for next toast.
                    posY += 100;// text.rectTransform.offsetMax.y;

                    
                }
            }
        }

        public void AddMessage(ToastMessage toastMessage)
        {
            m_messages.Add(toastMessage);
        }

        public void RetireAllToasts()
        {
            foreach (var message in m_messages)
            {
                message.m_timeSpawned = message.m_lifeTime;
            }
        }

        public void AddToast(String text)
        {
            var toast = DynamicallyAddToast(text);

            var m = toast.GetComponent<ToastMessage>();

            if (null != m)
                m_messages.Add(m);
        }

        private GameObject DynamicallyAddToast(
            String text)
        {
            if (gameObject.activeInHierarchy == false)
                return null;

            var newToast = (GameObject)Instantiate(m_toastMessagePrefab);

            if (null == newToast)
            {
                Debug.LogError("newToast = null");
                return null;
            }

            newToast.SetActive(true);
            newToast.name = "Toast_" + text;

            newToast.GetComponent<Text>().text = text;

            // Add toat UI Control to its parent UI control.
            newToast.transform.SetParent(m_toastPanel.transform, false);
            
           // var toastMessage = newToast.GetComponent<ToastMessage>();

            return newToast;
        }
    }
}
