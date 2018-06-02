using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.WM.Settings;
using Assets.Scripts.WM;

namespace Assets.WM.Scripts.UI
{
    public class TextActivePOIName : MonoBehaviour {

        // Update is called once per frame
        void Update()
        {

            var activePOI = POIManager.GetInstance().GetActivePOI();
            var text = (activePOI ? activePOI.name : "No POI active.");

            gameObject.GetComponent<Text>().text = text;
        }
    }
}
