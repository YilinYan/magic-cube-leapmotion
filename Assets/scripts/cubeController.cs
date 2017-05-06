using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubeController : MonoBehaviour {

	public GameObject magicCube;
	public float rotateSpeed = 1f;
	Vector3[] vecPoint;
	GameObject[] cubes;
	GameObject targetCude;
	Vector3 normal;
	Vector3 mouseStart;
	Vector3 mouseRun;
	Vector3 mouseCross;
	Vector3 rotateAxis;
	float tmp = 0f;
	bool isRun, isRotate;

	// Use this for initialization
	void Start () {
		vecPoint = new Vector3[]{ Vector3.forward, Vector3.back, Vector3.up, Vector3.down, Vector3.left, Vector3.right };
		cubes = GameObject.FindGameObjectsWithTag ("Cube");
		isRun = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0) && isRun) {
		//	print ("is tragger");
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			//print (ray);
			RaycastHit hitStart;
			if (Physics.Raycast (ray.origin, ray.direction, out hitStart)) {
				targetCude = hitStart.transform.gameObject;
				//print (targetCude.transform.position);
				normal = hitStart.normal;
				print (normal);
				mouseStart = hitStart.point;
			}
		}

		if (Input.GetMouseButton (0) && targetCude != null) {
			Ray rayRun = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hitrun;
			if (Physics.Raycast (rayRun.origin, rayRun.direction, out hitrun)) {
				mouseRun = hitrun.point - mouseStart;
				if (mouseRun.sqrMagnitude > 0.1f) {
					mouseCross = Vector3.Cross (normal, mouseRun).normalized;
					RotatePoint (mouseCross);
					targetCude = null;
				}
			}
		}
		if (isRotate) {
			tmp += Time.deltaTime * rotateSpeed;
			print (tmp);
			if (tmp >= 1) tmp = 1;
			Vector3 rotate = rotateAxis * Mathf.Clamp01 (tmp)*90f;
			transform.eulerAngles = rotate;
			if (tmp >= 1) {
				tmp = 0;
				foreach (GameObject cube in cubes) {
					cube.transform.parent = magicCube.transform;
					cube.transform.localScale = Vector3.one;
				}
				transform.rotation = Quaternion.identity;
				isRotate = false;
				isRun = true;
			}
			
		}
	}

	void RotatePoint(Vector3 cross){
		for (var i = 0; i < 6; ++i) {
			Vector3 vec = vecPoint [i];
			float dot = Vector3.Dot (vec, cross);
			if (dot > 0.72f && dot <= 1f) {
				rotateAxis = vec;
				for (int j = 0; j < 3; ++j) {
					if (Mathf.Abs (vec [j]) == 1) {
						FindFather (j);
						//print (vec);
					}
				}
				break;
			}
		}
	}

	void FindFather(int p){
		float targetP = targetCude.transform.position[p];
		foreach (GameObject cube in cubes) {
			float tmpP = cube.transform.position[p];
			if (Mathf.Abs (targetP - tmpP) < 0.5f) {
				cube.transform.parent = gameObject.transform;
				print (cube.transform);
			}
		}
		isRun = false;
		isRotate = true;
	}

}

