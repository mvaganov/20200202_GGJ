using System;
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

	private float originalSpeed = 10;

/// <summary>
/// The view UI the maslowmeter is connected to when a UI is visible.
/// </summary>
    public NeedsTriangle triangle;
	public RectTransform chestCanvas;

 public Need[] needs = new Need[] {
		new Need{color = new Color(1.0f,0.0f,0.0f,0.5f), value = .5f, lossPerSecond = 0.00010f, gainPerClick = .1f, name = "physiology" },
		new Need{color = new Color(1.0f,0.7f,0.0f,0.5f), value = .5f, lossPerSecond = 0.00008f, gainPerClick = .1f, name = "safety"},
		new Need{color = new Color(0.5f,1.0f,0.0f,0.5f), value = .5f, lossPerSecond = 0.00006f, gainPerClick = .1f, name = "belonging" },
		new Need{color = new Color(0.0f,1.0f,1.0f,0.5f), value = .5f, lossPerSecond = 0.00004f, gainPerClick = .1f, name = "esteem" },
		new Need{color = new Color(0.0f,0.5f,1.0f,0.5f), value = .5f, lossPerSecond = 0.00002f, gainPerClick = .1f, name = "actualization" },
 };

 public void InitNeeds()
 {
     needs = new Need[] {
		new Need{color = new Color(1.0f,0.0f,0.0f,0.5f), value = .5f, lossPerSecond = 0.00010f, gainPerClick = .1f, name = "physiology", maslow=this, layer = Habits.Layer.physiology  },
		new Need{color = new Color(1.0f,0.7f,0.0f,0.5f), value = .5f, lossPerSecond = 0.00008f, gainPerClick = .1f, name = "safety", maslow=this, layer = Habits.Layer.safety },
		new Need{color = new Color(0.5f,1.0f,0.0f,0.5f), value = .5f, lossPerSecond = 0.00006f, gainPerClick = .1f, name = "belonging", maslow=this, layer = Habits.Layer.belonging  },
		new Need{color = new Color(0.0f,1.0f,1.0f,0.5f), value = .5f, lossPerSecond = 0.00004f, gainPerClick = .1f, name = "esteem", maslow=this, layer = Habits.Layer.esteem  },
		new Need{color = new Color(0.0f,0.5f,1.0f,0.5f), value = .5f, lossPerSecond = 0.00002f, gainPerClick = .1f, name = "actualization", maslow=this, layer = Habits.Layer.actualization  },
 };
    if (!isPlayer)
    {
        GenerateHabits();
    }
 }

/// <summary>
/// When a Maslow meets another maslow they may start interacting with each other.
/// </summary>
 public MaslowMeter interactingWith = null;
	public GameObject meeting_line;
 /// <summary>
 /// When a Maslow actualizes their 4 base needs with habits, they pick a top habit as their actualization habit to promote.
 /// </summary>
 public bool influencer = false;
 /// <summary>
 /// The habit the influencer is promoting.
 /// </summary>
 public Habits.Habit influencerHabit;

    //public float food = 0;
    /// <summary>
    /// Variables changed, redraw text and set back to false till it changes again
    /// </summary>
    bool dirty = true;

