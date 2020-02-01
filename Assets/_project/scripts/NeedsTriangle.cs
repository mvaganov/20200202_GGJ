using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NeedsTriangle : MonoBehaviour
{
	public NeedUI[] triangleLayers;

	[System.Serializable]
	public class Need {
		public string name;
		public Color color;
		public float value;
		public float lossPerSecond;
		public float gainPerClick;
		public NeedUI ui;
		public Need dependency;
		public void Update(float deltaTime) {
			value -= lossPerSecond * deltaTime;
			if (value < 0) { value = 0; }
			float max = dependency != null ? dependency.value : 1;
			if (value > max) { value = max; }
			ui.progressbar.fillAmount = Mathf.Clamp(value, 0, 1);
		}
		public void Click() {
			value += gainPerClick;
		}
		public void Use(NeedUI ui, Need dependency) {
			this.dependency = dependency;
			this.ui = ui;
			ui.text.text = name;
			ui.progressbar.color = color;
		}
	}

	public Need[] needs = new Need[] {
		new Need{color = new Color(1.0f,0.0f,0.0f,0.5f), value = .5f, lossPerSecond = 0.010f, gainPerClick = .1f, name = "pizza" },
		new Need{color = new Color(1.0f,0.7f,0.0f,0.5f), value = .5f, lossPerSecond = 0.008f, gainPerClick = .1f, name = "job" },
		new Need{color = new Color(0.5f,1.0f,0.0f,0.5f), value = .5f, lossPerSecond = 0.006f, gainPerClick = .1f, name = "friends" },
		new Need{color = new Color(0.0f,1.0f,1.0f,0.5f), value = .5f, lossPerSecond = 0.004f, gainPerClick = .1f, name = "feelin' swell" },
		new Need{color = new Color(0.0f,0.5f,1.0f,0.5f), value = .5f, lossPerSecond = 0.002f, gainPerClick = .1f, name = "political change" },
	};

	void Start()
    {
		if (triangleLayers == null || triangleLayers.Length == 0) {
			triangleLayers = GetComponentsInChildren<NeedUI>();
		}
		if (needs.Length > triangleLayers.Length) { throw new System.Exception("need at least "+ needs.Length+" layers in the pyramid"); }
		for(int i = 0; i < needs.Length; ++i)
		{
			needs[i].Use(triangleLayers[i], i != 0? needs[i-1] : null);
		}
		System.Array.ForEach(needs, (need) => {
			need.ui.button.onClick.AddListener(() => {
				//Debug.Log("clicked " + need.name);
				need.Click();
			});
		});
	}

	void Update()
    {
		System.Array.ForEach(needs, (need) => need.Update(Time.deltaTime));
    }
}
