using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField] bool autoHideUI;
    [SerializeField] TMPro.TextMeshProUGUI txtStatus;
    [SerializeField] TMPro.TextMeshProUGUI[] votes;
    [SerializeField] TMPro.TextMeshProUGUI intermissionText;
    [SerializeField] UnityEngine.UI.Button btnContinue;
    [SerializeField] UnityEngine.UI.Button hideUI;
    [SerializeField] Transform NPCs;
    [SerializeField] Transform Judges;
    [SerializeField] Transform player;
    [SerializeField] Water water;
    int roundnumber = 0;
    int kMaxRounds = 4;
    float kNumSecondsPerRound = 60;
    float numSecondsRemaining = 999;

    string startedText = "To repair the planet, you must get politicians to vote positively on climate change. However, they must be first interally repaired before they can be inspired to vote positively...\n\nFufill their needs (increase the numbers above head) to make them vote positive. If a positive or negative person bumps another, the other person will be slightly affected.  Talk to people by bumping into them. Get inspired (three positive numbers), then inspire the politians. \n\nIf you cannot enact change within X generations, then  society will be lost forever underwater.";

    public void ForceGenerationToEnd() {
        bool won = VotesPositive();
        numSecondsRemaining = kNumSecondsPerRound;
        intermissionText.transform.parent.gameObject.SetActive( true );
        intermissionText.text = "<size=50%>Generation completed!" + "\n\n"
            + ( won ? "Politicians has voted to save society, you win!" : "Politicians have NOT voted to help." )
            + ( ( roundnumber == kMaxRounds || won ) ? "\n\nThe End" : "\n\n" + ( kMaxRounds - roundnumber ) + " generations remain..." );
        if ( water.waterTooHigh() ) {
            intermissionText.text += "Water became too high and everyone drowned =(";
        }
        if ( roundnumber == kMaxRounds || won ) {
            water.Reset();
            roundnumber = 0;
        }
    }

    void OnContiuePressed() {
        if ( roundnumber == 0 && intermissionText.text != startedText ) {
            intermissionText.text = startedText;
            return;
        }

        intermissionText.transform.parent.gameObject.SetActive( false );
        StartRound();

        roundnumber = ( roundnumber == kMaxRounds ) ? 0 : roundnumber + 1;
        water.ChangeGoalHeight( 4 );
    }

    bool VotesPositive() {
        int net = 0;
        foreach ( var txtVote in votes ) {
            net += ( txtVote.text == "+" ? 1 : -1 );
        }
        return net > 0;
    }

    void Start() {
        UnityEngine.Assertions.Assert.IsNotNull( txtStatus );
        roundnumber = 0;
        UnityEngine.Assertions.Assert.IsNotNull( btnContinue );
        btnContinue.onClick.AddListener( OnContiuePressed );
        intermissionText.text = startedText;
    }

    void StartRound() {

        if ( autoHideUI )
            hideUI.GetComponent<HideUI>().Hide();

        numSecondsRemaining = kNumSecondsPerRound;
        foreach ( var txtVote in votes ) {
            txtVote.text = "";
        }

        player.transform.position = player.GetComponent<CharacterMove>().startPosition;
        player.transform.GetComponent<MaslowMeter>().ReEvalInfluences( roundnumber );

        for ( int i = 0; i<NPCs.childCount; i++ ) {
            NPCs.GetChild( i ).gameObject.SetActive( true );
            NPCs.GetChild( i ).transform.position = NPCs.GetChild( i ).GetComponent<CharacterMove>().startPosition;
            NPCs.GetChild( i ).GetComponent<MaslowMeter>().ReEvalInfluences( roundnumber );
        }
        for ( int i = 0; i<Judges.childCount; i++ ) {
            Judges.GetChild( i ).gameObject.SetActive( true );
            Judges.GetChild( i ).transform.position = Judges.GetChild( i ).GetComponent<CharacterMove>().startPosition;
        }
    }

    private void Update() {
        numSecondsRemaining -= Time.deltaTime;
        if ( numSecondsRemaining <= 0 ) {
            ForceGenerationToEnd();
        }
        string secondsLeft =  (numSecondsRemaining.ToString()+"  ").Substring(0,2);
        txtStatus.text = "Generation: " + roundnumber + " of " + kMaxRounds
        +"\nTime until next: " + secondsLeft.Replace( ".", "" );
    }
}
