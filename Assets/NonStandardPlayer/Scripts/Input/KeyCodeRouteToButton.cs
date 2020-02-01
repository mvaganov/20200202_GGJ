using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KeyCodeRouteToButton : KeyCodeRoute {

	private Button button;
	private bool on = false;

	new void Start() {
		base.Start();
		button = GetComponent<Button>();
	}

	void Update () {
		if(key != KeyCode.None) {
			if (Input.GetKeyDown(key) && IsModifiersSatisfied()) {
				PointerEventData edata = new PointerEventData(eventSystem);
				button.OnPointerDown(edata);
				on = true;
			}
			if (Input.GetKeyUp(key) && on) {
				PointerEventData edata = new PointerEventData(eventSystem);
				button.OnPointerUp(edata);
				on = false;
			}
		}
	}
}
