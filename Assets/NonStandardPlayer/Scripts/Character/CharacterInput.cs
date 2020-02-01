using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInput : MonoBehaviour
{
	public enum KeyState { onPress, onRelease, onHold };

	[System.Serializable]
	public struct InputEntry {
		public string name;
		public KeyCode[] keys;
		public System.Action<float> action;
	}
	public InputEntry[] inputs;

	public CharacterMove characterMove;

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
