using Assets.Scripts.WM.ArchiVR.Application;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ProjectSelectButton : MonoBehaviour {
    public string m_projectName = "";

	// Use this for initialization
	void Start () {
        gameObject.GetComponent<Button>().onClick.AddListener(OnClick);

        var scenePath = "Assets/Scenes/" + m_projectName + ".unity";

        // Verify that the project exists.
        if (SceneUtility.GetBuildIndexByScenePath(scenePath) == -1)
        {
            var msg = "Scene '" + scenePath + "' not found in build scenes!";
            Debug.Log(msg);
            //throw new System.Exception(msg);
        }

        // Load the 'project preview' Sprite from project Reosources ('Assets/Resources')
        var spriteProjectPreviewResourcePath = "ProjectPreview/" + m_projectName;
                
        var spriteProjectPreview = Resources.Load<Sprite>(spriteProjectPreviewResourcePath);

        if (null == spriteProjectPreview)
        {
            var msg = "Project preview Sprite '" + spriteProjectPreviewResourcePath + "' not found in resources!";
            Debug.Log(msg);
            //throw new System.Exception(msg);
        }

        var buttonImageComponent = gameObject.transform.GetComponentInChildren<Image>();

        buttonImageComponent.sprite = spriteProjectPreview;
    }

    void OnClick()
    {
        ApplicationState.OpenProject(m_projectName);
    }    
}
