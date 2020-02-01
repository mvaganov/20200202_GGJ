// http://codegiraffe.com/unity/NonStandardPlayer.unitypackage
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof( CharacterMove ) )]
[RequireComponent( typeof( Rigidbody ) )]
public class NPC : MonoBehaviour {
    private MeshCollider region;
    private CharacterMove moveController;

    public void BeginPathing( MeshCollider boundingRegion ) {
        moveController = GetComponent<CharacterMove>();
        region = boundingRegion;
        float timeBetweenPaths = Random.Range(5, 10);
        InvokeRepeating( "WalkTowardsSpotInRegion", 0, timeBetweenPaths );
    }

    private void WalkTowardsSpotInRegion() {
        Vector3 position = NPCSpawner.GetRandomPointWithinMeshRegion( region );
        moveController.move.SetAutoMovePosition( position );

        //Debug.Log( "Path!" );
        //CharacterMove.ClickToMoveDetails moveCommand = new CharacterMove.ClickToMoveDetails();
        //moveCommand.targetPosition =
        //moveCommand.isNPC = true;
        //moveController.ClickToMove = true;
        //moveController.move.clickToMove = moveCommand;
    }
}
