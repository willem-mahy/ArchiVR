using System;
using UnityEngine;

namespace Assets.WM.CameraNavigation.TranslationControl
{
    public class TranslationControlTeleport : TranslationControlBase
    {
        public override void UpdateTranslation(GameObject gameObject)
        {
            // TODO
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnDisable()
        {
            Debug.Log("TranslationControlTeleport.OnDisable()");
        }

        void OnEnable()
        {
            Debug.Log("TranslationControlTeleport.OnEnable()");
        }
    }
}
