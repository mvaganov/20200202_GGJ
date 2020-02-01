using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class UserControl : MonoBehaviour {

	public static EventSystem GetEventSystem()
	{
		EventSystem es = GameObject.FindObjectOfType<UnityEngine.EventSystems.EventSystem>();
		if (es == null)
		{
			GameObject evOb = new GameObject("EventSystem");
			es = evOb.AddComponent<UnityEngine.EventSystems.EventSystem>();
			evOb.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
		}
		return es;
	}

	[System.Serializable]
	public struct KeyModifier
	{
		public KeyCode key;
		public bool notPressedExplicitly;
		public bool IsSatisfied() { return Input.GetKey(key) != notPressedExplicitly; }
	}

	public static bool IsModifiersSatisfied(KeyModifier[] modifiers)
	{
		bool working = true;
		if (modifiers != null && modifiers.Length > 0)
		{
			for (int i = 0; i < modifiers.Length; ++i)
			{
				if (!modifiers[i].IsSatisfied()) { working = false; break; }
			}
		}
		return working;
	}

}
