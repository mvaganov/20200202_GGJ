using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof( UnityEngine.UI.Button ) )]
public class HideUI : MonoBehaviour {
    void Start() {
        UnityEngine.UI.Button btnHideUI = GetComponent<UnityEngine.UI.Button>();
        UnityEngine.Assertions.Assert.IsNotNull( btnHideUI );
        btnHideUI.onClick.AddListener( delegate {
            Hide();
        } );
    }

    public void Hide() {
        Transform baseUIController = transform.parent;
        RecursiveDisableVisuals( baseUIController, baseUIController );
    }

    private void RecursiveDisableVisuals( Transform root, Transform self ) {
        for ( int i = 0; i<root.childCount; i++ ) {
            Transform curChild = root.GetChild(i);
            if ( curChild == self ) continue; // don't disable the button itself...
            UnityEngine.UI.Image curChildVisual = curChild.GetComponent<UnityEngine.UI.Image>();
            if ( curChildVisual != null ) {
                curChildVisual.enabled = false;
            }

            UnityEngine.UI.Text curChildText = curChild.GetComponent<UnityEngine.UI.Text>();
            if ( curChildText != null ) {
                curChildText.enabled = false;
            }

            if ( curChild.childCount > 0 ) {
                RecursiveDisableVisuals( curChild, self );
            }
        }
    }
}
