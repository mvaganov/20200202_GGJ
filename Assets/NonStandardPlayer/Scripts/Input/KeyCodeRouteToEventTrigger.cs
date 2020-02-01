using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class KeyCodeRouteToEventTrigger : KeyCodeRoute {

	private EventTrigger eventTrigger;
	public bool on = false;

	new void Start() {
		base.Start();
		eventTrigger = GetComponent<EventTrigger>();
	}

	void Update () {
		if(key != KeyCode.None) {
			if (Input.GetKeyDown(key) && IsModifiersSatisfied()) {
				PointerEventData edata = new PointerEventData(eventSystem);
				eventTrigger.OnPointerDown(edata);
				on = true;
			}
			if (Input.GetKeyUp(key) && on) {
				PointerEventData edata = new PointerEventData(eventSystem);
				eventTrigger.OnPointerUp(edata);
				on = false;
			}
		}
	}
}
