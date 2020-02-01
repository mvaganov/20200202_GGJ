using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Billboard : MonoBehaviour {

    void LateUpdate() {

        //Orient the camera after all movement is completed this frame to avoid jittering
        transform.LookAt( transform.position + Camera.main.transform.rotation * Vector3.forward,
            Camera.main.transform.rotation * Vector3.up );
    }
}
