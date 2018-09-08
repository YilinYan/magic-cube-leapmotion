using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wholeController : MonoBehaviour {

    public Transform hand;
    Vector3 previous, now, velo;

	// Use this for initialization
	void Start () {
        previous = hand.position;
	}
	
	// Update is called once per frame
	void Update () {

        now = hand.position;
        if ((now - previous).sqrMagnitude > 1f)
        {
            velo = velo * 0.99f + now - previous;
        }
        else velo *= 0.99f;

       // RotateCamera(veloSimu);

    }
     
}
