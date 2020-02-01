using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NeedUI : MonoBehaviour
{
	public Image background;
	public TMP_Text text;
	public Image progressbar;
	public Button button;

    void Awake() {
        if(button == null)
		{
			button = GetComponentInChildren<Button>();
		}
    }

    void Update() {
        
    }
}
