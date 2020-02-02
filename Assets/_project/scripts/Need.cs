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
        /// <summary>
        /// A link to the parent maslow
        /// </summary>
        public MaslowMeter maslow;

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

        private static float habitValueMax = 100f;

		/// <summary>
		/// Debug mode click to increase needs for testing.
		/// </summary>
		public bool clickToIncrease = false;

		public void Update(float deltaTime) {
            if (habitPrimary == null)
            {
                value = 0f;
            }
            else
            {
                value = habitPrimaryValue / MaslowMeter.maxHabitValue;
            }
			//value -= lossPerSecond * deltaTime;
			if (value < 0) { value = 0; }
			//float max = dependency != null ? dependency.value : 1;
            if (ui != null)
            {
                //if (value > max) { value = max; }
                
                ui.progressbar.fillAmount = Mathf.Clamp(value, 0, 1);
            }
		}
		public void Click() {
            GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
            MaslowMeter playerMaslow = playerGO.GetComponentInChildren<MaslowMeter>();

			if (clickToIncrease)
			{
				value += gainPerClick;
			}
            // If we click another maslow's habit to get it
            if (!maslow.isPlayer)
            {
                //then influence player
                playerMaslow.InfluenceMaslow(maslow,layer,maslow.happy,maslow.safety,maslow.safety);
            }
            // else we clicked the player's habits to influence our interactingWith partner
            else
            {
                if(maslow.interactingWith != null)
                {
                    //Debug.Log("Interacting with " + maslow.interactingWith.happy);
                    // If we clicked ourselves, we try to influence the other person we are meeting with.
                    maslow.interactingWith.InfluenceMaslow(playerMaslow,layer,playerMaslow.happy,playerMaslow.safety,playerMaslow.safety);
                }
                else
                {
                    //Debug.Log("Not interacting with anyone to send to.");
                }
            }
		}

        Sprite needSprite;
        int needSpriteIndex = 0;

        public Sprite GetNeedSprite()
        {
            switch (name){
                case "physiology":     
                    needSprite = MaslowManager.Instance.physiology;
                    break;
                case "safety":     
                    needSprite = MaslowManager.Instance.safety;
                    break;
                case "belonging":     
                    needSprite = MaslowManager.Instance.belonging;
                    break;
                case "esteem":     
                    needSprite = MaslowManager.Instance.esteem;
                    break;
                case "actualization":     
                    needSprite = MaslowManager.Instance.actualization;
                    break;
                case "happy":     
                    needSprite = MaslowManager.Instance.happies[9];
                    break;
                case "earth":     
                    needSprite = MaslowManager.Instance.cooling;
                    break;
                default:
                    needSprite = MaslowManager.Instance.cooling;
                    break;
            }
            return needSprite;
        }
        
		public void Use(NeedUI ui, Need dependency) {
			this.dependency = dependency;
            string uiText = "";
			this.ui = ui;
            if (GetNeedSprite()!=null)
            {
                uiText = uiText + "<sprite=" + MaslowManager.Instance.GetEmojiSpriteIndex(GetNeedSprite()) + ">";
            }
            
            if (habitPrimary != null)
            {
                //Debug.Log("Use primary name:" + habitPrimary.name);
                //Debug.Log("Use sprite:" + habitPrimary.sprite);

                uiText = "<sprite=" + MaslowManager.Instance.GetEmojiSpriteIndex(habitPrimary.sprite) + "> " + uiText;
            }
            else
            {
                uiText =  "    " + uiText;
            }
            if (habitSecondary != null)
            {
                uiText = uiText + " <sprite=" + MaslowManager.Instance.GetEmojiSpriteIndex(habitSecondary.sprite) + ">";
            }
            else
            {
                uiText = uiText + "    ";
            }
			ui.text.text = uiText; // no longer name
            if(habitPrimary == null)
            {
                ui.progressbar.color = new Color(0f,0f,0f,0.05f);
                habitPrimaryValue = 0f;
            }
            else
            {
			    ui.progressbar.color = color;
            }
		}
	}