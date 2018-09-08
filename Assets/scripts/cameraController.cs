using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour {

	//Transform cameraTarget;
	//double horizontalSpeed=2.0;  //水平移动速度  
	//double verticalSpeed=2.0;    //竖直移动速度  
	//float scrollSpeed=2.0;      //滚轮滚动速度  
	//float lerpSpeed=5.0;     
	float  hori;
	float vert;
	float cameraDistance;
	public Transform target;

	// Use this for initialization
	void Start () {
		hori = transform.eulerAngles.x;
		vert = transform.eulerAngles.y;
	}
	
	// Update is called once per frame
	void Update () {
		hori = vert = 0;
		if (Input.GetMouseButton (1)) {
			hori += 5f * Input.GetAxis("Mouse X");
			vert += 5f * Input.GetAxis("Mouse Y");
			//vert = Mathf.Clamp((float)vert,(float)-50.0,(float)-50.0);
		}

		Quaternion rotationAim = Quaternion.Euler((float)vert,(float)hori,0);  
		transform.RotateAround(Vector3.zero,Vector3.up, (float)hori);
		transform.RotateAround(Vector3.zero,Vector3.left, (float)vert);
		transform.Rotate (new Vector3 ((float)vert, (float)hori, 0));
		//主Camera动作  
        if (Mathf.Abs(Input.GetAxis ("Mouse ScrollWheel")) > 0.01f) {
			cameraDistance = -Input.GetAxis ("Mouse ScrollWheel") * 2f;  
			//cameraDistance = Mathf.Clamp ((float)cameraDistance, (float)2, (float)0.5);
			Vector3 tmp = (Vector3)transform.localPosition;
	
			tmp.x *= cameraDistance;
			tmp.y *= cameraDistance;
			tmp.z *= cameraDistance;

			transform.localPosition = Vector3.Lerp ((Vector3)transform.localPosition, (Vector3)transform.localPosition+tmp, (float)Time.deltaTime * 1f); 

		}
		transform.LookAt (target);
		
	}
}
