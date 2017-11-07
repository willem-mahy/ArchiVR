using System.Collections;
using System.Collections.Generic;
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
        SceneManager.LoadScene(m_projectName);
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("ViewProject", LoadSceneMode.Additive);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode arg1)
    {
        // Make sure the 'Project' scene has been loaded.
        var sp = SceneManager.GetSceneByName(m_projectName);

        if (!sp.IsValid())
            return;

        if (!sp.isLoaded)
            return;

        // Make sure the 'ViewProject' scene has been loaded.
        var svp = SceneManager.GetSceneByName("ViewProject");

        if (!svp.IsValid())
            return;

        if (svp.isLoaded == false)
            return;

        // Set the caption for the project name.
        var textProjectName = GameObject.Find("TextProjectName");

        if (textProjectName)
        {
            var text = textProjectName.GetComponent<Text>();

            if (text)
            {
                text.text = m_projectName;
            }
        }

        var gameObjects = sp.GetRootGameObjects();

        var gameObjectWorld = gameObjects[0];

        if (gameObjectWorld)
        {
            // Migrate the 'World' gameobject from the 'Project' scene into the 'ViewProject' scene.
            SceneManager.MoveGameObjectToScene(gameObjectWorld, svp);

            SceneManager.SetActiveScene(svp);

            // Then unload the 'Project' scene.
            SceneManager.UnloadSceneAsync(sp);
        }
    }
}
