// http://codegiraffe.com/unity/NonStandardPlayer.unitypackage
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof( CharacterMove ) )]
[RequireComponent( typeof( Rigidbody ) )]
public class NPC : MonoBehaviour {
    [SerializeField] bool judge = false;
    [SerializeField] Color judgeColor = Color.white;
    [SerializeField] TMPro.TextMeshProUGUI txtVote;
    private MeshCollider region;
    private CharacterMove moveController;

    public void BeginPathing( MeshCollider boundingRegion ) {
        moveController = GetComponent<CharacterMove>();
        region = boundingRegion;
        float timeBetweenPaths = UnityEngine.Random.Range(5, 10);
        InvokeRepeating( "WalkTowardsSpotInRegion", 0, timeBetweenPaths );
    }

    private void BeginJudgePathfinding() {
        UnityEngine.Assertions.Assert.IsNotNull( txtVote );
        Vector3 destination = GameObject.Find( "HouseRegion" ).transform.position;
        moveController = GetComponent<CharacterMove>();
        moveController.move.SetAutoMovePosition( destination );
    }

    private void Start() {
        if ( judge ) {
            var visual = transform.GetChild( 0 ).GetChild( 0 ).GetComponent<MeshRenderer>();
            UnityEngine.Assertions.Assert.IsNotNull( visual );

            var newMat = new Material( transform.GetChild( 0 ).GetChild( 0 ).GetComponent<MeshRenderer>().material);
            visual.material = newMat;
            visual.material.color = judgeColor;

            moveController = GetComponent<CharacterMove>();
            moveController.move.speed /= 2;

            float timeBetweenPaths = UnityEngine.Random.Range(5, 10);
            InvokeRepeating( "BeginJudgePathfinding", 0, timeBetweenPaths );
        }
    }

    private void WalkTowardsSpotInRegion() {
        Vector3 position = NPCSpawner.GetRandomPointWithinMeshRegion( region );
        moveController.move.SetAutoMovePosition( position );
    }

    internal void Vote( bool positive ) {
        UnityEngine.Assertions.Assert.IsTrue( judge );
        txtVote.text = positive ? "+" : "-";
    }
}
