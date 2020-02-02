using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasUIElement : MonoBehaviour
{
	public RectTransform prefab;
	/// <summary>
	/// Made public to allow debugging.
	/// </summary>
	public RectTransform ui;
	public bool activated = false;
	public bool hideIfDeactivated = true;
	
	public class CanvasUIElementElement : MonoBehaviour {
		public CanvasUIElement owner;
	}

	public RectTransform GetUI() {
		if(ui == null) {
			CanvasForCanvasUIElement c = CanvasForCanvasUIElement.Instance();
			ui = (Instantiate(prefab.gameObject) as GameObject).GetComponent<RectTransform>();
			ui.SetParent(c.transform);
			ui.name += ui.parent.childCount;
			CanvasUIElementElement slave = ui.gameObject.AddComponent<CanvasUIElementElement>();
			slave.owner = this;
		}
		return ui;
	}

    void Update() {
		if (activated) {
			CanvasForCanvasUIElement c = CanvasForCanvasUIElement.Instance();
			Vector2 position = c.uiCamera.WorldToScreenPoint(transform.position);
			ui = GetUI();
			ui.position = position;
		}
		else
		{
			ui.gameObject.SetActive(false);
		}
    }
}
