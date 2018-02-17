using Assets.Scripts.WM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSceneSelectionVR : MonoBehaviour {

    public List<String> m_projectNames;

    public GameObject m_projectButtonPrefab = null;

    public List<GameObject> m_projectButtons;

    public ProjectManager m_projectManager = null;

    // The spacing, in world space, between project buttons of successive rows.
    public float m_rowSpacingY = 0.2f;

    // Get the height of a layer option UI control.
    public float m_projectButtonHeight_World = 1.0f;// m_projectButtonPrefab.GetComponent<RectTransform>().rect.height;

    // The number of project buttons in one row of the selection menu.
    public int m_numProjectButtonsPerRow = 6;

    //! The total angle, around the Y axis, that the menu and its components cover.
    public float m_totalAngleY = 180;

    // Use this for initialization
    void Start () {
        DynamicallyAddProjectButtons();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void DynamicallyAddProjectButtons()
    {        
        var projects = m_projectManager.GetProjects();

        // Y spacing between successive layer option UI controls.
        
        // Y step between successive project Button controls.
        float yStep = m_projectButtonHeight_World + m_rowSpacingY;

        // Spacing on top
        float y = 0;
        float yAngle = 0;

        // Start with a spacing above the first layer option (=top-level option in the list).
        y = m_rowSpacingY;
        
        float angleStep = m_totalAngleY / (m_numProjectButtonsPerRow - 1);

        int numRows = (int)Math.Floor((double)projects.Count / m_numProjectButtonsPerRow) + 1;

        // From top to bottom, 
        // generate a list option for all layers.
        int projectIndex = 0;
        foreach (var project in projects)
        {
            //for (int i = 0; i < 360; ++i)
            //{
            //    // Adds a layer option for the given layer to m_layerButtonPanel at local position Vector3.zero.
            //    GameObject projectButtonDebug = DynamicallyAddButton(project, i);
            //}

            // Items in a row are sorted left-to-right
            yAngle = -m_totalAngleY * 0.5f;
            int indexInRow = (projectIndex % m_numProjectButtonsPerRow);
            yAngle += angleStep * indexInRow;

            int rowIndex = (int)Math.Floor((double)projectIndex / m_numProjectButtonsPerRow);

            // Rows are sorted top-to-bottom.
            y = yStep  * ((0.5f * (numRows -1)) - rowIndex);

            // Adds a layer option for the given layer to m_layerButtonPanel at local position Vector3.zero.
            GameObject projectButton = DynamicallyAddButton(project, y, yAngle);
            
            ++projectIndex;
        }


        //var contentRectTransform = m_scrollView.content.GetComponent<RectTransform>();

        //var contentSize = contentRectTransform.sizeDelta;

        //contentSize.y = y + 20;

        //contentRectTransform.sizeDelta = contentSize;

        //m_scrollView.content.set.rect.height = y + yStep;
    }

    private GameObject DynamicallyAddButton(
        Project project,
        float yOffset,
        float yAngleOffset)
    {
        //var text = m_layerOptionPrefab.transform.Find("LayerOptionText");

        //if (null == text)
        //{
        //    Debug.LogError("text = null");
        //    return null; ;
        //}

        //var textRectTransform = text.GetComponent<RectTransform>();

        //if (null != textRectTransform)
        //{
        //    textRectTransform.offsetMax = Vector2.zero;
        //    textRectTransform.offsetMin = Vector2.zero;
        //}

        var button = (GameObject)Instantiate(m_projectButtonPrefab);

        if (null == button)
        {
            Debug.LogError("button = null");
            return null;
        }

        button.SetActive(true);
        button.name = "ProjectButton<WS_" + project.m_name;

        // Set the rect transform top offset for the layer option UI control.
        var bsp = button.GetComponent<ButtonSelectProjectWS>();

        var rectTransform = button.GetComponent<RectTransform>();

        if (null != rectTransform)
        {
            // rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, rectTransform.offsetMax.y - yOffset);
            var pos = rectTransform.localPosition;
            pos.y += yOffset;
            rectTransform.localPosition = pos;

            //
            rectTransform.Rotate(Vector3.up, yAngleOffset);
        }

        // Add layer option UI Control to its parent UI control.
        button.transform.SetParent(m_projectButtonPrefab.transform.parent, false);

        // Initialize layer option UI control local scale, rotation and offset.
        //option.transform.localScale = Vector3.one;
        //option.transform.localRotation = Quaternion.identity;            

        var projectSelectButton = bsp.GetButton().GetComponent<ProjectSelectButton>();

        if (null == projectSelectButton)
        {
            Debug.LogError("projectSelectButton == null");
            return null;
        }
        else
        {
            projectSelectButton.SetProject(project);
        }

        return button;
    }
}
