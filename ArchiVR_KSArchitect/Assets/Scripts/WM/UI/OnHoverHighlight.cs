using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHoverHighlight : MonoBehaviour {

	// Use this for initialization
	void Start () {
        var ii = gameObject.GetComponent<VRStandardAssets.Utils.VRInteractiveItem>();

        if (ii)
        {
            ii.OnOver += HandleOver;
            ii.OnOut += HandleOut;
        }
    }
	
	// Update is called once per frame
	void Update () {
	}

    //Handle the Over event
    private void HandleOver()
    {
        Debug.Log("HandleOver");
        gameObject.transform.localScale = 1.1f * Vector3.one;
    }

    //Handle the Out event
    private void HandleOut()
    {
        Debug.Log("HandleOut");
        gameObject.transform.localScale = Vector3.one;
    }

}
