﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
		new Need{color = new Color(1.0f,0.0f,0.0f,0.5f), value = .5f, lossPerSecond = 0.00010f, gainPerClick = .1f, name = "physiology" },
		new Need{color = new Color(1.0f,0.7f,0.0f,0.5f), value = .5f, lossPerSecond = 0.00008f, gainPerClick = .1f, name = "safety" },
		new Need{color = new Color(0.5f,1.0f,0.0f,0.5f), value = .5f, lossPerSecond = 0.00006f, gainPerClick = .1f, name = "belonging" },
		new Need{color = new Color(0.0f,1.0f,1.0f,0.5f), value = .5f, lossPerSecond = 0.00004f, gainPerClick = .1f, name = "esteem" },
		new Need{color = new Color(0.0f,0.5f,1.0f,0.5f), value = .5f, lossPerSecond = 0.00002f, gainPerClick = .1f, name = "actualization" },
 };

 public void InitNeeds()
 {
     needs = new Need[] {
		new Need{color = new Color(1.0f,0.0f,0.0f,0.5f), value = .5f, lossPerSecond = 0.00010f, gainPerClick = .1f, name = "physiology" },
		new Need{color = new Color(1.0f,0.7f,0.0f,0.5f), value = .5f, lossPerSecond = 0.00008f, gainPerClick = .1f, name = "safety" },
		new Need{color = new Color(0.5f,1.0f,0.0f,0.5f), value = .5f, lossPerSecond = 0.00006f, gainPerClick = .1f, name = "belonging" },
		new Need{color = new Color(0.0f,1.0f,1.0f,0.5f), value = .5f, lossPerSecond = 0.00004f, gainPerClick = .1f, name = "esteem" },
		new Need{color = new Color(0.0f,0.5f,1.0f,0.5f), value = .5f, lossPerSecond = 0.00002f, gainPerClick = .1f, name = "actualization" },
 };
 }

/// <summary>
/// When a Maslow meets another maslow they may start interacting with each other.
/// </summary>
 public MaslowMeter interactingWith = null;
 
 /// <summary>
 /// When a Maslow actualizes their 4 base needs with habits, they pick a top habit as their actualization habit to promote.
 /// </summary>
 public bool influencer = false;
 /// <summary>
 /// The habit the influencer is promoting.
 /// </summary>
 public Habits.Habit InfluencerHabit;

    //public float food = 0;
    /// <summary>
    /// Variables changed, redraw text and set back to false till it changes again
    /// </summary>
    bool dirty = true;

/// <summary>
/// Reference to the player used for non-NPC peoples to calculate distances for hide/showing things
/// </summary>
    GameObject player;
    float kTransparencyRange = 5;
    bool votingPositive = false;

    public CharacterMove characterMove;

    public float distance = 0f;
    private float distanceToUIifyFace = 20f;
    public Transform headScreenUI;
    public Transform headWorldUI;

    /// <summary>
    /// Fades out UI stuff on NPCs far away.
    /// </summary>
    private void SetTransBasedOnPlayerDist() {
        float distToPlayer = Vector3.Distance(transform.position, player.transform.position);
        Color newColor = txtStatus.color;
        newColor.a = kTransparencyRange/distToPlayer;
        if ( tag == "Player" )
        {
            newColor.a = 1;
        }
        txtStatus.color = newColor;
    }

