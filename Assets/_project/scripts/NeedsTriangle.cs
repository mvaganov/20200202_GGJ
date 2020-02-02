using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NeedsTriangle : MonoBehaviour
{

	/// <summary>
	/// The model/controller instance that stores the data and methods the needsTriangle provides a view with UI interactions for.
	/// </summary>
	public MaslowMeter maslow;
	public NeedUI[] triangleLayers;

/// <summary>
/// A need level enum is used to classify habits by their primary met need level.
/// </summary>
	



	public enum HappyEmoji
	{
		H0 = 1207,
		H5 = 2883,
		H10 = 2158
	}

	public void ConnectButtonsToNeeds()
	{
		if (triangleLayers == null || triangleLayers.Length == 0) 
		{
			triangleLayers = GetComponentsInChildren<NeedUI>();
		}
		if(maslow == null) {
			Debug.LogError(transform.root.name+" does not have Maslow set");
			Transform t = transform;
			while(t != null)
			{
				Debug.Log(t.name);
				maslow = t.gameObject.GetComponent<MaslowMeter>();
				if(maslow != null) {
					Debug.Log("found a MaslowMeter!");
					break;
				}
				t = t.parent;
			}
		}
		if (maslow.needs.Length > triangleLayers.Length) { 
			throw new System.Exception("need at least "+ maslow.needs.Length+" layers in the pyramid"); 
		}
		for(int i = 0; i < maslow.needs.Length; ++i)
		{
			maslow.needs[i].Use(triangleLayers[i], i != 0? maslow.needs[i-1] : null);
		}
		System.Array.ForEach(
			maslow.needs, (need) => {
				need.ui.button.onClick.AddListener(
					() => 
					{
						//Debug.Log("clicked " + need.name);
						need.Click();
					}
				);
			}
		);
	}
	
	public void SetTextVisible(bool visible) {
		for(int i = 0; i < triangleLayers.Length; ++i) {
			triangleLayers[i].text.gameObject.SetActive(visible);
		}
	}

	void Start()
    {
		ConnectButtonsToNeeds();
		SetTextVisible(false);
	}

	void Update()
    {
		//TODO: Move this update into maslow
		//System.Array.ForEach(maslow.needs, (need) => need.Update(Time.deltaTime));
    }
}
