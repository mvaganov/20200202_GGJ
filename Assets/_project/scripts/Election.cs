using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Election : MonoBehaviour
{
	public Transform sealevel;
	private static Election _instance;
	public static Election Instance {
		get {
			if (_instance == null) { _instance = FindObjectOfType<Election>(); }
			return _instance;
		}
	}

	public List<MaslowMeter> electors = new List<MaslowMeter>();

	public void Tally(out int yea, out int nay)
	{
		yea = 0; nay = 0;
		for (int i = 0; i < electors.Count; ++i)
		{
			//Debug.Log(electors[i] + " " + electors[i].vote);
			if (electors[i].vote == MaslowMeter.MyVote.yea)
			{
				++yea;
			}
			if (electors[i].vote == MaslowMeter.MyVote.nay)
			{
				++nay;
			}
		}
	}

	public void ResetVotes(int unknownVoteSpriteID)
	{
		for (int i = 0; i < electors.Count; ++i)
		{
			electors[i].vote = MaslowMeter.MyVote.abstain;
			electors[i].GetComponent<NPCWithBoxes>().voteSlot.sprite = MaslowManager.Instance.emojiSprites[unknownVoteSpriteID];
		}
	}
}