using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.WM.Environment
{
    public class SkyBehavior : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            gameObject.transform.position = 
                Camera.main
                //GameObject.Find("Main Camera")
                .transform.position;
        }

        void SetSkyBox(string materialPath) // eg: "SkyboxNoonCloudy01_2048/SkyboxNoonCloudy01_2048"
        {
            if (true)
            {
                var material = Resources.Load<Material>(materialPath);

                RenderSettings.skybox = material;
            }
        }
    }
}