/// <summary>
/// Reference to the player used for non-NPC peoples to calculate distances for hide/showing things
/// </summary>
    GameObject player;
    public bool isPlayer = false;
    
    float kTransparencyRange = 5;
	public enum MyVote { abstain, present, yea, nay }
    public MyVote vote = MyVote.abstain;

    public CharacterMove characterMove;

    public float distance = 0f;
    private float distanceToUIifyFace = 30f;
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

    /// <summary>
    /// The highest layer you've reached, starting at -1 when you have no layers. Indicates which layer you need next or can give someone.
    /// </summary>
    public int highestLayer =  -1; 
    public Happiness happinessBar;
    private static float influencerHappyPerSecond = 0.05f;
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
            
            float happyChange = 0f;
            if (needs[0].habitPrimary != null) {
                happyChange += influencerHappyPerSecond;
            }
            if (needs[1].habitPrimary != null) {
                happyChange += influencerHappyPerSecond;
            }
            if (needs[2].habitPrimary != null) {
                happyChange += influencerHappyPerSecond;
            }
            if (needs[3].habitPrimary != null) {
                happyChange += influencerHappyPerSecond;
            }
            if (influencer) happy += Time.deltaTime * happyChange;
            if (happy > maxNeedValue)
            {
                happy = maxNeedValue;
            }
            if (happy <= minNeedValue)
            {
                happy = minNeedValue;
            }

            if (happinessBar == null)
            {
                happinessBar = GetComponentInChildren<Happiness>();
            }
            if(happy>0f)
            {
                happinessBar.happinessPosition = happy / maxNeedValue;
            }
            else
            {
                happinessBar.happinessPosition = 0f;

            }
            
            // Show world happy face close or screen happy face from afar
            if (distance > distanceToUIifyFace  )
            {
                //headScreenUI.gameObject.SetActive(true);
                headScreenUI.GetComponent<CanvasUIElement>().GetUI().gameObject.SetActive(true);
                headScreenUI.GetComponent<CanvasUIElement>().activated = true;
				//headWorldUI.GetComponent<CanvasUIElement>().GetUI().GetComponent<Image>().enabled = true;

				RectTransform rt = headScreenUI.GetComponent<CanvasUIElement>().GetUI();
				Transform t = rt.transform.GetChild(1);
				t.gameObject.SetActive(true);
				t.GetComponent<Image>().sprite = MaslowManager.Instance.happies[happyInt];

				//headScreenUI.GetComponent<CanvasUIElement>().GetUI().transform.GetChild(1).GetComponent<Image>().sprite = MaslowManager.Instance.happies[happyInt];
				headWorldUI.gameObject.SetActive(false);
            }
            else
            {
                headWorldUI.GetComponent<Image>().sprite = MaslowManager.Instance.happies[happyInt];
				//headScreenUI.GetComponent<CanvasUIElement>().activated = false;
				//headScreenUI.GetComponent<CanvasUIElement>().GetUI().gameObject.SetActive(false);
				RectTransform rt = headScreenUI.GetComponent<CanvasUIElement>().GetUI();
				Transform t = rt.transform.GetChild(1);
				t.gameObject.SetActive(false);

				headWorldUI.gameObject.SetActive(true);
			}
		}
    }

    #region Generation

    public static float maxHabitValue = 100f;
    public void GenerateHabits()
    {
        if (tag != "Judge")
        {
            if (UnityEngine.Random.Range(0,100) < 80)
            {
                needs[(int)Habits.Layer.physiology].habitPrimary = RandomHabit(Habits.Layer.physiology);
                needs[(int)Habits.Layer.physiology].habitPrimaryValue = maxHabitValue;
                highestLayer = 0;
                //Debug.Log("physiology habit is " + needs[(int)Habits.Layer.physiology].habitPrimary.name);

                if (UnityEngine.Random.Range(0,100) < 60)
                {
                    highestLayer = 1;
                    needs[(int)Habits.Layer.safety].habitPrimary = RandomHabit(Habits.Layer.safety);
                    needs[(int)Habits.Layer.safety].habitPrimaryValue = maxHabitValue;
                    //Debug.Log("safety habit is " + needs[(int)Habits.Layer.safety].habitPrimary.name);


                    if (UnityEngine.Random.Range(0,100) < 50)
                    {
                        highestLayer = 2;
                        needs[(int)Habits.Layer.belonging].habitPrimary = RandomHabit(Habits.Layer.belonging);
                        needs[(int)Habits.Layer.belonging].habitPrimaryValue = maxHabitValue;

                        if (UnityEngine.Random.Range(0,100) < 30)
                        {
                            highestLayer = 3;
                            needs[(int)Habits.Layer.esteem].habitPrimary = RandomHabit(Habits.Layer.esteem);
                            needs[(int)Habits.Layer.esteem].habitPrimaryValue = maxHabitValue;

                            highestLayer = 4;
                            influencer = true;
                            influencerHabit = RandomNeedPrimaryHabit();
                            needs[(int)Habits.Layer.actualization].habitPrimary = influencerHabit;
                        }
                    }
                }
            }
        }
        else
        {
            highestLayer=-1;
        }
        
        
        
        
        /*
        foreach (Need need in needs)
        {
            need.habitPrimary = RandomHabit(need);
        }
        */
    }

    // By default, return only the first 4 layers.
    public Habits.Layer RandomLayer()
    {
        return RandomLayer(3);
    }

    public Habits.Layer RandomLayer(int maxLayer)
    {
        int randomIndex = UnityEngine.Random.Range(0,maxLayer);
        if (randomIndex <= 6)
        {
            return (Habits.Layer)randomIndex;
        }
        else
        {
            return (Habits.Layer)0;
        }
    }
    public Habits.Habit RandomHabit(Habits.Layer layer)
    {
        Habits.Habit newHabit= new Habits.Habit();
        List<Habits.Habit> matchingHabits = new List<Habits.Habit>();
        foreach ( Habits.Habit eachHabit in Habits.habits)
        {
            // If this habit matches the randomly picked habit
            if (eachHabit.layer == layer)
            {
                //Debug.Log("randomizing list includes " + eachHabit.name);
                matchingHabits.Add(eachHabit);
            }
        }
        if(matchingHabits.Count <= 0)
        {
            Debug.LogWarning("No matching random habits found for that layer " + layer.ToString() + ".");
        }
        newHabit = matchingHabits[UnityEngine.Random.Range(0,matchingHabits.Count-1)];
        
        return newHabit;
    }

