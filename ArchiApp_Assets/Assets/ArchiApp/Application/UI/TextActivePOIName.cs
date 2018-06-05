using Assets.ArchiApp.Application.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.WM.Scripts.UI
{
    public class TextActivePOIName : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {

            var activePOI = POIManager.GetInstance().GetActivePOI();
            var text = (activePOI ? activePOI.name : "No POI active.");

            gameObject.GetComponent<Text>().text = text;
        }
    }
}
