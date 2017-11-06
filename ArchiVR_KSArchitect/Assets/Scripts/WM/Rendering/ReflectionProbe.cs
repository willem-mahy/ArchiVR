using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.WM
{
    public class ReflectionProbe : MonoBehaviour
    {

        enum Direction
        {
            X, Y, Z
        }

        public GameObject m_mirror = null;
        public GameObject m_camera = null;


        // Use this for initialization
        void Start()
        {
            if (m_camera == null)
            {
                m_camera = GameObject.Find("Main Camera");
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (m_camera == null)
            {
                m_camera = GameObject.Find("Main Camera");
            }

            if (m_camera == null)
            {
                return;
            }

            var offset = m_mirror.transform.position - m_camera.transform.position;

            var d = Direction.Z;

            switch (d)
            {
                case Direction.X:
                    transform.position = new Vector3(
                        m_mirror.transform.position.x + offset.x,
                        m_camera.transform.position.y,
                        m_camera.transform.position.z);
                    break;

                case Direction.Y:
                    transform.position = new Vector3(
                    m_camera.transform.position.x,
                    m_mirror.transform.position.y + offset.y,
                    m_camera.transform.position.z);
                    break;

                case Direction.Z:
                    transform.position = new Vector3(
                    m_camera.transform.position.x,
                    m_camera.transform.position.y,
                    m_mirror.transform.position.z + offset.z);
                    break;
            }
        }
    }
}