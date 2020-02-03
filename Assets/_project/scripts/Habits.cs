using UnityEngine;

public class Habits : MonoBehaviour
{
    public enum Layer
    {
        physiology,
        safety,
        belonging,
        esteem,
        actualization,
        mood,
        earth
    }

    [System.Serializable]
    public class Habit
    {
        public string name;
        public int id;
        public Sprite sprite;

        // Effect modifiers
        public float satisfaction;
        public float decay;

        // Maslow effects
        public float physiology;
        public float safety;
        public float belonging;
        public float esteem;
        public float actualization;

        // Other effects
        public float mood;
        public float earth;

        // Native layer of the habit (though others can be affected)
        public Layer layer;
    }

	
    public static Habit[] habits = {
        new Habit {
            name="plant-based diet",
            id=1833,
            layer=Layer.physiology,
            satisfaction=1,
            decay=1,
            physiology=2,
            safety=-1,
            belonging=0,
            esteem=0,
            actualization=-1,
            mood=-1,
            earth=1
        }, new Habit {
            name="meat-heavy diet",
            id=2470,
            layer=Layer.physiology,
            satisfaction=3,
            decay=3,
            physiology=-1,
            safety=0,
            belonging=0,
            esteem=-1,
            actualization=0,
            mood=1,
            earth=-2
        }, new Habit {
            name="sushi",
            id=119,
            layer=Layer.physiology,
            satisfaction=1,
            decay=2,
            physiology=1,
            safety=0,
            belonging=1,
            esteem=0,
            actualization=1,
            mood=1,
            earth=-1
        }, new Habit {
            name="cupcakes",
            id=548,
            layer=Layer.physiology,
            satisfaction=2,
            decay=3,
            physiology=-2,
            safety=0,
            belonging=0,
            esteem=0,
            actualization=1,
            mood=2,
            earth=0
        }, new Habit {
            name="BACON",
            id=1945,
            layer=Layer.physiology,
            satisfaction=3,
            decay=2,
            physiology=-2,
            safety=0,
            belonging=0,
            esteem=0,
            actualization=2,
            mood=2,
            earth=-2
        }, new Habit {
            name="eat your veggies",
            id=3009,
            layer=Layer.physiology,
            satisfaction=1,
            decay=2,
            physiology=2,
            safety=0,
            belonging=-1,
            esteem=0,
            actualization=-2,
            mood=-2,
            earth=2
        }, new Habit {
            name="takeout",
            id=2729,
            layer=Layer.physiology,
            satisfaction=2,
            decay=3,
            physiology=-1,
            safety=1,
            belonging=0,
            esteem=0,
            actualization=0,
            mood=1,
            earth=-1
        }, new Habit {
            name="home cooking",
            id=1921,
            layer=Layer.physiology,
            satisfaction=3,
            decay=1,
            physiology=1,
            safety=0,
            belonging=0,
            esteem=1,
            actualization=1,
            mood=1,
            earth=1
        }, new Habit {
            name="motorcycle",
            id=1802,
            layer=Layer.safety,
            satisfaction=2,
            decay=3,
            physiology=-1,
            safety=2,
            belonging=1,
            esteem=0,
            actualization=0,
            mood=2,
            earth=-1
        }, new Habit {
            name="bicycle",
            id=539,
            layer=Layer.safety,
            satisfaction=1,
            decay=1,
            physiology=2,
            safety=1,
            belonging=0,
            esteem=0,
            actualization=0,
            mood=-1,
            earth=2
        }, new Habit {
            name="train",
            id=314,
            layer=Layer.safety,
            satisfaction=3,
            decay=1,
            physiology=0,
            safety=2,
            belonging=-1,
            esteem=1,
            actualization=0,
            mood=-1,
            earth=2
        }, new Habit {
            name="car",
            id=1266,
            layer=Layer.safety,
            satisfaction=2,
            decay=2,
            physiology=0,
            safety=2,
            belonging=1,
            esteem=0,
            actualization=0,
            mood=1,
            earth=-2
        }, new Habit {
            name="electric moped",
            id=3060,
            layer=Layer.safety,
            satisfaction=1,
            decay=3,
            physiology=0,
            safety=1,
            belonging=0,
            esteem=0,
            actualization=1,
            mood=2,
            earth=-1
        }, new Habit {
            name="scooter",
            id=3004,
            layer=Layer.safety,
            satisfaction=1,
            decay=2,
            physiology=1,
            safety=0,
            belonging=0,
            esteem=-1,
            actualization=0,
            mood=0,
            earth=1
        }, new Habit {
            name="airplane",
            id=2350,
            layer=Layer.safety,
            satisfaction=3,
            decay=3,
            physiology=-1,
            safety=2,
            belonging=0,
            esteem=0,
            actualization=0,
            mood=-1,
            earth=-2
        }, new Habit {
            name="bus",
            id=650,
            layer=Layer.safety,
            satisfaction=2,
            decay=1,
            physiology=0,
            safety=-1,
            belonging=0,
            esteem=0,
            actualization=-1,
            mood=-2,
            earth=1
        }, new Habit {
            name="going out for drinks",
            id=1463,
            layer=Layer.belonging,
            satisfaction=2,
            decay=2,
            physiology=-2,
            safety=0,
            belonging=2,
            esteem=0,
            actualization=2,
            mood=2,
            earth=1
        }, new Habit {
            name="watching TV",
            id=1315,
            layer=Layer.belonging,
            satisfaction=2,
            decay=3,
            physiology=0,
            safety=-1,
            belonging=1,
            esteem=0,
            actualization=-2,
            mood=0,
            earth=-1
        }, new Habit {
            name="skiing",
            id=1006,
            layer=Layer.belonging,
            satisfaction=3,
            decay=1,
            physiology=-1,
            safety=1,
            belonging=2,
            esteem=1,
            actualization=2,
            mood=2,
            earth=-2
        }, new Habit {
            name="social dancing",
            id=1480,
            layer=Layer.belonging,
            satisfaction=3,
            decay=2,
            physiology=1,
            safety=0,
            belonging=2,
            esteem=1,
            actualization=2,
            mood=2,
            earth=2
        }, new Habit {
            name="volleyball",
            id=1970,
            layer=Layer.belonging,
            satisfaction=1,
            decay=3,
            physiology=2,
            safety=1,
            belonging=1,
            esteem=0,
            actualization=1,
            mood=1,
            earth=1
        }, new Habit {
            name="smoking",
            id=203,
            layer=Layer.belonging,
            satisfaction=1,
            decay=3,
            physiology=-2,
            safety=-1,
            belonging=1,
            esteem=-1,
            actualization=-1,
            mood=1,
            earth=-2
        }, new Habit {
            name="recycling",
            id=2181,
            layer=Layer.belonging,
            satisfaction=3,
            decay=3,
            physiology=0,
            safety=0,
            belonging=1,
            esteem=2,
            actualization=2,
            mood=1,
            earth=2
        }, new Habit {
            name="shopping",
            id=2276,
            layer=Layer.belonging,
            satisfaction=1,
            decay=2,
            physiology=0,
            safety=0,
            belonging=0,
            esteem=1,
            actualization=1,
            mood=1,
            earth=-1
        }, new Habit {
            name="yoga",
            id=1113,
            layer=Layer.esteem,
            satisfaction=2,
            decay=2,
            physiology=2,
            safety=0,
            belonging=0,
            esteem=1,
            actualization=-2,
            mood=2,
            earth=1
        }, new Habit {
            name="cat ownership",
            id=2027,
            layer=Layer.esteem,
            satisfaction=3,
            decay=1,
            physiology=1,
            safety=0,
            belonging=2,
            esteem=2,
            actualization=1,
            mood=2,
            earth=-1
        }, new Habit {
            name="gardening",
            id=510,
            layer=Layer.esteem,
            satisfaction=2,
            decay=1,
            physiology=2,
            safety=0,
            belonging=0,
            esteem=1,
            actualization=0,
            mood=1,
            earth=2
        }, new Habit {
            name="going on a cruise",
            id=2948,
            layer=Layer.esteem,
            satisfaction=2,
            decay=3,
            physiology=-1,
            safety=1,
            belonging=1,
            esteem=2,
            actualization=-1,
            mood=0,
            earth=-2
        }, new Habit {
            name="getting a degree",
            id=3087,
            layer=Layer.esteem,
            satisfaction=3,
            decay=1,
            physiology=-1,
            safety=0,
            belonging=-1,
            esteem=2,
            actualization=2,
            mood=-1,
            earth=0
        }, new Habit {
            name="champagne room",
            id=1631,
            layer=Layer.esteem,
            satisfaction=2,
            decay=3,
            physiology=-1,
            safety=0,
            belonging=2,
            esteem=2,
            actualization=1,
            mood=1,
            earth=-1
        }, new Habit {
            name="bubble baths",
            id=1324,
            layer=Layer.esteem,
            satisfaction=1,
            decay=2,
            physiology=0,
            safety=0,
            belonging=0,
            esteem=1,
            actualization=0,
            mood=1,
            earth=-1
        }, new Habit {
            name="cleaning the house",
            id=2515,
            layer=Layer.esteem,
            satisfaction=1,
            decay=3,
            physiology=0,
            safety=1,
            belonging=-1,
            esteem=1,
            actualization=1,
            mood=-2,
            earth=2
        },

    };

public static void InitSprites()
{
	foreach (Habit habit in habits)
		{
			habit.sprite = MaslowManager.Instance.emojiSprites[habit.id];
		}
}

    // Start is called before the first frame update
    void Awake()
    {
		InitSprites();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
