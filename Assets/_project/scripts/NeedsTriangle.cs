using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NeedsTriangle : MonoBehaviour
{
	public NeedLevel[] triangleLayers;

	[System.Serializable]
	public class Need {
		public Color color;
		public string name;
		public float value;
		public float lossPerSecond;
	}

	public Need[] needs = new Need[] {
		new Need{color = Color.red, name = "pizza", value = .5f, lossPerSecond = 0.02f },
		new Need{color = Color.red, name = "job", value = .5f, lossPerSecond = 0.04f },
		new Need{color = Color.red, name = "friends", value = .5f, lossPerSecond = 0.06f },
		new Need{color = Color.red, name = "feelin' swell", value = .5f, lossPerSecond = 0.08f },
		new Need{color = Color.red, name = "encouraging people to vote for reasonable problem solvers", value = .5f, lossPerSecond = 0.1f },
	};

//	public GameObject

	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
