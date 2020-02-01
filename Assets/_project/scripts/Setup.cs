using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setup : MonoBehaviour {
    [SerializeField] GameObject UI;
    void Start() {
        UI.gameObject.SetActive( true );
    }

}
