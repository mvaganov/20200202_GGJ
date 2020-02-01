using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaslowMeter : MonoBehaviour {
    [SerializeField] TMPro.TextMeshProUGUI txtStatus;
    [SerializeField] MeshRenderer innerGlow;
    [SerializeField] Material matGreen;
    [SerializeField] Material matRed;
    [SerializeField] Material matWhite;

    HashSet<Transform> peopleThisPersonInfluenced = new HashSet<Transform>();

    // Stats of [-10, 10]
    public float happy = 0;
    public float safety = 0;
    public float food = 0;
    bool dirty = true;

    GameObject player;
    float kTransparencyRange = 5;

    void Start() {
        player = GameObject.Find( "player" );
        UnityEngine.Assertions.Assert.IsNotNull( txtStatus );
        if ( tag == "Player" ) {
            // No change since last generation
        }
        else if ( tag == "Judge" ) {
            // Judges start negative
            happy = -10;
            safety = -10;
            food = -10;
        }
        else {
            happy = UnityEngine.Random.Range( -7.0f, 7.0f );
            safety = UnityEngine.Random.Range( -7.0f, 7.0f );
            food = UnityEngine.Random.Range( -7.0f, 7.0f );
        }
    }

    private void SetTransBasedOnPlayerDist() {
        float distToPlayer = Vector3.Distance(transform.position, player.transform.position);
        Color newColor = txtStatus.color;
        newColor.a = kTransparencyRange/distToPlayer;

        if ( tag == "Player" )
            newColor.a = 1;

        txtStatus.color = newColor;
    }


    void Update() {
        SetTransBasedOnPlayerDist();

        if ( dirty && txtStatus.gameObject.activeInHierarchy ) {
            string happyColor = "white";
            string safetyColor = "white";
            string foodColor = "white";

            int netResult = 0;

            if ( happy >= 5f ) {
                happyColor = "green";
                netResult += 1;
            }

            if ( happy <= -5f ) {
                happyColor = "red";
                netResult -= 1;
            }

            if ( safety >= 5f ) {
                safetyColor = "green";
                netResult += 1;
            }

            if ( safety <= -5f ) {
                safetyColor = "red";
                netResult -= 1;
            }

            if ( food >= 5f ) {
                foodColor = "green";
                netResult += 1;
            }

            if ( food <= -5f ) {
                foodColor = "red";
                netResult -= 1;
            }

            if ( netResult > 0 ) {
                innerGlow.material = matGreen;
            }
            else if ( netResult < 0 ) {
                innerGlow.material = matRed;
            }
            else {
                innerGlow.material = matWhite;
            }

            string happyNumber = happy.ToString() + "         ";
            string safetyNumber = safety.ToString()+ "         ";
            string foodNumber = food.ToString()+ "         ";

            happyNumber = happyNumber.Substring( 0, 4 );
            safetyNumber  = safetyNumber.Substring( 0, 4 );
            foodNumber = foodNumber.Substring( 0, 4 );

            happyNumber = happyNumber.Contains( "-" ) ? happyNumber : "+"+happyNumber;
            safetyNumber = safetyNumber.Contains( "-" ) ? safetyNumber : "+"+safetyNumber;
            foodNumber = foodNumber.Contains( "-" ) ? foodNumber : "+"+foodNumber;

            happyNumber = happyNumber.Substring( 0, 3 ).Replace( ".", " " );
            safetyNumber = safetyNumber.Substring( 0, 3 ).Replace( ".", " " );
            foodNumber = foodNumber.Substring( 0, 3 ).Replace( ".", " " );

            txtStatus.text = "";
            txtStatus.text +=    "<sprite=14> " + "<color=\""+happyColor+"\">" + happyNumber;
            txtStatus.text +=  "\n<sprite=2> "  + "<color=\""+safetyColor+"\">" + safetyNumber;
            txtStatus.text +=  "\n<sprite=11> " + "<color=\""+foodColor+"\">" + foodNumber;

            dirty = false;
        }
    }

    private void OnTriggerEnter( Collider other ) {
        // Judges don't influence others
        if ( tag == "Judge" )
            return;

        if ( other.tag == "Villager" || other.tag == "PlayerBubble" ) {
            if ( !peopleThisPersonInfluenced.Contains( other.transform.parent ) ) {
                peopleThisPersonInfluenced.Add( other.transform.parent );
                Debug.Log( gameObject.name + "Influenced " + other.gameObject.name );

                MaslowMeter otherMeter = other.gameObject.transform.parent.GetComponent<MaslowMeter>();
                UnityEngine.Assertions.Assert.IsNotNull( otherMeter );
                otherMeter.Influence( happy, safety, food );
            }
        }
    }

    // Based on input numbers [-10, 10] this person's numbers are influenced to change
    private void Influence( float phappy, float psafety, float pfood ) {
        // Right now influence is always +/- 1 or 0 
        if ( Math.Abs( phappy ) >= 5 ) {
            happy += phappy / Math.Abs( phappy );
            happy = Mathf.Clamp( happy, -10, 10 );
        }

        if ( Math.Abs( psafety ) >= 5 ) {
            safety += psafety / Math.Abs( psafety );
            safety = Mathf.Clamp( safety, -10, 10 );
        }

        if ( Math.Abs( pfood ) >= 5 ) {
            food += pfood / Math.Abs( pfood );
            food = Mathf.Clamp( food, -10, 10 );
        }

        dirty = true;
    }
}
