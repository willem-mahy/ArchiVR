using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.WM.UI
{
    // Represents a virtual (UI) D-pad stick.
    // Supports unidirectional(X or Y) and bidirectional (X and Y) control.
    public class DPadBehavior : MonoBehaviour
    {
        public enum Axes
        {
            X = 1,
            Y = 2,
            XY = 3
        }

        // The axis or axes along which the stick can be moved.
        public Axes m_axes = Axes.XY;

        public float zeroPositionX = 0.0f;
        public float zeroPositionY = 0.0f;

        public bool m_autoReturnToZero;

        // Flag indicating whether the stick is currently being manipulated.
        private bool m_isBeingManipulated = false;

        // The UI Button representing a D-pad stick.  Must be defined.
        public Button m_stick = null;

        // The area to which to constrain the D-Pad strick.  Optional.  Stick is unconstrained if not defined.
        public GameObject m_stickArea = null;

        // The base position for the stick in rest mode.
        private Vector3 m_stickBasePosition = new Vector3();

        // String containing the current status for this D-Pad stick.
        private String m_status;

        // Use this for initialization
        void Start()
        {
            m_stickBasePosition.Set(m_stick.transform.position.x, m_stick.transform.position.y, m_stick.transform.position.z);
        }

        // Update is called once per frame
        void Update()
        {
        }

        //! Query whether the stick is currently being manipulated.
        public bool IsBeingManipulated()
        {
            return m_isBeingManipulated;
        }

        //! Query whether the stick is setup to support maipulation along the X axis.
        public bool IsAxisXEnabled()
        {
            return (m_axes & Axes.X) == Axes.X;
        }

        //! Query whether the stick is setup to support maipulation along the Y axis.
        public bool IsAxisYEnabled()
        {
            return (m_axes & Axes.Y) == Axes.Y;
        }

        //! Get the status text.
        public String GetStatus()
        {
            return m_status;
        }

        //! Called upon 'PointerUp' event on Stick button.
        //  Ends the current stick manipulation.
        public void OnStickPointerUp(BaseEventData baseEventData)
        {
            Debug.Log("DPadBehavior.OnStickPointerUp()");

            var pointerEventData = (PointerEventData)baseEventData;

            if (pointerEventData == null)
            {
                return; // Sanity
            }

            m_isBeingManipulated = false;

            m_status = "OnPointerUp:" + pointerEventData.position.ToString();

            if (m_autoReturnToZero)
            {
                Debug.Log("Returning stick to zero position.");
                m_stick.transform.position.Set(m_stickBasePosition.x, m_stickBasePosition.y, m_stickBasePosition.z);
            }
        }

        //! Called upon 'PointerDown' event on Stick button.
        //  Starts a stick manipulation.
        public void OnStickPointerDown(BaseEventData baseEventData)
        {
            Debug.Log("DPadBehavior.OnStickPointerDown()");

            var pointerEventData = (PointerEventData)baseEventData;

            if (pointerEventData == null)
            {
                return; // Sanity
            }

            m_status = "OnPointerDown:" + pointerEventData.position.ToString();

            m_isBeingManipulated = true;

            m_stick.transform.position = ToVector3(pointerEventData.position);
        }

        //! Called upon 'Drag' event on Stick button.
        //  Moves the stick along with the touch movement.
        public void OnStickDrag(BaseEventData baseEventData)
        {
            Debug.Log("DPadBehavior.OnStickDrag()");

            var pointerEventData = (PointerEventData)baseEventData;

            if (pointerEventData == null)
            {
                return; // Sanity
            }

            m_status = "Stick " + gameObject.name + " OnDrag:" + pointerEventData.position.ToString();

            var newStickPosition = ToVector3(pointerEventData.position);

            if (!IsAxisXEnabled())
            {
                newStickPosition.x = m_stickBasePosition.x;
            }

            if (!IsAxisYEnabled())
            {
                newStickPosition.y = m_stickBasePosition.y;
            }

            if (m_stickArea != null)
            {
                var stickAreaTransform = (RectTransform)m_stickArea.transform;
                newStickPosition.x = Math.Min(stickAreaTransform.position.x + stickAreaTransform.rect.xMax, newStickPosition.x);
                newStickPosition.y = Math.Min(stickAreaTransform.position.y + stickAreaTransform.rect.yMax, newStickPosition.y);

                newStickPosition.x = Math.Max(stickAreaTransform.position.x + stickAreaTransform.rect.xMin, newStickPosition.x);
                newStickPosition.y = Math.Max(stickAreaTransform.position.y + stickAreaTransform.rect.yMin, newStickPosition.y);
            }

            m_stick.transform.position = newStickPosition;
        }

        //! Get the offset from the DPad stick rest position, to the current stick posistion.
        public Vector2 GetStickOffset()
        {
            return ToVector2(m_stick.transform.position - m_stickBasePosition);
        }

        //! Makes a Vector3 from the given Vector2 (z is set to 0)
        public Vector3 ToVector3(Vector2 v)
        {
            return new Vector3(v.x, v.y);
        }

        //! Makes a Vector2 from the given Vector2 (z is omitted)
        public Vector2 ToVector2(Vector3 v)
        {
            return new Vector2(v.x, v.y);
        }
    }
}