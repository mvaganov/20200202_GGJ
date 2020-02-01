using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class KeyCodeRouteToToggle : KeyCodeRoute {

	private Toggle toggle;
	public bool onToggleOnRelease = false;
	private bool on = false;

	new void Start() {
		base.Start();
		toggle = GetComponent<Toggle>();
	}

	public void Toggle() {
		toggle.isOn = !toggle.isOn;
	}

	void Update () {
		if(key != KeyCode.None) {
			if (Input.GetKeyDown(key) && IsModifiersSatisfied()) {
				Toggle();
				on = true;
			}
			if (onToggleOnRelease && Input.GetKeyUp(key) && on) {
				Toggle();
				on = false;
			}
		}
	}
}
