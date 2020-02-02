using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public Transform target;
    public bool inverted = true;
    // Start is called before the first frame update
    void Start()
    {
        target = Camera.main.transform;   
    }

    // Update is called once per frame
    void Update()
    {
        if (!inverted)
        {
            transform.LookAt(Camera.main.transform.position);
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(transform.position - target.position);
        }
    }
}