private int blinkRate = 99;
private float lastBlink = 0f;
private float blinkLength = 1f;
private bool blinked = false;
    private void UpdateHapiness()
    {
        if(headScreenUI != null && headWorldUI != null)
        {
            distance = Vector3.Distance(Camera.main.transform.position, transform.position);
            int happyInt = Mathf.Abs((int)happy);
            if (happyInt > 9)
            {
                happyInt = 9;
            }

            // if we just blinked
            if (UnityEngine.Random.RandomRange(1,100)>=blinkRate)
            {
                happyInt -= 1;
                lastBlink = Time.time;
                blinkLength = UnityEngine.Random.RandomRange(0.5f,1f);
                blinked = true;
            }
            // or we're continuing to blink
            else if (blinked && Time.time - lastBlink <= blinkLength)
            {
                happyInt -=1;
            }
            // or we're not blinking or done blinking
            else
            {
                blinked = false;
            }

            if (happyInt < 0)
            {
                happyInt = 0;
            }
            
            if (distance > distanceToUIifyFace  )
            {
                //headScreenUI.gameObject.SetActive(true);
                headScreenUI.GetComponent<CanvasUIElement>().GetUI().gameObject.SetActive(true);
                headScreenUI.GetComponent<CanvasUIElement>().activated = true;

                headScreenUI.GetComponent<CanvasUIElement>().GetUI().transform.GetChild(1).GetComponent<Image>().sprite = MaslowManager.Instance.happies[happyInt];
                headWorldUI.gameObject.SetActive(false);
            }
            else
            {
                headWorldUI.GetComponent<Image>().sprite = MaslowManager.Instance.happies[happyInt];
                headScreenUI.GetComponent<CanvasUIElement>().activated = false;
                headScreenUI.GetComponent<CanvasUIElement>().GetUI().gameObject.SetActive(false);
                headWorldUI.gameObject.SetActive(true);
            }
        }
    }

    #region Generation

    public void GenerateHabits()
    {
        foreach (Need need in needs)
        {
            need.habitPrimary = RandomHabit(need);
        }
    }

    public Habits.Habit RandomHabit(Need need)
    {
        Habits.Habit newHabit= new Habits.Habit();
        return newHabit;
    }
    #endregion


    /// <summary>
    /// Based on input numbers [-10, 10] this person's numbers are influenced to change by the influencer
    /// </summary>
    /// <param name="influencer"></param>
    /// <param name="phappy"></param>
    /// <param name="psafety"></param>
    /// <param name="pfood"></param>
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

    /// <summary>
    /// While meeting, don't move or meet someone else till someone departs the meeting
    /// </summary>
    public bool meeting = false;
    /// <summary>
    /// While departing, don't meet anyone for a short delay
    /// </summary>
    public bool departing = false;
    public float departTime = 0f;
    public float meetTime = 0f;
    private float departLength = 1f;
    private float meetLength = 5f;

    public void Meet(MaslowMeter metMaslow)
    {
        // TODO: Look at whoever you met
        meeting = true;
        departing = false;
        characterMove.move.speed = 0f;
        meetTime = Time.time;
    }

    public void Met(MaslowMeter meeter)
    {
        // TODO: Look at whoever you met
        meeting = true;
        departing = false;
        meetTime = Time.time;
        meetLength = 5f + UnityEngine.Random.Range(0f,5f);
    }

    public void Depart(MaslowMeter metMaslow)
    {
        meeting = false;
        departing = true;
        // TODO: Look away from whoever you're departing
        characterMove.move.speed = 2f;
        metMaslow.Departed(this);
        departTime = Time.time;
    } 
    public void Departed(MaslowMeter departed)
    {
        meeting = false;
        departing = true;
        // TODO: Look away from whoever you're departing
        characterMove.move.speed = 2f;
        departTime = Time.time;
    }

    private void UpdateMeetings()
    {
        if (meeting && Time.time - meetTime > meetLength)
        {
            Depart(interactingWith);
        }
        if (departing && Time.time - departTime > departLength)
        {
            departing = false;
        }
    }
    private void InfluenceMaslow( MaslowMeter influencer, Need.NeedLevelEnum needLevelEnum, float phappy, float psafety, float pfood ) {
        
        // Assign the persons interacting with each other to facilitate animation during exchange
        influencer.interactingWith = this;
        interactingWith = influencer;

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

#region MonoBehaviour Methods
 private void Awake() {
    {
        characterMove = GetComponent<CharacterMove>();

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
    void Update() {
        
        // Update needs simulations
		System.Array.ForEach(needs, (need) => need.Update(Time.deltaTime));
        UpdateHapiness();
        UpdateMeetings();
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
            // If this is someone we haven't exchanged with before, we could meet with them
            if ( !peopleThisPersonInfluenced.Contains( other.transform.parent ) ) {
                if (departing || meeting)
                {
                    // TODO: What happens if you're already meeting or departing, like a "sorry not now" eye roll face?
                    // TODO: To avoid bulldozing, turn around and go opposite way

                }
                else
                {
                    Meet(other.GetComponent<MaslowMeter>());
                }
                peopleThisPersonInfluenced.Add( other.transform.parent );
                Debug.Log( gameObject.name + "Influenced " + other.gameObject.name );

                MaslowMeter otherMeter = other.gameObject.transform.parent.GetComponent<MaslowMeter>();
                UnityEngine.Assertions.Assert.IsNotNull( otherMeter );
                //otherMeter.InfluenceMaslow( this, this.happy, safety, health );
                // TODO: Influence should happen during the meeting instead
            }
            // If not, just depart
            else
            {
                // TODO: Make sure bumping into people you've met recently just results in mutual depart and not bulldozing
                if (other.gameObject.GetComponent<MaslowMeter>()==null)
                {
                    Debug.Log(other.gameObject.name + " has no MaslowMeter");


                    
                }
                Depart(other.gameObject.transform.parent.GetComponent<MaslowMeter>());
            }
        }
    }
    #endregion

    
}
