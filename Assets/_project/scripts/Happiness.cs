using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Happiness : MonoBehaviour
{
	public float happinessPosition = 0.5f;
	public Image background;
	public Image progressbar;
	public Image sad;
	public Image happy;
	public Image indicator;

	void Start() {
        
    }

    // Update is called once per frame
    void Update() {
		happinessPosition = Mathf.Clamp(happinessPosition, 0, 1);
		progressbar.fillAmount = happinessPosition;
		indicator.rectTransform.localPosition = new Vector2((happinessPosition-0.5f) * progressbar.rectTransform.sizeDelta.x, 0);
    }
}
