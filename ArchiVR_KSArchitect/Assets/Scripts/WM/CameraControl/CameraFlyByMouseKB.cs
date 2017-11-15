using UnityEngine;

public class CameraFlyByMouseKB : CameraFlyBase
{
    public Vector3 m_lastMousePosition = Vector3.zero;
    public bool m_lastMousePositionSet = false;

    //! Whether to constrain Forward/Backward/Left_Right translation movement to the World XZ plane.
    public bool m_fixY = false;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    public void Update()
    {
        //DebugUtil.LogKeyPress("space");
        //DebugUtil.LogKeyPress("left");
        //DebugUtil.LogKeyPress("right");
        //DebugUtil.LogKeyPress("up");
        //DebugUtil.LogKeyPress("down");

        float speed = (Input.GetKey("right shift") ? m_translateSpeedNormal : m_translateSpeedFast);

        float offset = speed * Time.deltaTime;

        var translateDirXZ = Vector2.zero;

        if (Input.GetKey("up")) // Forward
        {
            translateDirXZ += Vector2.up;
        }

        if (Input.GetKey("down")) // Backward
        {
            translateDirXZ += Vector2.down;
        }

        if (Input.GetKey("left")) // Left
        {
            translateDirXZ += Vector2.left;
        }

        if (Input.GetKey("right")) // Right
        {
            translateDirXZ += Vector2.right;
        }

        TranslateXZ(translateDirXZ, offset, m_fixY);

        if (Input.GetKey("u"))
        {
            TranslateY(offset);
        }

        if (Input.GetKey("d"))
        {
            TranslateY(-offset);
        }

        if (Input.mousePresent)
        {
            if (!m_lastMousePositionSet)
            {
                m_lastMousePosition = Input.mousePosition;
                m_lastMousePositionSet = true;
            }

            Vector3 delta = Input.mousePosition - m_lastMousePosition;

            Vector3 rotation = 3.0f * delta;

            Rotate(rotation);

            m_lastMousePosition = Input.mousePosition;
        }
    }
}
