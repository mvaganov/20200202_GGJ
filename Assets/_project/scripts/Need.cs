using UnityEngine;

/// <summary>
/// Need is a named need level that changes in value over time.
/// </summary>
	[System.Serializable]
	public class Need {
		public string name;
        public Habits.Layer layer;
		public Color color;
        /// <summary>
        /// How met is the need? Not the same as the strength of the habits feeding it.
        /// </summary>
		public float value;
		public float lossPerSecond;
		public float gainPerClick;
        /// <summary>
        /// NeedUI gets hooked up to a NeedUI bar when a NeedsTriangle is shown.
        /// </summary>
		public NeedUI ui;
		/// <summary>
		/// The currently strongest habit the person has for meeting the need.
		/// </summary>
		public Habits.Habit habitPrimary;
		/// <summary>
		/// The second strongest habit the person ahs for meeting the need. When it reaches 0, becomes inert as old habit and secondary becomes empty.
		/// </summary>
		public Habits.Habit habitSecondary;
		/// <summary>
		/// A former habit for meeting the need that has reached zero. TODO: Could be displayed grayed out.
		/// </summary>
        /// public       
		public Habits.Habit habitOld;
        /// <summary>
        /// Strength of the primary habit. They should add up to 100 currently.
        /// </summary>
        public float habitPrimaryValue = 100f;
        /// <summary>
        /// Strength of the secondary habit. They should add up to 100 currently.
        /// </summary>
        public float habitSecondaryValue = 0f;
		/// <summary>
		/// The need below it that limits its maximum.
		/// </summary>
		public Need dependency;

		/// <summary>
		/// Debug mode click to increase needs for testing.
		/// </summary>
		public bool clickToIncrease = false;

		public void Update(float deltaTime) {
			value -= lossPerSecond * deltaTime;
			if (value < 0) { value = 0; }
			float max = dependency != null ? dependency.value : 1;
            if (ui != null)
            {
                if (value > max) { value = max; }
                ui.progressbar.fillAmount = Mathf.Clamp(value, 0, 1);
            }
		}
		public void Click() {
			if (clickToIncrease)
			{
				value += gainPerClick;
			}
		}
		public void Use(NeedUI ui, Need dependency) {
			this.dependency = dependency;
			this.ui = ui;
			ui.text.text = name;
			ui.progressbar.color = color;
		}
	}