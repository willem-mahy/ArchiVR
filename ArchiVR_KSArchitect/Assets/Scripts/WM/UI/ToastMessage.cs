using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.WM.UI
{
    abstract class ToastMessageState
    {
        abstract public void Start(ToastMessage t);

        abstract public void Update(ToastMessage t);

        abstract public bool ToastShouldBeRemoved();
    }

    class ToastMessageStateFadeIn : ToastMessageState
    {
        override public void Start(ToastMessage t)
        {
            var c = t.m_text.GetComponent<Text>().color;
            c.a = 0;

            t.m_text.GetComponent<Text>().color = c;
        }

        override public void Update(ToastMessage t)
        {
            var c = t.m_text.GetComponent<Text>().color;
            c.a += t.m_speed;

            t.m_text.GetComponent<Text>().color = c;

            if (c.a >= 1.0f)
            {
                t.FullyShown();
            }
        }

        override public bool ToastShouldBeRemoved()
        {
            return false;
        }
    }

    class ToastMessageStateShown : ToastMessageState
    {
        override public void Start(ToastMessage t)
        {
            t.m_timeSpawned = 0;
        }

        override public void Update(ToastMessage t)
        {
            t.m_timeSpawned += Time.deltaTime;

            if (t.m_timeSpawned >= t.m_lifeTime)
            {
                t.FadeOut();
            }
        }

        override public bool ToastShouldBeRemoved()
        {
            return false;
        }
    }

    class ToastMessageStateFadeOut : ToastMessageState
    {
        override public void Start(ToastMessage t)
        {

        }

        override public void Update(ToastMessage t)
        {
            //
            var c = t.m_text.GetComponent<Text>().color;
            c.a-= t.m_speed;

            t.m_text.GetComponent<Text>().color = c;

            if (c.a <= 0)
            {
                // Fadeout complete.  Remove message from scene.
                t.die();
            }
        }

        override public bool ToastShouldBeRemoved()
        {
            return false;
        }
    }

    class ToastMessageStateDead : ToastMessageState
    {
        override public void Start(ToastMessage t)
        {

        }

        override public void Update(ToastMessage t)
        {
        }

        override public bool ToastShouldBeRemoved()
        {
            return true;
        }
    }



    class ToastMessage : MonoBehaviour
    {
        public GameObject m_text;

        // The lifetime of the toast message, in secs.
        public float m_lifeTime = 2;

        public float m_timeSpawned = 0;

        // FadeIn/FadeOut speed.
        public float m_speed = 0.1f;

        public ToastMessageState m_state = null;

        public void Start()
        {
            FadeIn();
        }

        public void Update()
        {
            if (null != m_state)
            {
                m_state.Update(this);
            }
        }

        public void FadeIn()
        {
            m_state = new ToastMessageStateFadeIn();

            m_state.Start(this);
        }

        public void FadeOut()
        {
            m_state = new ToastMessageStateFadeOut();

            m_state.Start(this);
        }

        public void die()
        {
            m_text.transform.parent = null;

            m_state = new ToastMessageStateDead();

            m_state.Start(this);
        }
        
        public void FullyShown()
        {
            m_state = new ToastMessageStateShown();

            m_state.Start(this);
        }
    }
}
