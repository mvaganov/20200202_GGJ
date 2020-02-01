using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaslowMeter : MonoBehaviour {
    [SerializeField] TMPro.TextMeshProUGUI txtStatus;

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
            // No change since las generation
        }
        else if ( tag == "Judge" ) {
            // Judges start negative
            happy = -10;
            safety = -10;
            food = -10;
        }
        else {
            happy = Random.Range( -10.0f, 10.0f );
            safety = Random.Range( -10.0f, 10.0f );
            food = Random.Range( -10.0f, 10.0f );
        }
    }

    private void SetTransBasedOnPlayerDist() {
        float distToPlayer = Vector3.Distance(transform.position, player.transform.position);
        Color newColor = txtStatus.color;
        newColor.a = kTransparencyRange/distToPlayer;
        txtStatus.color = newColor;
    }

    void Update() {
        SetTransBasedOnPlayerDist();

        if ( dirty && txtStatus.gameObject.activeInHierarchy ) {
            string happyColor = "white";
            string safetyColor = "white";
            string foodColor = "white";

            if ( happy > 5f )
                happyColor = "green";
            if ( happy < -5f )
                happyColor = "red";

            if ( safety > 5f )
                safetyColor = "green";
            if ( safety < -5f )
                safetyColor = "red";

            if ( food > 5f )
                foodColor = "green";
            if ( food < -5f )
                foodColor = "red";

            string happyNumber = happy.ToString().Substring(0,4);
            string safetyNumber = safety.ToString().Substring(0,4);
            string foodNumber = food.ToString().Substring(0,4);

            happyNumber = happyNumber.Contains( "-" ) ? happyNumber : "+"+happyNumber;
            safetyNumber = safetyNumber.Contains( "-" ) ? safetyNumber : "+"+safetyNumber;
            foodNumber = foodNumber.Contains( "-" ) ? foodNumber : "+"+foodNumber;

            happyNumber = happyNumber.Substring( 0, 2 );
            safetyNumber = safetyNumber.Substring( 0, 2 );
            foodNumber = foodNumber.Substring( 0, 2 );

            txtStatus.text = "";
            txtStatus.text +=    "<sprite=14> " + "<color=\""+happyColor+"\">" + happyNumber;
            txtStatus.text +=  "\n<sprite=2> "  + "<color=\""+safetyColor+"\">" + safetyNumber;
            txtStatus.text +=  "\n<sprite=11> " + "<color=\""+foodColor+"\">" + foodNumber;

            dirty = false;
        }
    }
}
