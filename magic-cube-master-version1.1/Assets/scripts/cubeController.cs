using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;

public class cubeController : MonoBehaviour {

    //  public Camera cam;
    public GameObject CameraRight;
    public GameObject CameraUp;
	public GameObject magicCube;
	public float rotateSpeed = 1f;
    public GameObject indexBone;
    public PinchDetector detector;
    public GameObject leftHand;
	Vector3[] vecPoint;
	GameObject[] cubes;
	GameObject targetCude;
	Vector3 normal;
	Vector3 indexStart;
	Vector3 indexRun;
	Vector3 indexCross;
	Vector3 rotateAxis;
    Vector3 pastCoordinate, nowCoordinate, veloSimu;
    float tmp = 0f;
	bool isRun, isRotate;

    float hori, vert;

	// Use this for initialization
	void Start () {
        //   cam.rigidbody.AddT
        veloSimu = Vector3.zero;
        hori = Camera.main.transform.eulerAngles.x;
        vert = Camera.main.transform.eulerAngles.y;
		vecPoint = new Vector3[]{ Vector3.forward, Vector3.back, Vector3.up, Vector3.down, Vector3.left, Vector3.right };
		cubes = GameObject.FindGameObjectsWithTag ("Cube");
		isRun = true;
        pastCoordinate = leftHand.transform.position- Camera.main.transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        // LeftHand rotate
        TryLeftHandRotate();
        //if (detector.IsPinching) {
        //print("Pinching");
        //}
        if (detector.IsPinching && isRun && targetCude == null) {
		//	print ("is tragger");
			Ray ray = new Ray(Camera.main.transform.position, indexBone.transform.position - Camera.main.transform.position);
			//print (ray);
			RaycastHit hitStart;
			if (Physics.Raycast (ray.origin, ray.direction, out hitStart)) {
				targetCude = hitStart.transform.gameObject;
				//print (targetCude.transform.position);
				normal = hitStart.normal;
				//print (normal);
				indexStart = hitStart.point;
			}
		}

		if (detector.IsPinching && targetCude != null) {
            //print("PastAndNow");
            Ray rayRun = new Ray(Camera.main.transform.position, indexBone.transform.position - Camera.main.transform.position);
			RaycastHit hitrun;
			if (Physics.Raycast (rayRun.origin, rayRun.direction, out hitrun)) {
				indexRun = hitrun.point - indexStart;
                //print("indexRun:" + indexRun);
				if (indexRun.sqrMagnitude >0.1f) {
					indexCross = Vector3.Cross (normal, indexRun).normalized;
					RotatePoint (indexCross);
					targetCude = null;
				}
			}
		}
		if (isRotate) {
			tmp += Time.deltaTime * rotateSpeed;
			//print (tmp);
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
        bool sing = false;
		for (var i = 0; i < 6; ++i) {
			Vector3 vec = vecPoint [i];
			float dot = Vector3.Dot (vec, cross);
			if (dot > 0.72f && dot <= 1f) {
				rotateAxis = vec;
                sing = true;
				for (int j = 0; j < 3; ++j) {
					if (Mathf.Abs (vec [j]) == 1) {
						FindFather (j);
						//print (vec);
					}
				}
				break;
			}
		}
        if (!sing)
            print("Something Bad Happend.");
	}

	void FindFather(int p){
        print("FindFather");
		float targetP = targetCude.transform.position[p];
		foreach (GameObject cube in cubes) {
			float tmpP = cube.transform.position[p];
			if (Mathf.Abs (targetP - tmpP) < 0.3f) {
				cube.transform.parent = gameObject.transform;
				//print (cube.transform);
			}
		}
		isRun = false;
		isRotate = true;
	}

    void TryLeftHandRotate() {

        nowCoordinate = leftHand.transform.position;
        Vector3 velo = nowCoordinate - pastCoordinate;
       // print(velo);
        if (velo.sqrMagnitude > 0.15f)
            veloSimu = veloSimu * 0.99f + (nowCoordinate - pastCoordinate);   
        else
            veloSimu = veloSimu * 0.99f;
        if (veloSimu.sqrMagnitude < 0.05f) veloSimu = Vector3.zero;
        if (veloSimu.sqrMagnitude > 0.6f) veloSimu = veloSimu / veloSimu.sqrMagnitude * 0.6f;

        print(veloSimu);
        RotateCamera(veloSimu);
        pastCoordinate = nowCoordinate;
    }

    void RotateCamera(Vector3 velo) {
       // hori = 2f * velo[0];
       // vert = 2f * velo[1];
       // Quaternion rotationAim = Quaternion.Euler((float)vert, (float)hori, 0);
        Vector3 dir = -Camera.main.transform.position;
      //  float eps = 0.0000001f;
        Vector3 right = CameraRight.transform.position - Camera.main.transform.position;
        Vector3 up = CameraUp.transform.position - Camera.main.transform.position;

        //Vector3 a = new Vector3(dir[0], 0, 0);
        //Vector3 b = new Vector3(0, 0, dir[2]);
      


        //Vector3 up = Vector3.Cross(Camera.main.transform.position, a);
        //Vector3 left = Vector3.Cross(Camera.main.transform.position, b);

        
        Camera.main.transform.RotateAround(Vector3.zero, up, (float)Vector3.Dot(velo, right));
        Camera.main.transform.RotateAround(Vector3.zero, right, (float)Vector3.Dot(-velo, up));
       // Camera.main.transform.Rotate(new Vector3((float)vert, (float)hori, 0));

        CameraRight.transform.RotateAround(Vector3.zero, up, (float)Vector3.Dot(velo, right));
        CameraRight.transform.RotateAround(Vector3.zero, right, (float)Vector3.Dot(-velo, up));

        CameraUp.transform.RotateAround(Vector3.zero, up, (float)Vector3.Dot(velo, right));
        CameraUp.transform.RotateAround(Vector3.zero, right, (float)Vector3.Dot(-velo, up));
    }
}

