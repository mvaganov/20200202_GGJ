// http://codegiraffe.com/unity/NonStandardPlayer.unitypackage
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof( CharacterMove ) )]
[RequireComponent( typeof( Rigidbody ) )]
public class NPCWithBoxes : MonoBehaviour {
    [SerializeField] bool judge = false;
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
		if ( judge ) {
            Vector3 destination = GameObject.Find( "HouseRegion" ).transform.position;
            moveController = GetComponent<CharacterMove>();
            moveController.move.speed /= 2;
            moveController.move.SetAutoMovePosition( destination );

            var visual = transform.GetChild( 0 ).GetChild( 0 ).GetComponent<MeshRenderer>();
            UnityEngine.Assertions.Assert.IsNotNull( visual );

            var newMat = new Material( transform.GetChild( 0 ).GetChild( 0 ).GetComponent<MeshRenderer>().material);
            visual.material = newMat;
            visual.material.color = judgeColor;
        }
    }

    private void WalkTowardsSpotInRegion() {
		Vector3 position = NPCSpawnerForBoxes.RandomPointInBounds(region);// GetRandomPointWithinMeshRegion( region );
        moveController.move.SetAutoMovePosition( position );
		//NS.Lines.Make(ref line, transform.position, position, Color.black);
    }
}
