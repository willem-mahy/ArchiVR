using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frame : MonoBehaviour {

    public float size = 100f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}    

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(Vector3.right * size, Vector3.zero);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(Vector3.up * size, Vector3.zero);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(Vector3.forward * size, Vector3.zero);
        Gizmos.color = Color.white;
    }
}
