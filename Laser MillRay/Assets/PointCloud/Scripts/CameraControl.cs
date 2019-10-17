using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


public class CameraControl : MonoBehaviour {
    public float xMin, xMax, zMin, zMax;
    //public Vector3 mirar = Vector3.zero;
    public Transform cameraT;

    public float sensitivity = 1f;
    
    //private CameraRefocus _cameraRefocus;

    // Use this for initialization
    void Start() {

    }


    // Update is called once per frame
    void Update() {
        //Debug.Log(cameraT.forward);

        float moveHorizontal = CrossPlatformInputManager.GetAxisRaw("Horizontal");
        float moveVertical = CrossPlatformInputManager.GetAxisRaw("Vertical");
        //Debug.Log("Horizontal " + moveHorizontal);

        Vector3 movement =  (Vector3.ProjectOnPlane( cameraT.forward,Vector3.up).normalized * moveVertical + Vector3.ProjectOnPlane(cameraT.right, Vector3.up).normalized * moveHorizontal) * Time.deltaTime * sensitivity;



        Vector3 pos = transform.position + movement;
        pos.x = Mathf.Clamp(pos.x, xMin, xMax);
        pos.z = Mathf.Clamp(pos.z, zMin, zMax);

        transform.position = pos;
    }
        
}


