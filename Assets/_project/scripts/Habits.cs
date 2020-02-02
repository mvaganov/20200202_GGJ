using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Habits : MonoBehaviour
{
	public enum Layer { lowest_phys, low_safe, mid_belong, high_esteem, highest_actualization, happy }

/// <summary>
/// The habitual activity a person can do to meet some needs. Habits have a primary need level it effects positively and some secondary effected needs that can be positive or negative.
/// </summary>
	[System.Serializable]
	public class Habit {
		public string name;
		public int id;
		public Sprite sprite;
		public Need.NeedLevelEnum needLevel = Need.NeedLevelEnum.Physiology;
		public Effect[] effects;
		[System.Serializable]
		public struct Effect { public Layer layer; public float increase; public float timeToLose; }
		/// <summary>
		/// The current strength of a habit when a person has it. 0-10.
		/// </summary>
		public float strength = 0f;
	}

	public Habit[] habits = new Habit[] {
		new Habit{ name="programming", id=0, effects=new Habit.Effect[]{
			new Habit.Effect{layer=Layer.mid_belong, increase=.01f, timeToLose=5},
			new Habit.Effect{layer=Layer.high_esteem, increase=.01f, timeToLose=5},
			new Habit.Effect{layer=Layer.highest_actualization, increase=.1f, timeToLose=15}
		}},new Habit{ name="candy", id=7, effects=new Habit.Effect[]{
			new Habit.Effect{layer=Layer.lowest_phys, increase=.1f, timeToLose=1},
			new Habit.Effect{layer=Layer.happy, increase=.1f, timeToLose=1}
		}},new Habit{ name="running", id=9, effects=new Habit.Effect[]{
			new Habit.Effect{layer=Layer.lowest_phys, increase=.1f, timeToLose=5}
		}},new Habit{ name="weight lifting", id=10, effects=new Habit.Effect[]{
			new Habit.Effect{layer=Layer.lowest_phys, increase=.1f, timeToLose=5}
		}},new Habit{ name="government work", id=11, effects=new Habit.Effect[]{
			new Habit.Effect{layer=Layer.high_esteem, increase=.01f, timeToLose=5},
			new Habit.Effect{layer=Layer.low_safe, increase=.1f, timeToLose=5},
			new Habit.Effect{layer=Layer.highest_actualization, increase=.1f, timeToLose=1},
		}},new Habit{ name="eat bugs", id=12, effects=new Habit.Effect[]{
			new Habit.Effect{layer=Layer.lowest_phys, increase=.05f, timeToLose=5}
		}},new Habit{ name="rock music", id=15, effects=new Habit.Effect[]{
			new Habit.Effect{layer=Layer.mid_belong, increase=.1f, timeToLose=5},
			new Habit.Effect{layer=Layer.happy, increase=.05f, timeToLose=5}
		}},new Habit{ name="science", id=16, effects=new Habit.Effect[]{
			new Habit.Effect{layer=Layer.lowest_phys, increase=.1f, timeToLose=5},
		}},new Habit{ name="wheelchair", id=17, effects=new Habit.Effect[]{ new Habit.Effect{layer=Layer.lowest_phys, increase=.1f, timeToLose=5} } },
		new Habit{ name="painting", id=18, effects=new Habit.Effect[]{ new Habit.Effect{layer=Layer.lowest_phys, increase=.1f, timeToLose=5} } },
		new Habit{ name="firefighting", id=19, effects=new Habit.Effect[]{ new Habit.Effect{layer=Layer.lowest_phys, increase=.1f, timeToLose=5} } },
		new Habit{ name="police work", id=20, effects=new Habit.Effect[]{ new Habit.Effect{layer=Layer.lowest_phys, increase=.1f, timeToLose=5} } },
		new Habit{ name="fellowship", id=21, effects=new Habit.Effect[]{ new Habit.Effect{layer=Layer.lowest_phys, increase=.1f, timeToLose=5} } },
		new Habit{ name="dance troupe", id=22, effects=new Habit.Effect[]{ new Habit.Effect{layer=Layer.lowest_phys, increase=.1f, timeToLose=5} } },
		new Habit{ name="haircut", id=25, effects=new Habit.Effect[]{ new Habit.Effect{layer=Layer.lowest_phys, increase=.1f, timeToLose=5} } },
		new Habit{ name="flex", id=26, effects=new Habit.Effect[]{ new Habit.Effect{layer=Layer.lowest_phys, increase=.1f, timeToLose=5} } },
		new Habit{ name="megaphone", id=27, effects=new Habit.Effect[]{ new Habit.Effect{layer=Layer.lowest_phys, increase=.1f, timeToLose=5} } },
		new Habit{ name="be the top", id=28, effects=new Habit.Effect[]{ new Habit.Effect{layer=Layer.lowest_phys, increase=.1f, timeToLose=5} } },
		new Habit{ name="give em the finger", id=30, effects=new Habit.Effect[]{ new Habit.Effect{layer=Layer.lowest_phys, increase=.1f, timeToLose=5} } },
		new Habit{ name="feel like a victim", id=31, effects=new Habit.Effect[]{ new Habit.Effect{layer=Layer.lowest_phys, increase=.1f, timeToLose=5} } },
		new Habit{ name="fly a helicopter", id=34, effects=new Habit.Effect[]{ new Habit.Effect{layer=Layer.lowest_phys, increase=.1f, timeToLose=5} } },
		new Habit{ name="red flag", id=35, effects=new Habit.Effect[]{ new Habit.Effect{layer=Layer.lowest_phys, increase=.1f, timeToLose=5} } },
		new Habit{ name="go for a walk", id=36, effects=new Habit.Effect[]{ new Habit.Effect{layer=Layer.lowest_phys, increase=.1f, timeToLose=5} } },
		new Habit{ name="sci fi", id=37, effects=new Habit.Effect[]{ new Habit.Effect{layer=Layer.lowest_phys, increase=.1f, timeToLose=5} } },
		new Habit{ name="fight club", id=38, effects=new Habit.Effect[]{ new Habit.Effect{layer=Layer.lowest_phys, increase=.1f, timeToLose=5} } },
		new Habit{ name="take care of babies", id=39, effects=new Habit.Effect[]{ new Habit.Effect{layer=Layer.lowest_phys, increase=.1f, timeToLose=5} } },
	};
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
