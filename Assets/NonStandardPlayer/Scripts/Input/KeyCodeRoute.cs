using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class KeyCodeRoute : UserControl {
	public KeyCode key = KeyCode.None;
	public KeyModifier[] modifiers;

	[Tooltip("Leave empty to use the first available EventSystem. Set to specific player if there are multiple players (like a split screen game)")]
	public EventSystem eventSystem;

	public bool IsModifiersSatisfied() {
		return IsModifiersSatisfied(modifiers);
	}

	protected void Start () {
		if (eventSystem == null)
		{
			eventSystem = GetEventSystem();
		}
	}
}
