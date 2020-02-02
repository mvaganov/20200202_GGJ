using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasForCanvasUIElement : MonoBehaviour
{
	private static CanvasForCanvasUIElement _instance;

	public Camera uiCamera;

	public static CanvasForCanvasUIElement Instance() {
		if(_instance == null) { _instance = FindObjectOfType<CanvasForCanvasUIElement>(); }
		return _instance;
	}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
