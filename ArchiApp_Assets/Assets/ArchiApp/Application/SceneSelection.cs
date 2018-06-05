using Assets.WM.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.ArchiApp.Application.Entities;

namespace Assets.ArchiApp.Application.UI
{
    public class SceneSelection : SceneSelectionBase
    {
        public List<Text> m_selectedProjectNameTextArray = new List<Text>();

        public InputSwipe m_inputSwipe = null;

        public Button m_prevProjectButton = null;
        public Button m_nextProjectButton = null;

        public Image m_projectPreview = null;
        
        public void OnSwipe(object sender, SwipeEventArgs e)
        {
            Debug.Log("OnSwipe(" + e.dir + ")");

            switch (e.dir)
            {
                case SwipeDirection.up:
                case SwipeDirection.right:
                    m_projectManager.ActivateNextProject();
                    break;

                case SwipeDirection.down:
                case SwipeDirection.left:
                    m_projectManager.ActivatePreviousProject();
                    break;

                default:
                    Debug.LogWarning("Unsupported swipe direction! (" + e.dir + ")");
                    break;
            }
        }

        // Use this for initialization
        void Start()
        {
            Debug.Log("SceneSelectionBase.Start()");

            if (m_prevProjectButton)
            {
                Button btn = m_prevProjectButton.GetComponent<Button>();
                btn.onClick.AddListener(PrevProjectButtonOnClick);
            }

            if (m_nextProjectButton)
            {
                Button btn = m_nextProjectButton.GetComponent<Button>();
                btn.onClick.AddListener(NextProjectButtonOnClick);
            }

            if (m_inputSwipe)
            {
                m_inputSwipe.SwipeEvent += this.OnSwipe;
            }
        }       

        private void SetSelectedProjectPreview(Sprite spriteProjectPreview)
        {
            if (!m_projectPreview)
            {
                return;
            }

            m_projectPreview.sprite = spriteProjectPreview;
        }

        private void SetSelectedProjectName(String text)
        {
            foreach (var selectedProjectNameText in m_selectedProjectNameTextArray)
            {
                selectedProjectNameText.text = text;
            }
        }

        override protected void SetActiveProject(Project project)
        {
            if (null == project)
            {
                SetSelectedProjectPreview(null);
                SetSelectedProjectName("No project selected");
            }
            else
            {
                SetSelectedProjectPreview(project.m_image);
                SetSelectedProjectName(project.m_name);
            }
        }

        void PrevProjectButtonOnClick()
        {
            Debug.Log("PrevProjectButtonOnClick()");

            m_projectManager.ActivatePreviousProject();
        }

        void NextProjectButtonOnClick()
        {
            Debug.Log("NextProjectButtonOnClick()");

            m_projectManager.ActivateNextProject();
        }
    }
}