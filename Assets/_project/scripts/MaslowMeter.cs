using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaslowMeter : MonoBehaviour {
    [SerializeField] TMPro.TextMeshProUGUI txtStatus;
    //[SerializeField] MeshRenderer innerGlow;
    [SerializeField] Material matGreen;
    [SerializeField] Material matRed;
    [SerializeField] Material matWhite;

    
    HashSet<Transform> peopleThisPersonInfluenced = new HashSet<Transform>();

    // Stats of [-10, 10]
    public float happy = 5;
    public float health = 5;
    public float safety = 5;
    public float belonging = 5;
    public float esteem = 5;
    public float actualization = 5;

/// <summary>
/// The view UI the maslowmeter is connected to when a UI is visible.
/// </summary>
    public NeedsTriangle triangle;


 public Need[] needs = new Need[] {
		new Need{color = new Color(1.0f,0.0f,0.0f,0.5f), value = .5f, lossPerSecond = 0.010f, gainPerClick = .1f, name = "physiology" },
		new Need{color = new Color(1.0f,0.7f,0.0f,0.5f), value = .5f, lossPerSecond = 0.008f, gainPerClick = .1f, name = "safety" },
		new Need{color = new Color(0.5f,1.0f,0.0f,0.5f), value = .5f, lossPerSecond = 0.006f, gainPerClick = .1f, name = "belonging" },
		new Need{color = new Color(0.0f,1.0f,1.0f,0.5f), value = .5f, lossPerSecond = 0.004f, gainPerClick = .1f, name = "esteem" },
		new Need{color = new Color(0.0f,0.5f,1.0f,0.5f), value = .5f, lossPerSecond = 0.002f, gainPerClick = .1f, name = "actualization" },
 };

 public void InitNeeds()
 {
     needs = new Need[] {
		new Need{color = new Color(1.0f,0.0f,0.0f,0.5f), value = .5f, lossPerSecond = 0.010f, gainPerClick = .1f, name = "physiology" },
		new Need{color = new Color(1.0f,0.7f,0.0f,0.5f), value = .5f, lossPerSecond = 0.008f, gainPerClick = .1f, name = "safety" },
		new Need{color = new Color(0.5f,1.0f,0.0f,0.5f), value = .5f, lossPerSecond = 0.006f, gainPerClick = .1f, name = "belonging" },
		new Need{color = new Color(0.0f,1.0f,1.0f,0.5f), value = .5f, lossPerSecond = 0.004f, gainPerClick = .1f, name = "esteem" },
		new Need{color = new Color(0.0f,0.5f,1.0f,0.5f), value = .5f, lossPerSecond = 0.002f, gainPerClick = .1f, name = "actualization" },
 };
 }

    //public float food = 0;
    /// <summary>
    /// Variables changed, redraw text and set back to false till it changes again
    /// </summary>
    bool dirty = true;

    GameObject player;
    float kTransparencyRange = 5;
    bool votingPositive = false;

 private void Awake() {
    {
        InitNeeds();
    }
}
    void Start() {
        player = GameObject.Find( "player" );
		if(player == null) { throw new System.Exception("NEED SOMETHING NAMED \"player\""); }
		if (player.tag != "Player") { throw new System.Exception("\"player\" needs to be tagged \"Player\""); }
		UnityEngine.Assertions.Assert.IsNotNull( txtStatus );
        if ( tag == "Player" ) {
            // No change since last generation
        }
        else if ( tag == "Judge" ) {
            // Judges start negative
            happy = -10;
            safety = -10;
        }
        else {
            happy = UnityEngine.Random.Range( -7.0f, 7.0f );
            safety = UnityEngine.Random.Range( -7.0f, 7.0f );
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
        
        // Update needs simulations
		System.Array.ForEach(needs, (need) => need.Update(Time.deltaTime));

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

            if ( health >= 5f ) {
                foodColor = "green";
                netResult += 1;
            }

            if ( health <= -5f ) {
                foodColor = "red";
                netResult -= 1;
            }

            if ( netResult > 0 ) {
                votingPositive = true;
                //innerGlow.material = matGreen;
            }
            else if ( netResult < 0 ) {
                votingPositive = false;
                //innerGlow.material = matRed;
            }
            else {
                votingPositive = false;
                //innerGlow.material = matWhite;
            }

            string happyNumber = happy.ToString() + "         ";
            string safetyNumber = safety.ToString()+ "         ";
            string foodNumber = health.ToString()+ "         ";

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
        if ( tag == "Judge" && other.tag == "House" ) {
            // Judges don't influence others, only the city hall
            var npc = GetComponent<NPC>();
            UnityEngine.Assertions.Assert.IsNotNull( npc );
            npc.Vote( votingPositive );
            return;
        }
        else if ( other.tag == "Villager" || other.tag == "PlayerBubble" ) {
            if ( !peopleThisPersonInfluenced.Contains( other.transform.parent ) ) {
                peopleThisPersonInfluenced.Add( other.transform.parent );
                Debug.Log( gameObject.name + "Influenced " + other.gameObject.name );

                MaslowMeter otherMeter = other.gameObject.transform.parent.GetComponent<MaslowMeter>();
                UnityEngine.Assertions.Assert.IsNotNull( otherMeter );
                otherMeter.Influence( happy, safety, health );
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
            health += pfood / Math.Abs( pfood );
            health = Mathf.Clamp( health, -10, 10 );
        }

        dirty = true;
    }
}
