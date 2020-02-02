using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour {
    float goalY;
    float waterSpeed = 2;
    float epsilon = 0.1f;
    float startY =  36.8f;

    void Start() {
        goalY = transform.position.y;
    }

    public bool waterTooHigh() {
        return goalY > 64;
    }

    public void ChangeGoalHeight( float deltaHeight ) {
        goalY += deltaHeight;

        // Prevent water from getting stuck at top
        if ( goalY > 64 )
            goalY = 68.5f;
    }

    void Update() {
        float dist = transform.position.y -goalY;
        if ( Mathf.Abs( dist ) >= epsilon ) {
            Vector3 newPos = transform.position;
            newPos.y += Time.deltaTime * waterSpeed;
            transform.position = newPos;
        }
    }
    private void OnTriggerEnter( Collider other ) {
        if ( other.tag == "Player" || other.tag == "Judge" || other.tag == "House" ) {
            // Immunity
        }
        else {
            other.gameObject.SetActive( false );
        }
    }

    internal void Reset() {
        Vector3 newPos = transform.position;
        newPos.y = startY;
        transform.position = newPos;
        goalY = startY;
    }
}
