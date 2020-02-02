﻿using UnityEngine;

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
        new Habit{
        name="plant-based diet",
        id=2328,
        satisfaction=1,
        decay=1,
        physiology=2,
        safety=0,
        belonging=0,
        esteem=0,
        actualization=-1,
        mood=-1,
        earth=1,
		layer=Habits.Layer.physiology},
        new Habit{
        name="meat-heavy diet",
        id=380,
        satisfaction=3,
        decay=3,
        physiology=-1,
        safety=0,
        belonging=0,
        esteem=0,
        actualization=0,
        mood=1,
        earth=-2,
		layer=Habits.Layer.physiology},
        new Habit{
        name="sushi",
        id=394,
        satisfaction=1,
        decay=2,
        physiology=1,
        safety=0,
        belonging=0,
        esteem=0,
        actualization=1,
        mood=1,
        earth=-1,
		layer=Habits.Layer.physiology},
        new Habit{
        name="cupcakes",
        id=2473,
        satisfaction=2,
        decay=3,
        physiology=-2,
        safety=0,
        belonging=0,
        esteem=0,
        actualization=1,
        mood=2,
        earth=0},
        new Habit{
        name="BACON",
        id=2330,
        satisfaction=3,
        decay=2,
        physiology=-2,
        safety=0,
        belonging=0,
        esteem=0,
        actualization=2,
        mood=2,
        earth=-2,
		layer=Habits.Layer.physiology},
        new Habit{
        name="eat your veggies",
        id=2349,
        satisfaction=1,
        decay=2,
        physiology=2,
        safety=0,
        belonging=0,
        esteem=0,
        actualization=-2,
        mood=-2,
        earth=2,
		layer=Habits.Layer.physiology},
        new Habit{
        name="takeout",
        id=2344,
        satisfaction=2,
        decay=3,
        physiology=-1,
        safety=0,
        belonging=0,
        esteem=0,
        actualization=0,
        mood=1,
        earth=-1,
		layer=Habits.Layer.physiology},
        new Habit{
        name="home cooking",
        id=986,
        satisfaction=3,
        decay=1,
        physiology=1,
        safety=0,
        belonging=0,
        esteem=0,
        actualization=1,
        mood=1,
        earth=1,
		layer=Habits.Layer.physiology},
        new Habit{
        name="motorcycle",
        id=592,
        satisfaction=2,
        decay=3,
        physiology=-1,
        safety=2,
        belonging=0,
        esteem=0,
        actualization=0,
        mood=2,
        earth=-1,
		layer=Habits.Layer.safety},
        new Habit{
        name="bicycle",
        id=1969,
        satisfaction=1,
        decay=1,
        physiology=2,
        safety=1,
        belonging=0,
        esteem=0,
        actualization=0,
        mood=-1,
        earth=2,
		layer=Habits.Layer.safety},
        new Habit{
        name="train",
        id=1909,
        satisfaction=3,
        decay=1,
        physiology=0,
        safety=2,
        belonging=0,
        esteem=0,
        actualization=0,
        mood=-1,
        earth=2,
		layer=Habits.Layer.safety},
        new Habit{
        name="car",
        id=1926,
        satisfaction=2,
        decay=2,
        physiology=0,
        safety=2,
        belonging=0,
        esteem=0,
        actualization=0,
        mood=1,
        earth=-2,
		layer=Habits.Layer.safety},
        new Habit{
        name="electric moped",
        id=2070,
        satisfaction=1,
        decay=3,
        physiology=0,
        safety=1,
        belonging=0,
        esteem=0,
        actualization=1,
        mood=2,
        earth=-1,
		layer=Habits.Layer.safety},
        new Habit{
        name="scooter",
        id=2069,
        satisfaction=1,
        decay=2,
        physiology=1,
        safety=0,
        belonging=0,
        esteem=0,
        actualization=0,
        mood=0,
        earth=1,
		layer=Habits.Layer.safety},
        new Habit{
        name="airplane",
        id=3065,
        satisfaction=3,
        decay=3,
        physiology=-1,
        safety=2,
        belonging=0,
        esteem=0,
        actualization=0,
        mood=-1,
        earth=-2,
		layer=Habits.Layer.safety},
        new Habit{
        name="bus",
        id=1915,
        satisfaction=2,
        decay=1,
        physiology=0,
        safety=-1,
        belonging=0,
        esteem=0,
        actualization=-1,
        mood=-2,
        earth=1,
		layer=Habits.Layer.safety},
        new Habit{
        name="going out for drinks",
        id=418,
        satisfaction=2,
        decay=2,
        physiology=-2,
        safety=0,
        belonging=0,
        esteem=0,
        actualization=2,
        mood=2,
        earth=1,
		layer=Habits.Layer.belonging},
        new Habit{
        name="watching TV",
        id=1535,
        satisfaction=2,
        decay=3,
        physiology=0,
        safety=-1,
        belonging=0,
        esteem=0,
        actualization=-2,
        mood=0,
        earth=-1,
		layer=Habits.Layer.belonging},
        new Habit{
        name="skiing",
        id=3041,
        satisfaction=3,
        decay=1,
        physiology=-1,
        safety=1,
        belonging=0,
        esteem=0,
        actualization=2,
        mood=2,
        earth=-2,
		layer=Habits.Layer.belonging},
        new Habit{
        name="social dancing",
        id=1370,
        satisfaction=3,
        decay=2,
        physiology=1,
        safety=0,
        belonging=0,
        esteem=0,
        actualization=2,
        mood=2,
        earth=2,
		layer=Habits.Layer.belonging},
        new Habit{
        name="volleyball",
        id=595,
        satisfaction=1,
        decay=3,
        physiology=2,
        safety=1,
        belonging=0,
        esteem=0,
        actualization=1,
        mood=1,
        earth=1,
		layer=Habits.Layer.belonging},
        new Habit{
        name="camping",
        id=3061,
        satisfaction=2,
        decay=1,
        physiology=0,
        safety=0,
        belonging=0,
        esteem=0,
        actualization=0,
        mood=-1,
        earth=2,
		layer=Habits.Layer.belonging},
        new Habit{
        name="smoking",
        id=1963,
        satisfaction=1,
        decay=3,
        physiology=-2,
        safety=-1,
        belonging=0,
        esteem=0,
        actualization=-1,
        mood=1,
        earth=-2,
		layer=Habits.Layer.belonging},
        new Habit{
        name="shopping",
        id=2056,
        satisfaction=1,
        decay=2,
        physiology=0,
        safety=0,
        belonging=0,
        esteem=0,
        actualization=1,
        mood=1,
        earth=-1,
		layer=Habits.Layer.esteem},
        new Habit{
        name="yoga",
        id=2763,
        satisfaction=2,
        decay=2,
        physiology=2,
        safety=0,
        belonging=0,
        esteem=0,
        actualization=-2,
        mood=2,
        earth=1,
		layer=Habits.Layer.esteem},
        new Habit{
        name="cat ownership",
        id=652,
        satisfaction=3,
        decay=1,
        physiology=1,
        safety=0,
        belonging=0,
        esteem=0,
        actualization=1,
        mood=2,
        earth=-1,
		layer=Habits.Layer.esteem},
        new Habit{
        name="gardening",
        id=345,
        satisfaction=2,
        decay=1,
        physiology=2,
        safety=0,
        belonging=0,
        esteem=0,
        actualization=0,
        mood=1,
        earth=2,
		layer=Habits.Layer.esteem},
        new Habit{
        name="going on a cruise",
        id=2068,
        satisfaction=2,
        decay=3,
        physiology=-1,
        safety=0,
        belonging=0,
        esteem=0,
        actualization=-1,
        mood=0,
        earth=-2,
		layer=Habits.Layer.esteem},
        new Habit{
        name="getting a degree",
        id=447,
        satisfaction=3,
        decay=1,
        physiology=-1,
        safety=0,
        belonging=0,
        esteem=0,
        actualization=2,
        mood=-1,
        earth=0,
		layer=Habits.Layer.esteem},
        new Habit{
        name="champagne room",
        id=421,
        satisfaction=2,
        decay=3,
        physiology=-1,
        safety=0,
        belonging=0,
        esteem=0,
        actualization=1,
        mood=1,
        earth=-1,
		layer=Habits.Layer.esteem},
        new Habit{
        name="bubble baths",
        id=2039,
        satisfaction=1,
        decay=2,
        physiology=0,
        safety=0,
        belonging=0,
        esteem=0,
        actualization=0,
        mood=1,
        earth=-1,
		layer=Habits.Layer.esteem},
        new Habit{
        name="cleaning the house",
        id=2900,
        satisfaction=1,
        decay=3,
        physiology=0,
        safety=0,
        belonging=0,
        esteem=0,
        actualization=1,
        mood=-2,
        earth=2,
		layer=Habits.Layer.esteem
		}
		
    };


    // Start is called before the first frame update
    void Awake()
    {
		foreach (Habit habit in habits)
		{
			habit.sprite = MaslowManager.Instance.emojiSprites[habit.id];
		}
    }

    // Update is called once per frame
    void Update()
    {

    }
}
