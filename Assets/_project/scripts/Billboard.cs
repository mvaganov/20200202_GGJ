using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Billboard : MonoBehaviour {
	public Camera cameraToBillboardFor;
	void Start() {
		if(cameraToBillboardFor == null) { cameraToBillboardFor = Camera.main; }
	}
	void LateUpdate() {

        //Orient the camera after all movement is completed this frame to avoid jittering
        transform.LookAt( transform.position + cameraToBillboardFor.transform.rotation * Vector3.forward,
			cameraToBillboardFor.transform.rotation * Vector3.up );
    }
}
