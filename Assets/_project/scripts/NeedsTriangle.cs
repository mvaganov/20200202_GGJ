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

	public enum Show { inChest, playerUI, aboveHead }
	public Show triangleShow = Show.inChest;
	private Rigidbody2D rb;

	public void SetShow(Show triangleShow) {
		UpdateTriangleUIParent(triangleShow);
	}

	public void UpdateTriangleUIParent(Show triangleShow) {
		Transform maslowTransform = transform.parent;
		Transform placeTobe = null;
		if (rb != null) { rb.simulated = false; }
		Show lastState = this.triangleShow;
		this.triangleShow = triangleShow;
		switch (triangleShow) {
			case Show.inChest:
				if (lastState != Show.inChest) {
					Debug.Log("bringing to chest from " + lastState);
				}
				if(maslow.chestCanvas == null) {
					Debug.Log("need to set chest canvas for "+maslow.gameObject+" ("+maslow.transform.root+")");
				}
				placeTobe = maslow.chestCanvas.transform;
				SetTextVisible(false);
				break;
			case Show.playerUI:		placeTobe = CanvasForCanvasUIElement.Instance().transform.Find("maslowanchor"); break;
			case Show.aboveHead:
				RectTransform ui = maslow.headScreenUI.GetComponent<CanvasUIElement>().ui;
				if(ui == null) {
					NS.Chrono.setTimeout(()=>UpdateTriangleUIParent(triangleShow), 100);
					//Debug.Log("waiting for UI to happen...");
					return;
				}
				if (rb != null) { rb.simulated = true; isColliding = false; }
				placeTobe = ui.Find("maslowanchor");
				SetTextVisible(true);
				break;
		}
		//if (maslowTransform.parent != placeTobe)
		{
			if(placeTobe == null) { Debug.Log("no place to be when state is " + triangleShow); }
			maslowTransform.SetParent(placeTobe);
			maslowTransform.localScale = Vector3.one;
			maslowTransform.localPosition = Vector3.zero;
			maslowTransform.rotation = Quaternion.identity;
		}
	}

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
		rb = GetComponent<Rigidbody2D>();
		UpdateTriangleUIParent(triangleShow);
	}

	void FixedUpdate()
    {
		//TODO: Move this update into maslow
		//System.Array.ForEach(maslow.needs, (need) => need.Update(Time.deltaTime));

		if(triangleShow == Show.aboveHead) {
			RectTransform rt = GetComponent<RectTransform>();
			if (!isColliding) {
				rb.velocity = -rt.anchoredPosition;
			} else {
				rb.velocity += -rt.anchoredPosition * Time.deltaTime;
			}
		}
	}
	private bool isColliding = false;
	private void OnCollisionStay2D(Collision2D collision) { isColliding = true; }
	private void OnCollisionExit2D(Collision2D collision) { isColliding = false; }
}
