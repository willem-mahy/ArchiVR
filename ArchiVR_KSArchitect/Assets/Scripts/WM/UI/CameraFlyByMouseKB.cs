using UnityEngine;

public class CameraFlyByMouseKB : CameraFlyBase
{
    public Vector3 m_lastMousePosition = Vector3.zero;
    public bool m_lastMousePositionSet = false;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        var offsetXZ = Vector2.zero;

        float speed = 20.0f;

        if (Input.GetKey("right shift"))
        {
            speed *= 2;
        }

        var offset = speed * Time.deltaTime;

        if (Input.GetKey("up"))
        {
            offsetXZ += offset * Vector2.up;
        }

        if (Input.GetKey("down"))
        {
            offsetXZ += offset * Vector2.down;
        }

        if (Input.GetKey("left"))
        {
            offsetXZ += offset * Vector2.left;
        }

        if (Input.GetKey("right"))
        {
            offsetXZ += offset * Vector2.right;
        }

        TranslateXZ(offsetXZ);

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
