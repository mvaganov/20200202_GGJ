// http://codegiraffe.com/unity/NonStandardPlayer.unitypackage
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof( CharacterMove ) )]
[RequireComponent( typeof( Rigidbody ) )]
public class NPCWithBoxes : MonoBehaviour {
    [SerializeField] bool politician = false;
    [SerializeField] Color judgeColor = Color.white;
    private BoxCollider region;
    private CharacterMove moveController;
	public MeshRenderer skin;
	private GameObject line;
	public enum SkinColors { random, simpson, light, medium_light, medium, medium_dark, dark }
	public SkinColors skinColor;
	public static Color[] skinColors = {
		Color.magenta,
		new Color(1.0f,.86f,.36f),
		new Color(.97f,.87f,.80f),
		new Color(.95f,.82f,.64f),
		new Color(.84f,.67f,.53f),
		new Color(.68f,.49f,.34f),
		new Color(.48f,.32f,.24f),
	};
	public void RefreshSkinColor()
	{
		if (skin) {
			skin.material.color = skinColors[(int)skinColor];
		}
	}

    public void BeginPathing(BoxCollider boundingRegion ) {
        moveController = GetComponent<CharacterMove>();
        region = boundingRegion;
        float timeBetweenPaths = Random.Range(5, 10);
        InvokeRepeating( "WalkTowardsSpotInRegion", 0, timeBetweenPaths );
		moveController.move.clickToMove.whatToDoWhenTargetIsReached = () => {
			WalkTowardsSpotInRegion();
		};
	}

	private void Start() {
		if(skinColor == SkinColors.random) {
			skinColor = (SkinColors)Random.Range(1, skinColors.Length);
		}
		RefreshSkinColor();
		if ( politician ) {
            Vector3 destination = GameObject.Find( "HouseRegion" ).transform.position;
            moveController = GetComponent<CharacterMove>();
            moveController.move.speed /= 2;
            moveController.move.SetAutoMovePosition( destination );

            var visual = transform.GetChild( 0 ).GetChild( 0 ).GetComponent<MeshRenderer>();
            UnityEngine.Assertions.Assert.IsNotNull( visual );

            var newMat = new Material( transform.GetChild( 0 ).GetChild( 0 ).GetComponent<MeshRenderer>().material);
            visual.material = newMat;
            visual.material.color = judgeColor;

			//NS.Lines.MakeArrow(ref line, transform.position, destination, Color.black, 1, 1);
			Election.Instance.electors.Add(GetComponent<MaslowMeter>());
        }
    }

    private void WalkTowardsSpotInRegion() {
		Vector3 position = NPCSpawnerForBoxes.RandomPointInBounds(region);// GetRandomPointWithinMeshRegion( region );
        moveController.move.SetAutoMovePosition( position );
		//NS.Lines.Make(ref line, transform.position, position, Color.black);
    }
	private bool myVote = false;

	private static int[] noVotes = { 147, 642, 1762 };
	private static int[] yesVotes = { 1853, 1909 };
	private static int unknownVote = 2740;
	private static bool gameIsWon = false;

	public UnityEngine.UI.Image voteSlot;
	private static int totalRounds = 0;

	public void Vote( MaslowMeter.MyVote vote ) {
		Sprite voteSprite = null;
		if (vote == MaslowMeter.MyVote.yea) {
			voteSprite = MaslowManager.Instance.emojiSprites[yesVotes[Random.Range(0, yesVotes.Length)]];
		} else {
			voteSprite = MaslowManager.Instance.emojiSprites[noVotes[Random.Range(0, noVotes.Length)]];
		}
		// TODO move a bunch of this code to Election...
		int yesVoteCountThisTime, noVoteCountThisTime;
		Election.Instance.Tally(out yesVoteCountThisTime, out noVoteCountThisTime);
		if(voteSprite != null)
		{
			voteSlot.sprite = voteSprite;
		}
		int totalVotes = noVoteCountThisTime + yesVoteCountThisTime;
		//Debug.Log("VOTES: " + totalVotes+"   "+ yesVoteCountThisTime+" vs "+ noVoteCountThisTime);
		if (noVoteCountThisTime >= 3 || 
			(totalVotes == 5 && noVoteCountThisTime >= 3))
		{
			Noisy.PlaySound("Judge made bad choice");
		}
		if (yesVoteCountThisTime >= 3 || 
			(totalVotes == 5 && yesVoteCountThisTime >= 3))
		{
			Noisy.PlaySound("Judge made good choice");
		}
		// after 5 votes, increment the round
		if(totalVotes == 5) {
			totalRounds++;
		}
		if(yesVoteCountThisTime >= 3)
		{
			Noisy.PlaySound("Win game");
			//UnityEngine.SceneManagement.SceneManager.LoadScene("Win Screen");
			gameIsWon = true;
			Camera.main.GetComponent<CharacterCamera>().target = Election.Instance.transform;
			Election.Instance.victory.gameObject.SetActive(true);
			Election.Instance.victory.Play();
			return;
		}
		if(noVoteCountThisTime >= 3 && totalRounds >= 5)
		{
			Noisy.PlaySound("Lose game");
			UnityEngine.SceneManagement.SceneManager.LoadScene("Lose Screen");
		}
		if (totalVotes == 5)
		{
			// clear votes
			Election.Instance.ResetVotes(unknownVote);
			// restart people at their start locations
			CharacterMove[] cms = FindObjectsOfType<CharacterMove>();
			System.Array.ForEach(cms, cm => { cm.transform.position = cm.startPosition; });
			// bring up water
			Election.Instance.sealevel.position = Election.Instance.sealevel.position + Vector3.up * 5;
		}
	}
}
