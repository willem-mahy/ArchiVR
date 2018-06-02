using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR;

namespace Assets.Scripts.WM.Util
{
    class InputUtil
    {
        public static bool IsPointerOverUIObject(Vector2 position)
        {
            Debug.Log("InputUtil.IsPointerOverUIObject(" + position + ")");

            // Referencing this code for GraphicRaycaster https://gist.github.com/stramit/ead7ca1f432f3c0f181f
            // the ray cast appears to require only eventData.position.
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = position;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

            if (results.Count == 0)
            {
                Debug.Log("Not over UI Object.");
                return false;
            }

            foreach (var result in results)
            {
                Debug.Log("Over UI Object '" + result.gameObject.name + "'.");
            }

            foreach (var result in results)
            {
                if (result.gameObject.tag != "VRMenu")
                {
                    return true;
                }
            }

            return false;
        }
    }
}
