using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;

public class cubeController : MonoBehaviour {
    public PinchDetector detector;
	public GameObject magicCube;
    public GameObject indexBone;
    public GameObject camera;
	public float rotateSpeed = 1f;
	Vector3[] vecPoint;
	GameObject[] cubes;
	GameObject targetCude;
    
	Vector3 normal;
	//Vector3 mouseStart;
	//Vector3 mouseRun;
	//Vector3 mouseCross;
	Vector3 rotateAxis;
    Vector3 indexStart;
    Vector3 indexRun;
    Vector3 indexCross;
	float tmp = 0f;
    int cnt = 0;
	bool isRun, isRotate;

    
	// Use this for initialization
	void Start () {
		vecPoint = new Vector3[]{ Vector3.forward, Vector3.back, Vector3.up, Vector3.down, Vector3.left, Vector3.right };
		cubes = GameObject.FindGameObjectsWithTag ("Cube");
		isRun = true;
	}
	
	// Update is called once per frame
	void Update () {
        print(indexBone.transform.position);
        if (detector.IsPinching) {
            print("hello!");
        }
		if (detector.IsPinching && isRun) {
			print ("is tragger");
            //Ray ray = Camera.main.ScreenPointToRay (indexBone.transform.position);
            Ray ray = new Ray(camera.transform.position, indexBone.transform.position - camera.transform.position);
            //print (ray);
			RaycastHit hitStart;
			if (Physics.Raycast (ray.origin, ray.direction, out hitStart)) {
                print("hit here!");
				targetCude = hitStart.transform.gameObject;
				//print (targetCude.transform.position);
				normal = hitStart.normal;
				//print (normal);
				indexStart = hitStart.point;
			}
		}

		if (detector.IsPinching && targetCude != null) {
            print("hello!");
            Ray rayRun = new Ray(camera.transform.position, indexBone.transform.position - camera.transform.position);
            RaycastHit hitrun;
			if (Physics.Raycast (rayRun.origin, rayRun.direction, out hitrun)) {
				indexRun = hitrun.point - indexStart;
                print(indexRun.sqrMagnitude);
                //print(string.Format("{0:000.000}", indexRun.sqrMagnitude));
                //Debug.Log("%.6f"+indexRun.sqrMagnitude);
                if (indexRun.sqrMagnitude > 0.1f) {
					indexCross = Vector3.Cross (normal, indexRun).normalized;                   
                    RotatePoint (indexCross);
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
        print("Rotate");
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

