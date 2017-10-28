using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraRotateBySwipe : CameraRotate
{
    private bool m_swiping = false;
    private Vector2 m_swipeStart = new Vector2(0,0);
    private Vector2 m_lastTouchPosition = new Vector2(0,0);

    // Use this for initialization
    new public void Start () {
        base.Start();
        Debug.Log("CameraRotateBySwipe.Start()");
    }

    /// <summary>
    /// Cast a ray to test if Input.mousePosition is over any UI object in EventSystem.current. This is a replacement
    /// for IsPointerOverGameObject() which does not work on Android in 4.6.0f3
    /// </summary>
    private bool IsPointerOverUIObject(Touch t)
    {
        Debug.Log("CameraRotateBySwipe.IsPointerOverUIObject()");

        // Referencing this code for GraphicRaycaster https://gist.github.com/stramit/ead7ca1f432f3c0f181f
        // the ray cast appears to require only eventData.position.
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(t.position.x, t.position.y);

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        if (results.Count > 0)
        {
            Debug.Log("Over UI Object '" + results[0].gameObject.name + "'.");
        }
        else
        {
            Debug.Log("Not over UI Object.");
        }

        return results.Count > 0;
    }

    private int GetIndexOfTouchClosestToLastTouch()
    {
        Debug.Log("CameraRotateBySwipe.GetIndexOfTouchClosestToLastTouch()");

        int touchIndex = -1;

        if (Input.touchCount > 0)
        {
            float minDist = float.MaxValue;

            for (int i = 0; i < Input.touchCount; ++i)
            {
                Vector2 offset = (m_lastTouchPosition - Input.touches[i].position);
                float dist = offset.sqrMagnitude;

                if (dist < minDist)
                {
                    touchIndex = i;
                    minDist = dist;
                }
            }
        }

        Debug.Log("Index of closest touch = " + touchIndex);

        return touchIndex;
    }

    private Quaternion m_initialCameraRotation = new Quaternion();

    protected override void UpdateCameraRotation()
    {
        //Debug.Log("WMCameraRotateBySwipe.UpdateCameraRotation()");

        //Debug.Log("#touches:" + Input.touchCount);

        switch (Input.touchCount)
        {
            case 0:
                m_swiping = false;
                break;
            default:
                {
                    var closestTouchIndex = GetIndexOfTouchClosestToLastTouch();                    

                    if (closestTouchIndex == -1)
                    {
                        Debug.Log("invalid closestTouchIndex:" + closestTouchIndex);
                        m_swiping = false;
                        return;
                    }

                    var closestTouch = Input.touches[closestTouchIndex];

                    m_lastTouchPosition = closestTouch.position;

                    if (m_swiping)
                    {
                        Debug.Log("Swiping...");
                        switch (closestTouch.phase)
                        {
                            case TouchPhase.Moved:
                                {
                                    Debug.Log("Continue Swipe");
                                    Vector2 offset = closestTouch.position - m_swipeStart;
                                    Vector2 euler = 0.1f * offset;
                                    Quaternion rotX = Quaternion.Euler(new Vector3(euler.y, 0, 0));
                                    Quaternion rotY = Quaternion.Euler(new Vector3(0, -euler.x, 0));

                                    m_camera.transform.rotation = m_initialCameraRotation;

                                    Vector3 axis = new Vector3();
                                    float angle = 0;

                                    {                                        
                                        rotX.ToAngleAxis(out angle, out axis);
                                        m_camera.transform.rotation = m_initialCameraRotation;
                                        m_camera.transform.Rotate(axis, angle, Space.Self);
                                    }

                                    {
                                        rotY.ToAngleAxis(out angle, out axis);
                                        
                                        m_camera.transform.Rotate(axis, angle, Space.World);
                                    }
                                }
                                break;
                            case TouchPhase.Ended:
                                {
                                    Debug.Log("End Swipe");
                                    m_swiping = false;
                                }
                                break;
                        }
                    }
                    else
                    {
                        Debug.Log("Not swiping...");

                        Debug.Log("Touch phase:" + closestTouch.phase);
                        switch (closestTouch.phase)
                        {
                            case TouchPhase.Began:                                
                                m_swiping = !IsPointerOverUIObject(closestTouch);
                                Debug.Log(m_swiping ? "Start Swipe" : "UI Press");
                                m_swipeStart = closestTouch.position;
                                m_initialCameraRotation = m_camera.transform.rotation;
                                break;
                        }
                    }
                    break;
                }
        }
    }
}