/// <summary>
/// Returns one of the person's primary habits.
/// </summary>
/// <returns></returns>
    public Habits.Habit RandomNeedPrimaryHabit()
    {
        Habits.Habit newHabit= new Habits.Habit();
        List<Habits.Habit> matchingHabits = new List<Habits.Habit>();
        foreach ( Need eachNeed in needs)
        {
            matchingHabits.Add(eachNeed.habitPrimary);
        }
        if(matchingHabits.Count <= 0)
        {
            Debug.LogWarning("No matching random habits found for needs.");
        }
        newHabit = matchingHabits[UnityEngine.Random.Range(0,matchingHabits.Count-1)];
        
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
	private const float showTriangleLength = 5f;
	private float showTriangleDuration;

    public void RestoreInteractingWithColor()
    {
        if (interactingWith != null)
        {
            if(interactingWith.highestLayer>-1)
            {
            interactingWith.needs[interactingWith.highestLayer].ui.background.color = interactingWith.needs[interactingWith.highestLayer].color;
            }
        }
    }
    public void Meet(MaslowMeter metMaslow)
    {
        // TODO: Look at whoever you met
        meeting = true;
		departing = false;
//        RestoreInteractingWithColor();
		//interactingWith = metMaslow;
		//if (metMaslow.tag == "Player")
		//{
		//	//Debug.LogError(name + " met Player "+triangle.triangleShow+" "+triangle.transform.parent.parent.parent);
		//}
//		characterMove.move.speed = 0.05f;
		metMaslow.Met(this);
		meetTime = Time.time;
        meetLength = 5f + UnityEngine.Random.Range(0f,5f);
    }

    public void Met(MaslowMeter meeter)
    {
        // TODO: Look at whoever you met
        meeting = true;
		departing = false;
 //       characterMove.move.speed = 0.05f;
//        RestoreInteractingWithColor();
		//interactingWith = meeter;
		//if (meeter.tag == "Player")
		//{
		//	triangle.SetShow(NeedsTriangle.Show.aboveHead);
		//}
		meetTime = Time.time;
	}

	public void Depart(MaslowMeter metMaslow)
    {
        meeting = false;
        departing = true;
		// TODO: Look away from whoever you're departing
		characterMove.move.speed = originalSpeed;
        metMaslow.Departed(this);
        departTime = Time.time;
    } 
    public void Departed(MaslowMeter departed)
    {
        meeting = false;
        departing = true;
        // TODO: Look away from whoever you're departing
        characterMove.move.speed = originalSpeed;
        departTime = Time.time;
    }
public float goodBlinkSpeed = 0.1f;
public float goodBlinkLast = 0f;
public bool goodBlinking = false;

    private Color blinkColorOriginal;
    private Color blinkColorGood = Color.green;
    private Color blinkColorBad = Color.red;
    public int blinkingLayer = 0;
    private void UpdateMeetings()
    {
        if (interactingWith != null && highestLayer > interactingWith.highestLayer  && (isPlayer || interactingWith.isPlayer))
        {
            blinkingLayer = interactingWith.highestLayer+1;
            goodBlinking = true;
            if (Time.time - goodBlinkLast > goodBlinkSpeed)
            {
                goodBlinkLast = Time.time;
                if (needs[blinkingLayer].ui.background.color == needs[highestLayer].color)
                {
                    needs[blinkingLayer].ui.background.color = blinkColorGood;
                }
                else
                {
                    needs[highestLayer].ui.background.color = needs[highestLayer].color;
                }
            }
        }
        else
        {
            goodBlinking = false;
        }

		if (interactingWith != null) {
			Vector3 delta = interactingWith.transform.position - transform.position;
			Vector3 midpoint = transform.position + delta / 2;
			if (interactingWith.interactingWith != this) {
				//NS.Lines.MakeArrow(ref meeting_line, transform.position, midpoint, Color.red);
				if (meeting_line != null) { meeting_line.SetActive(false); }
                        RestoreInteractingWithColor();

				interactingWith = null;
				meeting = false;
			} else {
				NS.Lines.MakeArrow(ref meeting_line, transform.position, midpoint, Color.black);
				meeting_line.SetActive(true);
				showTriangleDuration = showTriangleLength;
			}
		}
		if (meeting && Time.time - meetTime > meetLength)
        {
            Depart(interactingWith);
        }
        if (departing && Time.time - departTime > departLength)
        {
            departing = false;
        }
    }
	private GameObject faceToTriangle_line;
	private void UpdateTriangleUI()
	{
		if (triangle.triangleShow == NeedsTriangle.Show.aboveHead) {
			RectTransform rt = headScreenUI.GetComponent<CanvasUIElement>().GetUI();
			Transform faceTransform = rt.transform.GetChild(1);
			LineUI.Create(ref faceToTriangle_line, triangle.transform, faceTransform, new Vector2(0, -100), Vector2.zero, new Color(1,1,1,.5f), 10);
			faceToTriangle_line.transform.SetParent(triangle.transform.parent);
			faceToTriangle_line.transform.localScale = Vector3.one;
			faceToTriangle_line.SetActive(true);
		} else if(faceToTriangle_line != null) { faceToTriangle_line.SetActive(false); }
		if (showTriangleDuration > 0) {
			showTriangleDuration -= Time.deltaTime;
			if (showTriangleDuration <= 0) {
				if (transform.tag != "Player")
				{ // player UI is always in the player UI area. never go back to chest.
					//Debug.Log(name+" TRIANGLE OFF!");
					triangle.SetShow(NeedsTriangle.Show.inChest);
				}
			}
		}
	}

    private static float secondaryInfluenceAmount = 25f;
    private static float minHabitValue = 0f;

    public void UpdateHighest()
    {
        /*
        if(needs[0].habitPrimary == null || needs[0].habitPrimary.name != "") { highestLayer = -1;}
        if(needs[1].habitPrimary != null || needs[1].habitPrimary.name != "") { highestLayer = 0;}
        if(needs[2].habitPrimary != null || needs[2].habitPrimary.name != "") { highestLayer = 1;}
        if(needs[3].habitPrimary != null || needs[3].habitPrimary.name != "") { highestLayer = 2;}
        if(needs[4].habitPrimary != null || needs[4].habitPrimary.name != "") { highestLayer = 3;}
        //if(needs[5].habitPrimary != null) { highestLayer = 4;}
*/
    }

    public void InfluenceMaslow( MaslowMeter influencer, Habits.Layer influenceLayer, float phappy, float psafety, float pfood ) {

        if (phappy > 0)
        {
            Noisy.PlaySound("Mood Increased");
        }
        else if (phappy < 0)
        {
            Noisy.PlaySound("Mood Decreased");
        }

        // Assign the persons interacting with each other to facilitate animation during exchange
        influencer.RestoreInteractingWithColor();

        influencer.interactingWith = this;
        if(interactingWith!=null)
        {interactingWith.RestoreInteractingWithColor();}

        interactingWith = influencer;

        Need receivedNeed = needs[(int)influenceLayer];
        Need influencerNeed =  influencer.needs[(int)influenceLayer];
        if ( (int)influencerNeed.layer <= highestLayer + 1 ) 
        {
            if(influencerNeed.habitPrimary != null)
            {
                // If we have no primary, get us started at max
                if (receivedNeed.habitPrimary == null)
                {
                    //Debug.Log("isPlayer:"+isPlayer);
                    //Debug.Log("highestLayer:"  + highestLayer);
                    highestLayer ++;
                    happy+=2f;
                    interactingWith.happy +=2f;
                    //Debug.Log("highestLayer:"  + highestLayer);

                    // TODO: Make a way to share someone's secondary needs with yourself, otehrwise just always ask for their primary.
                    receivedNeed.habitPrimary = influencerNeed.habitPrimary;
                    receivedNeed.habitPrimaryValue = MaslowMeter.maxHabitValue;
                    if (highestLayer + 1 == (int) receivedNeed.layer)
                    {
                        //highestLayer ++;
                    }
                }
                // otherwise check if its the primary we already have being boosted to max again
                else if (receivedNeed.habitPrimary.name != influencerNeed.habitPrimary.name)
                {
                    receivedNeed.habitSecondary = influencerNeed.habitPrimary;
                    receivedNeed.habitPrimaryValue = MaslowMeter.maxHabitValue;

                }
                // or its our first secondary
                else if (receivedNeed.habitSecondary == null)
                {
                    receivedNeed.habitSecondary = influencerNeed.habitPrimary;
                    receivedNeed.habitPrimaryValue -= secondaryInfluenceAmount;
                    receivedNeed.habitSecondaryValue += secondaryInfluenceAmount;
                    
                }
                // or a secondary we do have
                else if (receivedNeed.habitSecondary.name == influencerNeed.name)
                {
                    receivedNeed.habitSecondary = influencerNeed.habitPrimary;
                    receivedNeed.habitPrimaryValue = MaslowMeter.maxHabitValue;
                }
                // or a secondary different from ours replaces our secondary
                else if (receivedNeed.habitSecondary.name != influencerNeed.habitPrimary.name)
                {
                    receivedNeed.habitSecondary = influencerNeed.habitPrimary;
                    receivedNeed.habitPrimaryValue -= secondaryInfluenceAmount;
                    receivedNeed.habitSecondaryValue += secondaryInfluenceAmount;
                    
                }
                // or nothing is there to gain
                else
                {
                    UnityEngine.Debug.Log("Nothing to gain.");
                }
            }
            receivedNeed.Use(receivedNeed.ui,receivedNeed);

            // If our secondary beat our primary, bin the old and in with the new
            if(secondaryInfluenceAmount >= maxHabitValue)
            {
                receivedNeed.habitOld = receivedNeed.habitPrimary;
                receivedNeed.habitPrimary = receivedNeed.habitSecondary;
                receivedNeed.habitPrimaryValue = maxHabitValue;
                receivedNeed.habitSecondaryValue = minHabitValue;
            }
            
            // else 
            else
            {

            }

            /*
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
            }*/
        } // end if not higher than next higher layer
        dirty = true;
    }

#region MonoBehaviour Methods
 private void Awake() {
    {
        if(gameObject.tag == "Player")
        {
            isPlayer = true;
        }
        characterMove = GetComponent<CharacterMove>();

        Habits.InitSprites();
        InitNeeds();
			if(chestCanvas == null) {
				Debug.LogWarning("Missing chest canvas!");
			}
    }
}
    void Start() {
		originalSpeed = characterMove.move.speed;
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
            happy = UnityEngine.Random.Range( minNeedValue, maxNeedRandomValue);
            safety = UnityEngine.Random.Range( minNeedValue, maxNeedRandomValue);
        }
	}

    public static float  minNeedValue = 0f;
    public static float maxNeedValue = 10f;
    public static float maxNeedRandomValue = 7f;

	MyVote VoteForEarth() {
		return (happy >= 5 && actualization >= 5)?MyVote.yea:MyVote.nay;
	}

    void FixedUpdate() {
        
        // Update needs simulations
		System.Array.ForEach(needs, (need) => need.Update(Time.deltaTime));
        UpdateHapiness();
        UpdateMeetings();
        UpdateHighest();
		UpdateTriangleUI();
		//SetTransBasedOnPlayerDist();

        //if ( dirty && txtStatus.gameObject.activeInHierarchy ) {
        //    string happyColor = "white";
        //    string safetyColor = "white";
        //    string foodColor = "white";

        //    int netResult = 0;

        //    if ( happy >= 5f ) {
        //        happyColor = "green";
        //        netResult += 1;
        //    }

        //    if ( happy <= -5f ) {
        //        happyColor = "red";
        //        netResult -= 1;
        //    }

        //    if ( safety >= 5f ) {
        //        safetyColor = "green";
        //        netResult += 1;
        //    }

        //    if ( safety <= -5f ) {
        //        safetyColor = "red";
        //        netResult -= 1;
        //    }

        //    if ( health >= 5f ) {
        //        foodColor = "green";
        //        netResult += 1;
        //    }

        //    if ( health <= -5f ) {
        //        foodColor = "red";
        //        netResult -= 1;
        //    }

        //    if ( netResult > 0 ) {
        //        votingPositive = true;
        //        //innerGlow.material = matGreen;
        //    }
        //    else if ( netResult < 0 ) {
        //        votingPositive = false;
        //        //innerGlow.material = matRed;
        //    }
        //    else {
        //        votingPositive = false;
        //        //innerGlow.material = matWhite;
        //    }

        //    string happyNumber = happy.ToString() + "         ";
        //    string safetyNumber = safety.ToString()+ "         ";
        //    string foodNumber = health.ToString()+ "         ";

        //    happyNumber = happyNumber.Substring( 0, 4 );
        //    safetyNumber  = safetyNumber.Substring( 0, 4 );
        //    foodNumber = foodNumber.Substring( 0, 4 );

        //    happyNumber = happyNumber.Contains( "-" ) ? happyNumber : "+"+happyNumber;
        //    safetyNumber = safetyNumber.Contains( "-" ) ? safetyNumber : "+"+safetyNumber;
        //    foodNumber = foodNumber.Contains( "-" ) ? foodNumber : "+"+foodNumber;

        //    happyNumber = happyNumber.Substring( 0, 3 ).Replace( ".", " " );
        //    safetyNumber = safetyNumber.Substring( 0, 3 ).Replace( ".", " " );
        //    foodNumber = foodNumber.Substring( 0, 3 ).Replace( ".", " " );

        //    txtStatus.text = "";
        //    txtStatus.text +=    "<sprite=14> " + "<color=\""+happyColor+"\">" + happyNumber;
        //    txtStatus.text +=  "\n<sprite=2> "  + "<color=\""+safetyColor+"\">" + safetyNumber;
        //    txtStatus.text +=  "\n<sprite=11> " + "<color=\""+foodColor+"\">" + foodNumber;

        //    dirty = false;
        //}
    }

	public MaslowMeter GetMaslowMeter(GameObject other) {
		MaslowMeter mm = other.GetComponent<MaslowMeter>();
		if(mm == null) {
			mm = other.transform.parent.GetComponent<MaslowMeter>();
		}
		if (mm == null) {
			Debug.Log("CAN'T FIND MASLOWMETER FOR " + other);
		}
		return mm;
	}

	private static void ActivateTriangleUI(MaslowMeter mm) {
		mm.triangle.SetShow(NeedsTriangle.Show.aboveHead);
		mm.showTriangleDuration = showTriangleLength;
	}

	private void OnTriggerEnter( Collider other ) {
        if ( tag == "Judge" && other.tag == "House" ) {
			vote = VoteForEarth();
			// Judges don't influence others, only the city hall
			var npc = GetComponent<NPCWithBoxes>();
            UnityEngine.Assertions.Assert.IsNotNull( npc );
            npc.Vote( vote );
			//Debug.Log("JUDGED!");
            return;
        }
        else if ( other.tag == "Villager" || other.tag == "Judge" || other.tag == "PlayerBubble" ) {
			MaslowMeter otherMaslow = GetMaslowMeter(other.gameObject);
			if (other.tag == "Player") {
				//showTriangleDuration = showTriangleLength;
				ActivateTriangleUI(this);
			}
			if (tag == "Player" && otherMaslow != null) {
				//otherMaslow.showTriangleDuration = showTriangleLength;
				ActivateTriangleUI(otherMaslow);
			}
			if(otherMaslow != null)
			{
				interactingWith = otherMaslow;
				otherMaslow.interactingWith = this;
				RestoreInteractingWithColor();
			}
			// If this is someone we haven't exchanged with before, we could meet with them
			if ( !peopleThisPersonInfluenced.Contains( other.transform.parent ) ) {
				if (departing || meeting)
                {
                    // TODO: What happens if you're already meeting or departing, like a "sorry not now" eye roll face?
                    // TODO: To avoid bulldozing, turn around and go opposite way
                    Depart(otherMaslow);

                }
                else
                {
					Meet(otherMaslow);
                }
                peopleThisPersonInfluenced.Add( other.transform.parent );
                //Debug.Log( gameObject.name + "Influenced " + other.gameObject.name );

                //MaslowMeter otherMeter = other.gameObject.transform.parent.GetComponent<MaslowMeter>();
                //UnityEngine.Assertions.Assert.IsNotNull( otherMeter );
                //otherMeter.InfluenceMaslow( this, this.happy, safety, health );
                // TODO: Influence should happen during the meeting instead
            }
            // If not, just depart
            else
            {
                
                //Depart(GetMaslowMeter(other.gameObject));
            }
        }
    }
    #endregion

    
}
