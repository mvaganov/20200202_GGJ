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
    bool votingPositive = false;
    float historicalInfluence = 0;
    AudioSource thunk;


    void Start() {
        thunk = GetComponent<AudioSource>();
        player = GameObject.Find( "player" );
        UnityEngine.Assertions.Assert.IsNotNull( txtStatus );
        ReEvalInfluences( 0 );
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
                votingPositive = true;
                innerGlow.material = matGreen;
            }
            else if ( netResult < 0 ) {
                votingPositive = false;
                innerGlow.material = matRed;
            }
            else {
                votingPositive = true;
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
            txtStatus.text +=    "<sprite=593> " + "<color=\""+happyColor+"\">" + happyNumber;
            txtStatus.text +=  "\n<sprite=11> "  + "<color=\""+safetyColor+"\">" + safetyNumber;
            txtStatus.text +=  "\n<sprite=623> " + "<color=\""+foodColor+"\">" + foodNumber;

            dirty = false;
        }
    }

    internal void ReEvalInfluences( int roundNumber ) {
        peopleThisPersonInfluenced = new HashSet<Transform>();
        if ( roundNumber == 0 ) {
            historicalInfluence = 0;

            if ( tag == "Player" ) {
                // Start nuetral
                happy = 0;
                safety = 0;
                food = 0;
            }
            else if ( tag == "Judge" ) {
                // Judges start negative
                happy = -5;
                safety = -5;
                food = -5;
            }
            else {
                happy = UnityEngine.Random.Range( -6.0f, 6.0f );
                safety = UnityEngine.Random.Range( -6.0f, 8.0f );
                food = UnityEngine.Random.Range( -6.0f, 9.0f );
            }
        }
        else {
            historicalInfluence += votingPositive ? 1 : -1;
            if ( tag == "Player" ) {
                // No change since last generation
            }
            else if ( tag == "Judge" ) {
                // No change since last generation
            }
            else {
                float low = -6 + historicalInfluence;
                float high = 7 + historicalInfluence;

                // 1/3 chance pure random
                if ( UnityEngine.Random.Range( 0, 100 ) < 30 ) {
                    low = -10;
                    high = 10;
                }

                // New sentiment is halfway between last and random
                happy = ( happy + UnityEngine.Random.Range( low, high ) );
                safety = ( safety + UnityEngine.Random.Range( low, high ) );
                food = ( food + UnityEngine.Random.Range( low, high ) );
            }
        }
        happy = Mathf.Clamp( happy, -10, 10 );
        safety = Mathf.Clamp( safety, -10, 10 );
        food = Mathf.Clamp( food, -10, 10 );
        dirty = true;

    }

    private void OnTriggerEnter( Collider other ) {
        if ( tag == "Judge" ) {
            if ( other.tag == "House" ) {
                // Judges don't influence others, only the city hall
                var npc = GetComponent<NPC>();
                UnityEngine.Assertions.Assert.IsNotNull( npc );
                npc.Vote( votingPositive );
                return;
            }
        }
        else if ( other.tag == "Villager" || other.tag == "PlayerBubble" || other.tag == "Judge" ) {
            if ( !peopleThisPersonInfluenced.Contains( other.transform.parent ) ) {
                peopleThisPersonInfluenced.Add( other.transform.parent );
                Debug.Log( gameObject.name + " influenced " + other.transform.parent.gameObject.name );

                MaslowMeter otherMeter = other.gameObject.transform.parent.GetComponent<MaslowMeter>();

                if ( otherMeter == null )
                    otherMeter = other.gameObject.transform.GetComponent<MaslowMeter>();

                UnityEngine.Assertions.Assert.IsNotNull( otherMeter );
                otherMeter.Influence( happy, safety, food );

                if ( tag == "Player" ) {
                    thunk.Play();
                }
            }
        }
    }

    // Based on input numbers [-10, 10] this person's numbers are influenced to change
    private void Influence( float phappy, float psafety, float pfood ) {

        if ( tag == "Player" ) {
            thunk.Play();
        }

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
